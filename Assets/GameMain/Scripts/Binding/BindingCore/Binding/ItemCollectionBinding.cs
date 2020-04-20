using UnityEngine;

#pragma warning disable 0649
namespace StarForce
{
    public abstract class ItemCollectionBinding : CollectionBinding
    {
        [InspectorReadOnly(InspectorDiplayMode.EnabledInPlayMode)] [SerializeField]
        private GameObject m_Template;

        public GameObject Template
        {
            get { return m_Template; }
        }
    }
}