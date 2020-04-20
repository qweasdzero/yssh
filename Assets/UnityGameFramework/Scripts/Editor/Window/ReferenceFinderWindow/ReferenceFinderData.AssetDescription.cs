using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Editor
{
    public partial class ReferenceFinderData
    {
        public class AssetDescription
        {
            private string m_Name = "";
            private string m_Path = "";
            private Hash128 m_AssetDependencyHash;
            private List<string> m_Dependencies = new List<string>();
            private List<string> m_References = new List<string>();
            private AssetState m_State = AssetState.NORMAL;

            public string Name
            {
                get { return m_Name; }
                set { m_Name = value; }
            }

            public string Path
            {
                get { return m_Path; }
                set { m_Path = value; }
            }

            public Hash128 AssetDependencyHash
            {
                get { return m_AssetDependencyHash; }
                set { m_AssetDependencyHash = value; }
            }

            public List<string> Dependencies
            {
                get { return m_Dependencies; }
                set { m_Dependencies = value; }
            }

            public List<string> References
            {
                get { return m_References; }
                set { m_References = value; }
            }

            public AssetState State
            {
                get { return m_State; }
                set { m_State = value; }
            }
        }
    }
}