using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GameFramework;
using UnityEditor;

namespace UnityGameFramework.Editor
{
    [Serializable]
    internal sealed partial class EventDataInfo : ScriptableObject
    {
        public static EventDataInfo Load()
        {
            string tempPath = Utility.Path.GetCombinePath("Assets", "Temp");
            string path = Utility.Path.GetCombinePath(tempPath, "EventDataInfos.asset");
            if (!AssetDatabase.IsValidFolder(tempPath))
            {
                AssetDatabase.CreateFolder("Assets", "Temp");
            }

            EventDataInfo eventDataInfo;
            if (!File.Exists(path))
            {
                eventDataInfo = ScriptableObject.CreateInstance<EventDataInfo>();
                AssetDatabase.CreateAsset(eventDataInfo, path);
            }
            else
            {
                eventDataInfo = AssetDatabase.LoadAssetAtPath<EventDataInfo>(path);
            }

            return eventDataInfo;
        }

        [SerializeField] private string m_Namespace = "SG1";

        [SerializeField] private string m_Version = "1.0";

        [SerializeField] private string m_Descriptions = string.Empty;

        [SerializeField] private string m_ClassName = string.Empty;

        [SerializeField] private string m_GeneratePath = string.Empty;

        [SerializeField] private List<EventArgsInfo> m_EventArgsInfos = new List<EventArgsInfo>();

        public string Namespace
        {
            get { return m_Namespace; }
            set { m_Namespace = value; }
        }

        public string Version
        {
            get { return m_Version; }
            set { m_Version = value; }
        }

        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }

        public string Descriptions
        {
            get { return m_Descriptions; }
            set { m_Descriptions = value; }
        }

        public List<EventArgsInfo> EventArgsInfos
        {
            get { return m_EventArgsInfos; }
            set { m_EventArgsInfos = value; }
        }

        public string GeneratePath
        {
            get { return m_GeneratePath; }
            set
            {
                if (Directory.Exists(Utility.Path.GetRegularPath(value)))
                {
                    m_GeneratePath = value;
                }
            }
        }
    }
}