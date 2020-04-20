using System;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor
{
    internal sealed partial class EventDataInfo
    {
        [Serializable]
        internal class EventArgsInfo
        {   
            public static readonly float WIDTH = EditorGUIUtility.singleLineHeight * 3 + 4 * 2 + 4;
            
            [SerializeField] private string m_LanguageKeyword = string.Empty;

            [SerializeField] private string m_Name = string.Empty;

            [SerializeField] private string m_Comment = string.Empty;

            public string LanguageKeyword
            {
                get { return m_LanguageKeyword; }
                set { m_LanguageKeyword = value; }
            }

            public string Name
            {
                get { return m_Name; }
                set { m_Name = value; }
            }

            public string Comment
            {
                get { return m_Comment; }
                set { m_Comment = value; }
            }
        }
    }
}