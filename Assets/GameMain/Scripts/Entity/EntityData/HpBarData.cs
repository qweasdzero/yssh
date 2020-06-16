using StarForce;

namespace StarForce
{
    public class HpBarData: EntityData
    {
        private int m_Hp;
        private int m_HpMax;
        private int m_Power;

        private Role m_Father;

        public Role Father
        {
            get { return m_Father; }
            set { m_Father = value; }
        }

        public int Hp
        {
            get { return m_Hp; }
            set { m_Hp = value; }
        }

        public int HpMax
        {
            get { return m_HpMax; }
            set { m_HpMax = value; }
        }

        public int Power
        {
            get { return m_Power; }
            set { m_Power = value; }
        }

     

        public HpBarData(int entityId, int typeId,Role father) : base(entityId, typeId)
        {
            m_Father = father;
        }
    }
}