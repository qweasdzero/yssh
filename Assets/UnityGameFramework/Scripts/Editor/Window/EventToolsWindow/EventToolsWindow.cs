using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityGameFramework.Editor
{
    public partial class EventToolsWindow : GameFrameworkMeumWindow
    {
        private StringBuilder m_Code = new StringBuilder();

        private Regex m_Regex = new Regex(@"^[a-zA-Z_][a-zA-Z_\w]{0,32}");

        private Vector2 m_LeftScroll = Vector2.zero;

        private Vector2 m_RightScroll = Vector2.zero;

        private ReorderableList m_EventArgsInfoReorderableList;

        [SerializeField] private EventDataInfo m_EventDataInfo;

        private SerializedObject m_EventDataInfoSerializedObject;

        private SerializedProperty m_EventArgsInfos;

        public override string Title
        {
            get { return "Event Tools"; }
        }

        public override void OnEnable()
        {
            base.OnEnable();

            m_EventDataInfo = EventDataInfo.Load();
            m_EventDataInfoSerializedObject = new SerializedObject(m_EventDataInfo);
            m_EventArgsInfos = m_EventDataInfoSerializedObject.FindProperty("m_EventArgsInfos");

            m_EventArgsInfoReorderableList = new ReorderableList(m_EventDataInfoSerializedObject, m_EventArgsInfos,
                true, true, true, true)
            {
                drawHeaderCallback = OnDrawHeaderCallback,
                onRemoveCallback = OnRemoveCallback,
                drawElementCallback = DrawParamsElement,
                elementHeight = EventDataInfo.EventArgsInfo.WIDTH,
            };
        }

        private void DrawParamsElement(Rect rect, int index, bool isactive, bool isfocused)
        {
            SerializedProperty element =
                m_EventArgsInfoReorderableList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            SerializedProperty name = element.FindPropertyRelative("m_Name");

            SerializedProperty languageKeyword = element.FindPropertyRelative("m_LanguageKeyword");

            SerializedProperty comment = element.FindPropertyRelative("m_Comment");

            Rect pos = new Rect(rect.x, rect.y, 300f, EditorGUIUtility.singleLineHeight);

            languageKeyword.stringValue = EditorGUI.TextField(pos, "Keyword", languageKeyword.stringValue);

            pos.y += EditorGUIUtility.singleLineHeight + 4;

            name.stringValue = EditorGUI.TextField(pos, "Name", name.stringValue);

            pos.y += EditorGUIUtility.singleLineHeight + 4;

            comment.stringValue = EditorGUI.TextField(pos, "Comment", comment.stringValue);
        }

        private void OnRemoveCallback(ReorderableList list)
        {
//            if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete the eventArgs?", "Yes",
//                "No"))
//            {
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
//            }
        }

        private void OnDrawHeaderCallback(Rect rect)
        {
            GUI.Label(rect, "EventArgs");
        }

        public override void Window(int unusedWindowID)
        {
            GUI.FocusWindow(unusedWindowID);
            EditorGUILayout.BeginHorizontal();
            {
                LeftWindow();

                RightWindow();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void LeftWindow()
        {
            m_EventDataInfoSerializedObject.Update();

            EditorGUILayout.BeginHorizontal("box", GUILayout.Width(530f), GUILayout.ExpandHeight(true));
            {
                m_LeftScroll =
                    EditorGUILayout.BeginScrollView(m_LeftScroll, GUILayout.ExpandWidth(true));
                {
                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.BeginHorizontal(GUILayout.Width(500f));
                        {
                            EditorGUILayout.LabelField("Namespace:", EditorStyles.boldLabel, GUILayout.MaxWidth(100f));
                            m_EventDataInfo.Namespace = GUILayout.TextField(m_EventDataInfo.Namespace);

                            EditorGUILayout.Space();

                            EditorGUILayout.LabelField("Version:", EditorStyles.boldLabel, GUILayout.MaxWidth(100f));
                            m_EventDataInfo.Version = GUILayout.TextField(m_EventDataInfo.Version);
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space();

                        EditorGUILayout.BeginHorizontal(GUILayout.Width(500f));
                        {
                            EditorGUILayout.LabelField("Descriptions:", EditorStyles.boldLabel,
                                GUILayout.MaxWidth(100f));
                            m_EventDataInfo.Descriptions =
                                GUILayout.TextArea(m_EventDataInfo.Descriptions, GUILayout.Height(40f));
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space();

                        EditorGUILayout.BeginHorizontal(GUILayout.Width(500f));
                        {
                            EditorGUILayout.LabelField("ClassName:", EditorStyles.boldLabel, GUILayout.MaxWidth(100f));
                            m_EventDataInfo.ClassName = GUILayout.TextField(m_EventDataInfo.ClassName);
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal(GUILayout.Width(500f));
                        {
                            EditorGUILayout.LabelField("GeneratePath:", EditorStyles.boldLabel,
                                GUILayout.MaxWidth(100f));
                            m_EventDataInfo.GeneratePath = GUILayout.TextField(m_EventDataInfo.GeneratePath);
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space();
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginHorizontal(GUILayout.Width(500f));
                    {
                        if (GUILayout.Button("Clear Empty"))
                        {
                            ClearEmptyEventArgs();
                        }

                        if (GUILayout.Button("Clear All"))
                        {
                            ClearAllEventArgs();
                        }

                        if (GUILayout.Button("Generate"))
                        {
                            Generate();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginVertical(GUILayout.Width(500f));
                    {
                        m_EventArgsInfoReorderableList.DoLayoutList();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndHorizontal();

            m_EventDataInfoSerializedObject.ApplyModifiedProperties();
        }

        private void ClearAllEventArgs()
        {
            m_EventDataInfo.EventArgsInfos.Clear();
        }

        private void ClearEmptyEventArgs()
        {
            for (int i = m_EventDataInfo.EventArgsInfos.Count - 1; i >= 0; i--)
            {
                EventDataInfo.EventArgsInfo eventArgsInfo = m_EventDataInfo.EventArgsInfos[i];
                if (string.IsNullOrWhiteSpace(eventArgsInfo.Name) ||
                    string.IsNullOrWhiteSpace(eventArgsInfo.LanguageKeyword))
                {
                    m_EventDataInfo.EventArgsInfos.RemoveAt(i);
                }
            }
        }

        private void RightWindow()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                m_RightScroll =
                    EditorGUILayout.BeginScrollView(m_RightScroll, GUILayout.ExpandWidth(true),
                        GUILayout.ExpandHeight(true));
                {
                    if (string.IsNullOrEmpty(m_EventDataInfo.Namespace) ||
                        string.IsNullOrEmpty(m_EventDataInfo.ClassName) ||
                        string.IsNullOrEmpty(m_EventDataInfo.Version) ||
                        !m_Regex.IsMatch(m_EventDataInfo.Namespace) ||
                        !m_Regex.IsMatch(m_EventDataInfo.ClassName))
                    {
                        EditorGUILayout.HelpBox("EventData is error", MessageType.Error);
                    }
                    else
                    {
                        EditorGUILayout.TextArea(m_Code.ToString(), EditorStyles.boldLabel);
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        private void Generate()
        {
            string className = GameFramework.Utility.Text.Format("{0}EventArgs", m_EventDataInfo.ClassName);

            m_Code.Length = 0;
            m_Code.AppendFormat("// Version:{0}", m_EventDataInfo.Version).AppendLine();
            m_Code.AppendLine("using GameFramework.Event;");
            m_Code.AppendLine();
            m_Code.AppendLine("namespace SG1");
            m_Code.AppendLine("{");
            if (!string.IsNullOrEmpty(m_EventDataInfo.Descriptions))
            {
                m_Code.Append(' ', 4).AppendLine("/// <summary>");

                m_Code.Append(' ', 4).Append("/// ").Append(m_EventDataInfo.Descriptions).AppendLine("。");

                m_Code.Append(' ', 4).AppendLine("/// <summary>");
            }

            m_Code.Append(' ', 4).AppendFormat("public sealed class {0} : GameEventArgs",
                className).AppendLine();

            m_Code.Append(' ', 4).AppendLine("{");

            if (!string.IsNullOrEmpty(m_EventDataInfo.Descriptions))
            {
                m_Code.Append(' ', 8).AppendLine("/// <summary>");

                m_Code.Append(' ', 8).Append("/// ").Append(m_EventDataInfo.Descriptions).AppendLine("编号。");

                m_Code.Append(' ', 8).AppendLine("/// <summary>");
            }

            m_Code.Append(' ', 8)
                .AppendFormat("public static readonly int EventId = typeof({0}).GetHashCode();", className)
                .AppendLine();

            m_Code.AppendLine();

            if (!string.IsNullOrEmpty(m_EventDataInfo.Descriptions))
            {
                m_Code.Append(' ', 8).AppendLine("/// <summary>");

                m_Code.Append(' ', 8).Append("/// ").Append("获取").Append(m_EventDataInfo.Descriptions)
                    .AppendLine("编号。");

                m_Code.Append(' ', 8).AppendLine("/// <summary>");
            }

            m_Code.Append(' ', 8).AppendLine("public override int Id");

            m_Code.Append(' ', 8).AppendLine("{");
            m_Code.Append(' ', 12).AppendLine("get");
            m_Code.Append(' ', 12).AppendLine("{");
            m_Code.Append(' ', 16).AppendLine("return EventId;");
            m_Code.Append(' ', 12).AppendLine("}");
            m_Code.Append(' ', 8).AppendLine("}");

            foreach (var eventArgsInfo in m_EventDataInfo.EventArgsInfos)
            {
                if (string.IsNullOrEmpty(eventArgsInfo.Name) ||
                    string.IsNullOrEmpty(eventArgsInfo.LanguageKeyword) ||
                    !m_Regex.IsMatch(eventArgsInfo.Name) ||
                    !m_Regex.IsMatch(eventArgsInfo.LanguageKeyword))
                {
                    continue;
                }

                m_Code.AppendLine();

                if (!string.IsNullOrEmpty(eventArgsInfo.Comment))
                {
                    m_Code.Append(' ', 8).AppendLine("/// <summary>");

                    m_Code.Append(' ', 8).Append("/// ").Append("获取").Append(eventArgsInfo.Comment)
                        .AppendLine("。");

                    m_Code.Append(' ', 8).AppendLine("/// <summary>");
                }

                m_Code.Append(' ', 8)
                    .AppendFormat("public {0} {1}", eventArgsInfo.LanguageKeyword, eventArgsInfo.Name)
                    .AppendLine();

                m_Code.Append(' ', 8).AppendLine("{");
                m_Code.Append(' ', 12).AppendLine("get;");
                m_Code.Append(' ', 12).AppendLine("private set;");
                m_Code.Append(' ', 8).AppendLine("}");
            }

            m_Code.AppendLine();
            m_Code.Append(' ', 8).AppendLine("public override void Clear()");
            m_Code.Append(' ', 8).AppendLine("{");
            foreach (var eventArgsInfo in m_EventDataInfo.EventArgsInfos)
            {
                if (string.IsNullOrEmpty(eventArgsInfo.Name) ||
                    string.IsNullOrEmpty(eventArgsInfo.LanguageKeyword) ||
                    !m_Regex.IsMatch(eventArgsInfo.Name) ||
                    !m_Regex.IsMatch(eventArgsInfo.LanguageKeyword))
                {
                    continue;
                }

                m_Code.Append(' ', 12).AppendFormat("{0} = default({1});", eventArgsInfo.Name,
                    eventArgsInfo.LanguageKeyword).AppendLine();
            }

            m_Code.Append(' ', 8).AppendLine("}");

            m_Code.AppendLine();
            m_Code.Append(' ', 8).AppendFormat("public {0} Fill()", className).AppendLine();
            m_Code.Append(' ', 8).AppendLine("{");
            m_Code.Append(' ', 12).AppendLine("return this");
            m_Code.Append(' ', 8).AppendLine("}");

            m_Code.Append(' ', 4).AppendLine("}");
            m_Code.AppendLine("}");
        }
    }
}