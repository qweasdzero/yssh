using System.Collections.Generic;
using UnityEditor;
using System;
using UnityEngine;

namespace UnityGameFramework.Editor
{
    /// <summary>
    /// 游戏框架 EditorWindow 抽象类。
    /// </summary>
    public abstract class GameFrameworkWindow : EditorWindow
    {
        private bool m_IsCompiling = false;

        public virtual void OnGUI()
        {
            if (m_IsCompiling && !EditorApplication.isCompiling)
            {
                m_IsCompiling = false;
                OnCompileComplete();
            }
            else if (!m_IsCompiling && EditorApplication.isCompiling)
            {
                m_IsCompiling = true;
                OnCompileStart();
            }
        }

        /// <summary>
        /// 编译开始事件。
        /// </summary>
        protected virtual void OnCompileStart()
        {
        }

        /// <summary>
        /// 编译完成事件。
        /// </summary>
        protected virtual void OnCompileComplete()
        {
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnFocus()
        {
        }

        public virtual void OnSelectionChange()
        {
        }

        protected bool IsPrefabInHierarchy(UnityEngine.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

#if UNITY_2018_3_OR_NEWER
            return true;
#else
            return PrefabUtility.GetPrefabType(obj) != PrefabType.Prefab;
#endif
        }
    }
}