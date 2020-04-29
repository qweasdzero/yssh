using StarForce;
using UnityEngine;

namespace SG1
{
    public struct RoleImpactData
    {
        private RoleData m_Data;

        public RoleImpactData(RoleData roleData)
        {
            m_Data = roleData;
        }

        public CampType Camp
        {
            get
            {
                return m_Data.Camp;
            }
        }

        public CampType Enemy
        {
            get
            {
                if (m_Data.Camp == CampType.Player)
                {
                    return CampType.Enemy;
                }
                else
                {
                    return CampType.Player;
                }
            }
        }

        public int Hp
        {
            get
            {
                return m_Data.Hp;
            }
        }

        public int HpMax
        {
            get
            {
                return m_Data.HpMax;
            }
        }

        public int Power
        {
            get
            {
                return m_Data.Power;
            }
        }

        public int Attack
        {
            get
            {
                return m_Data.Atk;
            }
        }

        public int Speed
        {
            get { return m_Data.Speed; }
        }

        public Vector3 Position
        {
            get { return m_Data.Position; }
        }

        public bool Die
        {
            get { return m_Data.Die; }
        }

        public int Seat
        {
            get { return m_Data.Seat; }
        } 
        
        public int SkillId
        {
            get { return m_Data.SkillId; }
        }
    }
}