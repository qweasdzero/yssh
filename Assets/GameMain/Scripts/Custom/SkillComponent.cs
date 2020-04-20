using System;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class SkillComponent : GameFrameworkComponent
    {
        private int m_Power;
        private int m_Skill1Consume;
        private int m_Skill2Consume;
        private int m_Skill3Consume;
        private int m_Skill4Consume;
        private int m_Skill5Consume;


        private float m_Skill1Cd;
        private float m_Skill2Cd;
        private float m_Skill3Cd;
        private float m_Skill4Cd;
        private float m_Skill5Cd;

        private float m_Skill1CdMax;
        private float m_Skill2CdMax;
        private float m_Skill3CdMax;
        private float m_Skill4CdMax;
        private float m_Skill5CdMax;

        public int Power
        {
            get { return m_Power; }
            set
            {
                if (value >= 10)
                {
                    value = 10;
                }

                m_Power = value;
            }
        }

        public int Skill1Consume
        {
            get { return m_Skill1Consume; }
            set { m_Skill1Consume = value; }
        }

        public int Skill2Consume
        {
            get { return m_Skill2Consume; }
            set { m_Skill2Consume = value; }
        }

        public int Skill3Consume
        {
            get { return m_Skill3Consume; }
            set { m_Skill3Consume = value; }
        }

        public int Skill4Consume
        {
            get { return m_Skill4Consume; }
            set { m_Skill4Consume = value; }
        }

        public int Skill5Consume
        {
            get { return m_Skill5Consume; }
            set { m_Skill5Consume = value; }
        }

        public float Skill1Cd
        {
            get { return m_Skill1Cd; }
            set { m_Skill1Cd = value; }
        }

        public float Skill2Cd
        {
            get { return m_Skill2Cd; }
            set { m_Skill2Cd = value; }
        }

        public float Skill3Cd
        {
            get { return m_Skill3Cd; }
            set { m_Skill3Cd = value; }
        }

        public float Skill4Cd
        {
            get { return m_Skill4Cd; }
            set { m_Skill4Cd = value; }
        }

        public float Skill5Cd
        {
            get { return m_Skill5Cd; }
            set { m_Skill5Cd = value; }
        }

        public float Skill1CdMax
        {
            get { return m_Skill1CdMax; }
            set { m_Skill1CdMax = value; }
        }

        public float Skill2CdMax
        {
            get { return m_Skill2CdMax; }
            set { m_Skill2CdMax = value; }
        }

        public float Skill3CdMax
        {
            get { return m_Skill3CdMax; }
            set { m_Skill3CdMax = value; }
        }

        public float Skill4CdMax
        {
            get { return m_Skill4CdMax; }
            set { m_Skill4CdMax = value; }
        }

        public float Skill5CdMax
        {
            get { return m_Skill5CdMax; }
            set { m_Skill5CdMax = value; }
        }
    }
}