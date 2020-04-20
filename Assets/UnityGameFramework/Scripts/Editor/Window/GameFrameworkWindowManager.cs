using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityGameFramework.Editor;

namespace UnityGameFramework.Editor
{
    public class GameFrameworkWindowManager : GameFrameworkWindow
    {
        private const float LeftWightContent = 150f;

        private int m_WindowTypeNameIndex = 0;

        private string[] m_Titles = new string[0];

        private static List<GameFrameworkMeumWindow> m_GameFrameworkMeumWindows = new List<GameFrameworkMeumWindow>();

        private GameFrameworkMeumWindow m_CurrentWindow = null;

        private Vector2 m_ProtobufScroll;

        [MenuItem("Assets/Open Tools Window", false, int.MaxValue)]
        [MenuItem("Game Framework/Window", false, 11)]
        static void Init()
        {
            EditorWindow window = EditorWindow.GetWindow<GameFrameworkWindowManager>(false, "Tools Window", true);
            window.minSize = new Vector2(LeftWightContent + 50f, 200f);
        }

        public override void OnGUI()
        {
            base.OnGUI();

            BeginWindows();

            m_ProtobufScroll =
                EditorGUILayout.BeginScrollView(m_ProtobufScroll, GUILayout.MinWidth(LeftWightContent),
                    GUILayout.Height(position.height));
            {
                EditorGUILayout.BeginVertical();
                {
                    m_WindowTypeNameIndex =
                        GUILayout.SelectionGrid(m_WindowTypeNameIndex, m_Titles, 1);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();

            if (m_GameFrameworkMeumWindows.Count > 0)
            {
                Rect windowRect = new Rect(LeftWightContent, 0, position.width - LeftWightContent, position.height);

                m_CurrentWindow = m_GameFrameworkMeumWindows[m_WindowTypeNameIndex];

                if (m_CurrentWindow != null)
                {
                    GUILayout.Window(1, windowRect, m_CurrentWindow.Window, m_CurrentWindow.Title,
                        GUILayout.ExpandWidth(true),
                        GUILayout.ExpandHeight(true));
                }
            }

            EndWindows();

            Repaint();
        }

        public override void OnSelectionChange()
        {
            base.OnSelectionChange();

            if (m_CurrentWindow != null)
            {
                m_CurrentWindow.OnSelectionChange();
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            RefreshTypeNames();
            if (m_CurrentWindow != null)
            {
                m_CurrentWindow.OnEnable();
            }
        }

        public override void OnFocus()
        {
            base.OnFocus();
            if (m_CurrentWindow != null)
            {
                m_CurrentWindow.OnEnable();
            }
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();
            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            var windowTypes = Type.GetEditorTypes(typeof(GameFrameworkMeumWindow));
            m_GameFrameworkMeumWindows.Clear();
            foreach (var windowType in windowTypes)
            {
                GameFrameworkMeumWindow gameFramework =
                    ((GameFrameworkMeumWindow) ScriptableObject.CreateInstance(windowType));
                m_GameFrameworkMeumWindows.Add(gameFramework);
            }

            m_Titles = (from window in m_GameFrameworkMeumWindows select window.Title).ToArray();
        }
    }
}