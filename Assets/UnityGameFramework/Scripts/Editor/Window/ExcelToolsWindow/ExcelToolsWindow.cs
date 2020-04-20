using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using GameFramework;
using UnityEditor;
using UnityEngine;

//TODO:添加其他文件的支持。
//{"TXT", "JSON", "CSV", "XML", "LUA"};

namespace UnityGameFramework.Editor
{
    public sealed partial class ExcelToolsWindow : GameFrameworkMeumWindow
    {
        /// <summary>
        /// 输出格式索引。
        /// </summary>
        private static int m_IndexOfFormatOption = 0;

        /// <summary>
        /// 输出格式。
        /// </summary>
        private static string[] m_FormatOption = new string[] {"TXT", "XML", "Enum"};

        /// <summary>
        /// 编码索引。
        /// </summary>
        private static int m_IndexOfEncoding = 0;

        /// <summary>
        /// 编码选项。
        /// </summary>
        private static string[] m_EncodingOption = new string[] {"UTF-8", "GB2312"};

        /// <summary>
        /// Excel文件路径。
        /// </summary>
        private string m_FilePath = string.Empty;

        /// <summary>
        /// Execl数据。
        /// </summary>
        private ExcelData m_ExcelData;

        /// <summary>
        /// 选中的Excel表格编号。
        /// </summary>
        private int m_SelectionExcelIndex = 0;

        /// <summary>
        /// 锁定当前文件。
        /// </summary>
        private bool m_Look = false;

        private Vector2 m_ExcelScroll = Vector2.zero;

        public override string Title
        {
            get { return "Execl Tools"; }
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
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(100f), GUILayout.ExpandHeight(true));
            {
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Format option:", EditorStyles.boldLabel, GUILayout.Width(100f));
                    m_IndexOfFormatOption =
                        EditorGUILayout.Popup(m_IndexOfFormatOption, m_FormatOption, GUILayout.Width(100f));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Encoding:", EditorStyles.boldLabel, GUILayout.Width(100f));
                    m_IndexOfEncoding =
                        EditorGUILayout.Popup(m_IndexOfEncoding, m_EncodingOption, GUILayout.Width(100f));
                }
                GUILayout.EndHorizontal();

                m_Look = EditorGUILayout.ToggleLeft(new GUIContent("Look excel file"), m_Look);

                DrawExcelInfo();
            }
            EditorGUILayout.EndVertical();
        }

        private void RightWindow()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                m_ExcelScroll = EditorGUILayout.BeginScrollView(m_ExcelScroll);
                {
                    if (m_ExcelData != null)
                    {
                        DataTable sheet = m_ExcelData.GetDataTable(m_SelectionExcelIndex);

                        int rowCount = sheet.Rows.Count;
                        int colCount = sheet.Columns.Count;

                        List<string> grid = new List<string>(rowCount * colCount);

                        for (int i = 0; i < rowCount; i++)
                        {
                            for (int j = 0; j < colCount; j++)
                            {
                                grid.Add(sheet.Rows[i][j].ToString());
                            }
                        }

                        var alignment = EditorStyles.textField.alignment;
                        EditorStyles.textField.alignment = TextAnchor.MiddleCenter;
                        GUILayout.SelectionGrid(0, grid.ToArray(), colCount, EditorStyles.textField);
                        EditorStyles.textField.alignment = alignment;
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        public override void OnSelectionChange()
        {
            Refresh();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            Refresh();
        }

        public override void OnFocus()
        {
            Refresh();
        }

        private void Refresh()
        {
            if (m_Look)
            {
                return;
            }

            SelectionExcel();
            Repaint();
        }

        private void Convert(int index)
        {
            //判断编码类型
            Encoding encoding = null;
            if (m_IndexOfEncoding == 0)
            {
                encoding = Encoding.GetEncoding("utf-8");
            }
            else if (m_IndexOfEncoding == 1)
            {
                encoding = Encoding.GetEncoding("gb2312");
            }

            string path = Path.GetDirectoryName(m_FilePath);

            path = Utility.Path.GetCombinePath(path, m_ExcelData.TableNames[index]);

            switch (m_IndexOfFormatOption)
            {
                case 0:
                    path += ".txt";
                    m_ExcelData.CreateTxt(path, index, encoding);
                    break;
                case 1:
                    path += ".xml";
                    m_ExcelData.CreateXML(path, index, encoding);
                    break;
                case 2:
                    path += ".cs";
                    m_ExcelData.CreateEnum(path, index, encoding);
                    break;
                default:
                    break;
            }

            //刷新本地资源
            AssetDatabase.Refresh();
        }

        private void DrawExcelInfo()
        {
            if (!string.IsNullOrEmpty(m_FilePath))
            {
                GUILayout.BeginVertical("box");
                {
                    if (GUILayout.Button("Convert"))
                    {
                        Convert(m_SelectionExcelIndex);
                    }

                    if (GUILayout.Button("Convert All"))
                    {
                        for (int i = 0; i < m_ExcelData.TableCount; i++)
                        {
                            Convert(i);
                        }
                    }

                    EditorGUILayout.Space();

                    GUILayout.Label(Path.GetFileName(m_FilePath), EditorStyles.boldLabel);

                    if (m_ExcelData != null)
                    {
                        GUILayout.BeginVertical();
                        {
                            m_SelectionExcelIndex =
                                GUILayout.SelectionGrid(m_SelectionExcelIndex, m_ExcelData.TableNames.ToArray(), 1);
                        }
                        GUILayout.EndVertical();
                    }
                }
                GUILayout.EndVertical();
            }
        }

        private void SelectionExcel()
        {
            Object selection = Selection.activeObject;

            if (selection == null)
                return;

            string objPath = AssetDatabase.GetAssetPath(selection);

            m_FilePath = objPath.EndsWith(".xlsx") ? objPath : string.Empty;

            m_ExcelData = string.IsNullOrEmpty(m_FilePath) ? null : new ExcelData(m_FilePath);
        }
    }
}