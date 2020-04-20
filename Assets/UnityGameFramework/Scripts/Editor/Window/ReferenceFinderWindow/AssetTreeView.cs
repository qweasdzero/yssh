using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace UnityGameFramework.Editor
{
    /// <summary>
    /// 资源引用树
    /// </summary>
    public class AssetTreeView : TreeView
    {
        //列信息
        private enum MyColumns
        {
            Name,
            Path,
            State,
        }

        //图标宽度
        private const float c_KIconWidth = 18f;

        //列表高度
        private const float c_KRowHeights = 20f;

        private AssetViewItem m_AssetRoot;

        private readonly GUIStyle stateGUIStyle = new GUIStyle {richText = true, alignment = TextAnchor.MiddleCenter};

        public AssetViewItem AssetRoot
        {
            get { return m_AssetRoot; }
            set { m_AssetRoot = value; }
        }

        public AssetTreeView(TreeViewState state, MultiColumnHeader multicolumnHeader) : base(state, multicolumnHeader)
        {
            rowHeight = c_KRowHeights;
            columnIndexForTreeFoldouts = 0;
            showAlternatingRowBackgrounds = true;
            showBorder = false;
            customFoldoutYOffset =
                (c_KRowHeights - EditorGUIUtility.singleLineHeight) *
                0.5f; // center foldout in the row since we also center content. See RowGUI
            extraSpaceBeforeIconAndLabel = c_KIconWidth;
        }

        /// <summary>
        /// 响应双击事件
        /// </summary>
        /// <param name="id"></param>
        protected override void DoubleClickedItem(int id)
        {
            var item = (AssetViewItem) FindItem(id, rootItem);
            //在ProjectWindow中高亮双击资源
            if (item != null)
            {
                var assetObject = AssetDatabase.LoadAssetAtPath(item.Data.Path, typeof(UnityEngine.Object));
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = assetObject;
                EditorGUIUtility.PingObject(assetObject);
            }
        }

        /// <summary>
        /// 生成ColumnHeader
        /// </summary>
        /// <param name="treeViewWidth"></param>
        /// <returns></returns>
        public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState(float treeViewWidth)
        {
            var columns = new[]
            {
                //图标+名称
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Name"),
                    headerTextAlignment = TextAlignment.Center,
                    sortedAscending = false,
                    width = 200,
                    minWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = false,
                    canSort = false
                },
                //路径
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Path"),
                    headerTextAlignment = TextAlignment.Center,
                    sortedAscending = false,
                    width = 360,
                    minWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = false,
                    canSort = false
                },
                //状态
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("State"),
                    headerTextAlignment = TextAlignment.Center,
                    sortedAscending = false,
                    width = 60,
                    minWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = true,
                    canSort = false
                },
            };
            var state = new MultiColumnHeaderState(columns);
            return state;
        }

        protected override TreeViewItem BuildRoot()
        {
            return m_AssetRoot;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (AssetViewItem) args.item;
            for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
            {
                CellGui(args.GetCellRect(i), item, (MyColumns) args.GetColumn(i), ref args);
            }
        }

        /// <summary>
        /// 绘制列表中的每项内容
        /// </summary>
        /// <param name="cellRect"></param>
        /// <param name="item"></param>
        /// <param name="column"></param>
        /// <param name="args"></param>
        private void CellGui(Rect cellRect, AssetViewItem item, MyColumns column, ref RowGUIArgs args)
        {
            CenterRectUsingSingleLineHeight(ref cellRect);
            switch (column)
            {
                case MyColumns.Name:
                {
                    var iconRect = cellRect;
                    iconRect.x += GetContentIndent(item);
                    iconRect.width = c_KIconWidth;
                    if (iconRect.x < cellRect.xMax)
                    {
                        var icon = GetIcon(item.Data.Path);
                        if (icon != null)
                            GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
                    }

                    args.rowRect = cellRect;
                    base.RowGUI(args);
                }
                    break;
                case MyColumns.Path:
                {
                    GUI.Label(cellRect, item.Data.Path);
                }
                    break;
                case MyColumns.State:
                {
                    GUI.Label(cellRect, ReferenceFinderData.GetInfoByState(item.Data.State), stateGUIStyle);
                }
                    break;
            }
        }

        /// <summary>
        /// 根据资源信息获取资源图标
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static Texture2D GetIcon(string path)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            if (obj != null)
            {
                Texture2D icon = AssetPreview.GetMiniThumbnail(obj);
                if (icon == null)
                    icon = AssetPreview.GetMiniTypeThumbnail(obj.GetType());
                return icon;
            }

            return null;
        }
    }
}