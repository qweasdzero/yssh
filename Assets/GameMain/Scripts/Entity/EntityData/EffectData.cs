using StarForce;
using UnityEngine;

namespace StarForce
{
    public class EffectData: EntityData
    {
        private Vector3 m_Scale;
        private int m_Father;

        public int Father
        {
            get { return m_Father; }
            set { m_Father = value; }
        }

        public Vector3 Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }

        public EffectData(int entityId, int typeId,int father) : base(entityId, typeId)
        {
            m_Father = father;
        }
    }
}