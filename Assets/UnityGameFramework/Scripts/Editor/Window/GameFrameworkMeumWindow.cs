using UnityEditor;

namespace UnityGameFramework.Editor
{
    public abstract class GameFrameworkMeumWindow : GameFrameworkWindow
    {
        private SerializedObject m_SerializedObject;

        public SerializedObject SerializedObject
        {
            get { return m_SerializedObject; }
        }

        /// <summary>
        /// 获取窗口标题。
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// GUI逻辑处理函数。
        /// </summary>
        /// <param name="unusedWindowID">窗口Id。</param>
        public abstract void Window(int unusedWindowID);


        public override void OnEnable()
        {
            base.OnEnable();

            m_SerializedObject = new SerializedObject(this);
        }
    }
}