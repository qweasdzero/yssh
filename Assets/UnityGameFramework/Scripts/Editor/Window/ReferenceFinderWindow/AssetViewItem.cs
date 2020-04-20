using UnityEditor.IMGUI.Controls;

namespace UnityGameFramework.Editor
{
    /// <summary>
    /// 带数据的TreeViewItem
    /// </summary>
    public class AssetViewItem : TreeViewItem
    {
        private ReferenceFinderData.AssetDescription m_Data;

        public ReferenceFinderData.AssetDescription Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }
    }
}