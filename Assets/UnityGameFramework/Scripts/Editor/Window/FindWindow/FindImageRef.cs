using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor
{
    public class FindImageRef : GameFrameworkMeumWindow
    {
        private static List<GameObject> findResult = new List<GameObject>();
        private string m_Guid;
        private Dictionary<GameObject, bool> Select;

        void DrawGUI()
        {
            if (Selection.activeObject == null)
            {
                GUILayout.Label(" select a guid file from Project Window.");
                return;
            }

            //判断选中项是否为图片
            name = Selection.activeObject.name;
            string guid = Get();

            if (guid == null)
            {
                GUILayout.Label(" select a guid file from Project Window.");
                return;
            }

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            //列出脚本名称和“Find”按钮
            GUILayout.Label(name);
            GUILayout.Space(10);
            bool click = GUILayout.Button("Find");
            GUILayout.EndHorizontal();


            //列出搜索结果
            Select = new Dictionary<GameObject, bool>();
            if (guid == m_Guid)
            {
                if (findResult != null && findResult.Count > 0)
                {
                    GUILayout.BeginScrollView(Vector2.zero, GUIStyle.none);
                    foreach (GameObject go in findResult)
                    {
                        GUILayout.Label(go.name);
                        Select.Add(go, GUILayout.Button("Select"));
                    }

                    GUILayout.EndScrollView();
                }
                else
                {
                    GUILayout.Label("Can't Find Prefab");
                }
            }

            foreach (var item in Select)
            {
                if (item.Value && null != item.Key)
                {
                    EditorGUIUtility.PingObject(item.Key);
                }
            }

            if (click)
            {
                m_Guid = guid;
                findResult = new List<GameObject>();
                EditorApplication.update += Search;
            }

            GUILayout.EndVertical();
        }

        private string Get()
        {
            var guids = AssetDatabase.FindAssets(name);
            for (int i = 0; i < guids.Length; i++)
            {
                var Path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (AssetDatabase.LoadAssetAtPath<Object>(Path) == Selection.activeObject)
                {
                    return guids[i];
                }
            }

            return null;
        }

        private void Search()
        {
            var guids = AssetDatabase.FindAssets("t:GameObject");
            var tp = typeof(GameObject);
            int index = 0;
            int total = guids.Length;
            foreach (var guid in guids)
            {
                bool isCancle = EditorUtility.DisplayCancelableProgressBar("执行中...", "正在查找", (float) index / total);
                index++;
                var path = AssetDatabase.GUIDToAssetPath(guid);
                string[] strs = File.ReadAllLines(path);

                foreach (var str in strs)
                {
                    if (str.Contains(m_Guid))
                    {
                        var obj = AssetDatabase.LoadAssetAtPath(path, tp) as GameObject;
                        if (!findResult.Contains(obj))
                        {
                            findResult.Add(obj);
                        }
                    }
                }

                if (isCancle || index >= total)
                {
                    EditorUtility.ClearProgressBar();
                    AssetDatabase.RemoveUnusedAssetBundleNames();
                    EditorApplication.update -= Search;
                }
            }
        }

        public override string Title
        {
            get { return "Find Script Tool"; }
        }
        public override void Window(int unusedWindowID)
        {
            GUI.FocusWindow(unusedWindowID);
            DrawGUI();
        }
    }
}