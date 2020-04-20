using SG1;

namespace StarForce
{
    public class HpBarData: EntityData
    {
        private int m_Seat;

        private int m_Hp;
        private int m_HpMax;
        private int m_Power;
        public const int PowerMax = 100;

        private Role m_Father;

        public Role Father
        {
            get { return m_Father; }
            set { m_Father = value; }
        }

        public int Seat
        {
            get { return m_Seat; }
            set { m_Seat = value; }
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