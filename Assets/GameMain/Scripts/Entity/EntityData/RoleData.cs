using System.Collections.Generic;

namespace StarForce
{
    public class RoleData: EntityData
    {
        private CampType m_Camp;
        private int m_Seat;

        private int m_Hp;
        private int m_HpMax;
        private int m_Power;
        private int m_Speed;
        private int m_Atk;

        private bool m_Die;
        
        public CampType Camp
        {
            get { return m_Camp; }
            set { m_Camp = value; }
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

        public int Speed
        {
            get { return m_Speed; }
            set { m_Speed = value; }
        }

        public int Atk
        {
            get { return m_Atk; }
            set { m_Atk = value; }
        }

        public bool Die
        {
            get { return m_Die; }
            set { m_Die = value; }
        }

        private int m_SkillId;

        private int m_PowerId;

        public int SkillId
        {
            get { return m_SkillId; }
            set { m_SkillId = value; }
        }

        public int PowerId
        {
            get { return m_PowerId; }
            set { m_PowerId = value; }
        }

        private Dictionary<Buff,BuffState> m_BuffState;

        public Dictionary<Buff,BuffState> BuffState
        {
            get { return m_BuffState; }
            set { m_BuffState = value; }
        }

        public RoleData(int entityId, int typeId) : base(entityId, typeId)
        {
        }
    }
}