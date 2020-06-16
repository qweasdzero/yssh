using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.DataTable;
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

        private Dictionary<int, Skill> m_Dic;

        public Dictionary<int, Skill> Dic
        {
            get { return m_Dic; }
        }

        protected override void Awake()
        {
            base.Awake();
            IDataTable<DREntity> drEntities;
            m_Dic = new Dictionary<int, Skill>
            {
                {0, new Skill(0, SkillType.Positive, TargetType.Enemy, 1, Buff.Default, 0, 0)}, //普通攻击
                {1, new Skill(1, SkillType.Positive, TargetType.Enemy, 2, Buff.Vertigo, 1, 0)}, //单体眩晕
                {2, new Skill(2, SkillType.All, TargetType.Teammate, -1, Buff.Default, 0, 0)}, //群体加血
                {3, new Skill(3, SkillType.Back, TargetType.Enemy, 1.3, Buff.Poisoning, 2, 0.2)}, //后排中毒
                {4, new Skill(4, SkillType.Chain, TargetType.Enemy, 1.5, Buff.Default, 0, 0)}, //链式伤害
                {5, new Skill(5, SkillType.Front, TargetType.Enemy, 1.8, Buff.Default, 1, 0)}, //前排伤害
                {6, new Skill(6, SkillType.Positive, TargetType.Enemy, 3, Buff.Vertigo, 1, 0)}, //单体眩晕
                {7, new Skill(7, SkillType.All, TargetType.Teammate, -2, Buff.Default, 0, 0)}, //群体加血
                {8, new Skill(8, SkillType.Back, TargetType.Enemy, 2.3, Buff.Poisoning, 2, 0.2)}, //后排中毒
                {9, new Skill(9, SkillType.Chain, TargetType.Enemy, 2.5, Buff.Default, 0, 0)}, //链式伤害
                {10, new Skill(10, SkillType.Front, TargetType.Enemy, 2.8, Buff.Default, 1, 0)}, //前排伤害
                {14, new Skill(14, SkillType.Self, 3, true, 15, 0, 16)}, //变身
                {15, new Skill(15, SkillType.Chain, TargetType.Enemy, 1.4, Buff.Default, 1, 0)}, //强化普攻
                {16, new Skill(16, SkillType.Chain, TargetType.Enemy, 2.8, Buff.Default, 1, 0)}, //强化技能
            };
        }
    }
}