using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor
{
    public partial class ReferenceFinderData
    {
        //缓存路径
        private const string c_CachePath = "Library/ReferenceFinderCache";

        //资源引用信息字典
        private readonly Dictionary<string, AssetDescription> m_AssetDict = new Dictionary<string, AssetDescription>();

        public Dictionary<string, AssetDescription> AssetDict
        {
            get { return m_AssetDict; }
        }

        /// <summary>
        /// 收集资源引用信息并更新缓存
        /// </summary>
        public void CollectDependenciesInfo()
        {
            try
            {
                ReadFromCache();
                var allAssets = AssetDatabase.GetAllAssetPaths();
                int totalCount = allAssets.Length;
                for (int i = 0; i < allAssets.Length; i++)
                {
                    //每遍历100个Asset，更新一下进度条，同时对进度条的取消操作进行处理
                    if ((i % 100 == 0) && EditorUtility.DisplayCancelableProgressBar("Refresh",
                            string.Format("Collecting {0} assets", i), (float) i / totalCount))
                    {
                        EditorUtility.ClearProgressBar();
                        return;
                    }

                    if (File.Exists(allAssets[i]))
                        ImportAsset(allAssets[i]);
                    if (i % 2000 == 0)
                        GC.Collect();
                }

                //将信息写入缓存
                EditorUtility.DisplayCancelableProgressBar("Refresh", "Write to cache", 1f);
                WriteToChache();
                //生成引用数据
                EditorUtility.DisplayCancelableProgressBar("Refresh", "Generating asset reference info", 1f);
                UpdateReferenceInfo();
                EditorUtility.ClearProgressBar();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                EditorUtility.ClearProgressBar();
            }
        }

        /// <summary>
        /// 通过依赖信息更新引用信息
        /// </summary>
        private void UpdateReferenceInfo()
        {
            foreach (var asset in m_AssetDict)
            {
                foreach (var assetGuid in asset.Value.Dependencies)
                {
                    m_AssetDict[assetGuid].References.Add(asset.Key);
                }
            }
        }

        /// <summary>
        /// 生成并加入引用信息
        /// </summary>
        /// <param name="path"></param>
        private void ImportAsset(string path)
        {
            //通过path获取guid进行储存
            string guid = AssetDatabase.AssetPathToGUID(path);
            //获取该资源的最后修改时间，用于之后的修改判断
            Hash128 assetDependencyHash = AssetDatabase.GetAssetDependencyHash(path);
            //如果assetDict没包含该guid或包含了修改时间不一样则需要更新
            if (!m_AssetDict.ContainsKey(guid) || m_AssetDict[guid].AssetDependencyHash != assetDependencyHash)
            {
                //将每个资源的直接依赖资源转化为guid进行储存
                var guids = AssetDatabase.GetDependencies(path, false).Select(p => AssetDatabase.AssetPathToGUID(p))
                    .ToList();

                //生成asset依赖信息，被引用需要在所有的asset依赖信息生成完后才能生成
                AssetDescription ad = new AssetDescription();
                ad.Name = Path.GetFileNameWithoutExtension(path);
                ad.Path = path;
                ad.AssetDependencyHash = assetDependencyHash;
                ad.Dependencies = guids;

                if (m_AssetDict.ContainsKey(guid))
                    m_AssetDict[guid] = ad;
                else
                    m_AssetDict.Add(guid, ad);
            }
        }

        /// <summary>
        /// 读取缓存信息
        /// </summary>
        /// <returns></returns>
        public bool ReadFromCache()
        {
            m_AssetDict.Clear();
            if (File.Exists(c_CachePath))
            {
                var serializedGuid = new List<string>();
                var serializedDependencyHash = new List<Hash128>();
                var serializedDenpendencies = new List<int[]>();
                //反序列化数据
                using (FileStream fs = File.OpenRead(c_CachePath))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    EditorUtility.DisplayCancelableProgressBar("Import Cache", "Reading Cache", 0);
                    serializedGuid = (List<string>) bf.Deserialize(fs);
                    serializedDependencyHash = (List<Hash128>) bf.Deserialize(fs);
                    serializedDenpendencies = (List<int[]>) bf.Deserialize(fs);
                    EditorUtility.ClearProgressBar();
                }

                for (int i = 0; i < serializedGuid.Count; ++i)
                {
                    string path = AssetDatabase.GUIDToAssetPath(serializedGuid[i]);
                    if (!string.IsNullOrEmpty(path))
                    {
                        var ad = new AssetDescription();
                        ad.Name = Path.GetFileNameWithoutExtension(path);
                        ad.Path = path;
                        ad.AssetDependencyHash = serializedDependencyHash[i];
                        m_AssetDict.Add(serializedGuid[i], ad);
                    }
                }

                for (int i = 0; i < serializedGuid.Count; ++i)
                {
                    string guid = serializedGuid[i];
                    if (m_AssetDict.ContainsKey(guid))
                    {
                        var guids = serializedDenpendencies[i].Select(index => serializedGuid[index])
                            .Where(g => m_AssetDict.ContainsKey(g)).ToList();
                        m_AssetDict[guid].Dependencies = guids;
                    }
                }

                UpdateReferenceInfo();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        private void WriteToChache()
        {
            if (File.Exists(c_CachePath))
                File.Delete(c_CachePath);

            var serializedGuid = new List<string>();
            var serializedDependencyHash = new List<Hash128>();
            var serializedDenpendencies = new List<int[]>();
            //辅助映射字典
            var guidIndex = new Dictionary<string, int>();
            //序列化
            using (FileStream fs = File.OpenWrite(c_CachePath))
            {
                foreach (var pair in m_AssetDict)
                {
                    guidIndex.Add(pair.Key, guidIndex.Count);
                    serializedGuid.Add(pair.Key);
                    serializedDependencyHash.Add(pair.Value.AssetDependencyHash);
                }

                foreach (var guid in serializedGuid)
                {
                    int[] indexes = m_AssetDict[guid].Dependencies.Select(s => guidIndex[s]).ToArray();
                    serializedDenpendencies.Add(indexes);
                }

                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, serializedGuid);
                bf.Serialize(fs, serializedDependencyHash);
                bf.Serialize(fs, serializedDenpendencies);
            }
        }

        /// <summary>
        /// 更新引用信息状态
        /// </summary>
        /// <param name="guid"></param>
        public void UpdateAssetState(string guid)
        {
            AssetDescription ad;
            if (m_AssetDict.TryGetValue(guid, out ad) && ad.State != AssetState.NODATA)
            {
                if (File.Exists(ad.Path))
                {
                    //修改时间与记录的不同为修改过的资源
                    if (ad.AssetDependencyHash != AssetDatabase.GetAssetDependencyHash(ad.Path))
                    {
                        ad.State = AssetState.CHANGED;
                    }
                    else
                    {
                        //默认为普通资源
                        ad.State = AssetState.NORMAL;
                    }
                }
                //不存在为丢失
                else
                {
                    ad.State = AssetState.MISSING;
                }
            }

            //字典中没有该数据
            else if (!m_AssetDict.TryGetValue(guid, out ad))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ad = new AssetDescription();
                ad.Name = Path.GetFileNameWithoutExtension(path);
                ad.Path = path;
                ad.State = AssetState.NODATA;
                m_AssetDict.Add(guid, ad);
            }
        }

        /// <summary>
        /// 根据引用信息状态获取状态描述
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string GetInfoByState(AssetState state)
        {
            if (state == AssetState.CHANGED)
            {
                return "<color=#F0672AFF>Changed</color>";
            }
            else if (state == AssetState.MISSING)
            {
                return "<color=#FF0000FF>Missing</color>";
            }
            else if (state == AssetState.NODATA)
            {
                return "<color=#FFE300FF>No Data</color>";
            }

            return "Normal";
        }
    }
}