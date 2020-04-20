//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public sealed partial class EntityComponent : GameFrameworkComponent
    {
        [Serializable]
        private sealed class EntityGroup
        {
            [SerializeField]
            private string m_Name = null;

            [SerializeField]
            private float m_InstanceAutoReleaseInterval = 60f;

            [SerializeField]
            private int m_InstanceCapacity = 16;

            [SerializeField]
            private float m_InstanceExpireTime = 60f;

            [SerializeField]
            private int m_InstancePriority = 0;

            [SerializeField] 
            private Vector3 m_Position = Vector3.zero;

            [SerializeField]
            private GameObject m_InstanceRoot = null;

            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            public float InstanceAutoReleaseInterval
            {
                get
                {
                    return m_InstanceAutoReleaseInterval;
                }
            }

            public int InstanceCapacity
            {
                get
                {
                    return m_InstanceCapacity;
                }
            }

            public float InstanceExpireTime
            {
                get
                {
                    return m_InstanceExpireTime;
                }
            }

            public int InstancePriority
            {
                get
                {
                    return m_InstancePriority;
                }
            }

            public Vector3 Position
            {
                get
                {
                    return m_Position;
                }
            }

            public GameObject InstanceRoot
            {
                get
                {
                    return m_InstanceRoot;
                }
            }
        }
    }
}
