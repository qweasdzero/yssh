using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace UnityGameFramework.Editor
{
    public class ReferenceFinderWindow : EditorWindow
    {
        //依赖模式的key
        private const string c_IsDependPrefKey = "ReferenceFinderData_IsDepend";

        //是否需要更新信息状态的key
        private const string c_NeedUpdateStatePrefKey = "ReferenceFinderData_needUpdateState";

        private static readonly ReferenceFinderData s_Data = new ReferenceFinderData();
        private static bool s_InitializedData = false;

        private bool m_IsDepend = false;
        private bool m_NeedUpdateState = true;

        private bool m_NeedUpdateAssetTree = false;

        private bool m_InitializedGuiStyle = false;

        //工具栏按钮样式
        private GUIStyle m_ToolbarButtonGuiStyle;

        //工具栏样式
        private GUIStyle m_ToolbarGuiStyle;

        //选中资源列表
        private readonly List<string> m_SelectedAssetGuid = new List<string>();

        [SerializeField] private AssetTreeView m_AssetTreeView;

        [SerializeField] private TreeViewState m_TreeViewState;

        /// <summary>
        /// 查找资源引用信息
        /// </summary>
        [MenuItem("Assets/Find References &r")]
        private static void FindRef()
        {
            InitDataIfNeeded();
            OpenWindow();
            ReferenceFinderWindow window = GetWindow<ReferenceFinderWindow>();
            window.UpdateSelectedAssets();
        }

        /// <summary>
        /// 打开窗口
        /// </summary>
        [MenuItem("Window/Refrence Finder", false, 1000)]
        private static void OpenWindow()
        {
            ReferenceFinderWindow window = GetWindow<ReferenceFinderWindow>();
            window.wantsMouseMove = false;
            window.titleContent = new GUIContent("Reference Finder");
            window.Show();
            window.Focus();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private static void InitDataIfNeeded()
        {
            if (!s_InitializedData)
            {
                //初始化数据
                if (!s_Data.ReadFromCache())
                {
                    s_Data.CollectDependenciesInfo();
                }

                s_InitializedData = true;
            }
        }

        /// <summary>
        /// 初始化GUIStyle
        /// </summary>
        private void InitGuiStyleIfNeeded()
        {
            if (!m_InitializedGuiStyle)
            {
                m_ToolbarButtonGuiStyle = new GUIStyle("ToolbarButton");
                m_ToolbarGuiStyle = new GUIStyle("Toolbar");
                m_InitializedGuiStyle = true;
            }
        }

        /// <summary>
        /// 更新选中资源列表
        /// </summary>
        private void UpdateSelectedAssets()
        {
            m_SelectedAssetGuid.Clear();
            foreach (var obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                //如果是文件夹
                if (Directory.Exists(path))
                {
                    string[] folder = new string[] {path};
                    //将文件夹下所有资源作为选择资源
                    string[] guids = AssetDatabase.FindAssets(null, folder);
                    foreach (var guid in guids)
                    {
                        if (!m_SelectedAssetGuid.Contains(guid) &&
                            !Directory.Exists(AssetDatabase.GUIDToAssetPath(guid)))
                        {
                            m_SelectedAssetGuid.Add(guid);
                        }
                    }
                }
                //如果是文件资源
                else
                {
                    string guid = AssetDatabase.AssetPathToGUID(path);
                    m_SelectedAssetGuid.Add(guid);
                }
            }

            m_NeedUpdateAssetTree = true;
        }

        /// <summary>
        /// 通过选中资源列表更新TreeView
        /// </summary>
        private void UpdateAssetTree()
        {
            if (m_NeedUpdateAssetTree && m_SelectedAssetGuid.Count != 0)
            {
                var root = SelectedAssetGuidToRootItem(m_SelectedAssetGuid);
                if (m_AssetTreeView == null)
                {
                    //初始化TreeView
                    if (m_TreeViewState == null)
                        m_TreeViewState = new TreeViewState();
                    var headerState = AssetTreeView.CreateDefaultMultiColumnHeaderState(position.width);
                    var multiColumnHeader = new MultiColumnHeader(headerState);
                    m_AssetTreeView = new AssetTreeView(m_TreeViewState, multiColumnHeader);
                }

                m_AssetTreeView.AssetRoot = root;
                m_AssetTreeView.CollapseAll();
                m_AssetTreeView.Reload();
                m_NeedUpdateAssetTree = false;
            }
        }

        private void OnEnable()
        {
            m_IsDepend = PlayerPrefs.GetInt(c_IsDependPrefKey, 0) == 1;
            m_NeedUpdateState = PlayerPrefs.GetInt(c_NeedUpdateStatePrefKey, 1) == 1;
        }

        private void OnGUI()
        {
            InitGuiStyleIfNeeded();
            DrawOptionBar();
            UpdateAssetTree();
            if (m_AssetTreeView != null)
            {
                //绘制Treeview
                m_AssetTreeView.OnGUI(new Rect(0, m_ToolbarGuiStyle.fixedHeight, position.width,
                    position.height - m_ToolbarGuiStyle.fixedHeight));
            }
        }

        //绘制上条
        public void DrawOptionBar()
        {
            EditorGUILayout.BeginHorizontal(m_ToolbarGuiStyle);
            //刷新数据
            if (GUILayout.Button("Refresh Data", m_ToolbarButtonGuiStyle))
            {
                s_Data.CollectDependenciesInfo();
                m_NeedUpdateAssetTree = true;
                GUIUtility.ExitGUI();
            }

            //修改模式
            bool preIsDepend = m_IsDepend;
            m_IsDepend = GUILayout.Toggle(m_IsDepend, m_IsDepend ? "Model(Depend)" : "Model(Reference)",
                m_ToolbarButtonGuiStyle, GUILayout.Width(100));
            if (preIsDepend != m_IsDepend)
            {
                OnModelSelect();
            }

            //是否需要更新状态
            bool preNeedUpdateState = m_NeedUpdateState;
            m_NeedUpdateState = GUILayout.Toggle(m_NeedUpdateState, "Need Update State", m_ToolbarButtonGuiStyle);
            if (preNeedUpdateState != m_NeedUpdateState)
            {
                PlayerPrefs.SetInt(c_NeedUpdateStatePrefKey, m_NeedUpdateState ? 1 : 0);
            }

            GUILayout.FlexibleSpace();

            //扩展
            if (GUILayout.Button("Expand", m_ToolbarButtonGuiStyle))
            {
                if (m_AssetTreeView != null) m_AssetTreeView.ExpandAll();
            }

            //折叠
            if (GUILayout.Button("Collapse", m_ToolbarButtonGuiStyle))
            {
                if (m_AssetTreeView != null) m_AssetTreeView.CollapseAll();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void OnModelSelect()
        {
            m_NeedUpdateAssetTree = true;
            PlayerPrefs.SetInt(c_IsDependPrefKey, m_IsDepend ? 1 : 0);
        }


        //生成root相关
        private HashSet<string> updatedAssetSet = new HashSet<string>();

        /// <summary>
        /// 通过选择资源列表生成TreeView的根节点
        /// </summary>
        /// <param name="selectedAssetGuid"></param>
        /// <returns></returns>
        private AssetViewItem SelectedAssetGuidToRootItem(List<string> selectedAssetGuid)
        {
            updatedAssetSet.Clear();
            int elementCount = 0;
            var root = new AssetViewItem {id = elementCount, depth = -1, displayName = "Root", Data = null};
            int depth = 0;
            foreach (var childGuid in selectedAssetGuid)
            {
                root.AddChild(CreateTree(childGuid, ref elementCount, depth));
            }

            updatedAssetSet.Clear();
            return root;
        }

        /// <summary>
        /// 通过每个节点的数据生成子节点
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="elementCount"></param>
        /// <param name="_depth"></param>
        /// <returns></returns>
        private AssetViewItem CreateTree(string guid, ref int elementCount, int _depth)
        {
            if (m_NeedUpdateState && !updatedAssetSet.Contains(guid))
            {
                s_Data.UpdateAssetState(guid);
                updatedAssetSet.Add(guid);
            }

            ++elementCount;
            var referenceData = s_Data.AssetDict[guid];
            var root = new AssetViewItem
                {id = elementCount, displayName = referenceData.Name, Data = referenceData, depth = _depth};
            var childGuids = m_IsDepend ? referenceData.Dependencies : referenceData.References;
            foreach (var childGuid in childGuids)
            {
                root.AddChild(CreateTree(childGuid, ref elementCount, _depth + 1));
            }

            return root;
        }
    }
}