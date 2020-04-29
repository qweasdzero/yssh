using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using SG1;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class NormalGame : GameBase
    {
//        private float m_ElapseSeconds = 0f;

        public override GameMode GameMode
        {
            get
            {
                return GameMode.Normal;
            }
        }

        private int m_Seat;
        private int m_Round;
        private bool m_Start;
        private bool m_Wait;

        public int Seat
        {
            get { return m_Seat; }
            set { m_Seat = value; }
        }

        public int Round
        {
            get { return m_Round; }
            set { m_Round = value; }
        }

        //攻击角色
        private Role m_First;
        private Role m_Second;

        public Role First
        {
            get { return m_First; }
            set { m_First = value; }
        }

        public Role Second
        {
            get { return m_Second; }
            set { m_Second = value; }
        }

        private IFsm<NormalGame> m_PlayerFsm;

        public override void Initialize()
        {
            base.Initialize();
            m_SkillList = new List<Role>();
            m_UseSkill = new Stack<Role>();
            FsmBase[] state = new FsmBase[]
            {
                new FSeat(), new FStart(), new FRoundEnd(), new FRoundStart(),
                new FSeatStart(), new FEnd(),
            };

            m_PlayerFsm = GameEntry.Fsm.CreateFsm("Game", this, state);
            m_PlayerFsm.Start<FStart>();

            GameEntry.Event.Subscribe(ActiveSkillEventArgs.EventId, OnActiveSkill);
            GameEntry.Event.Subscribe(AtkEndEventArgs.EventId, OnAtkEnd);
        }

        public override void Shutdown()
        {
            base.Shutdown();
            GameEntry.Event.Unsubscribe(ActiveSkillEventArgs.EventId, OnActiveSkill);
            GameEntry.Event.Unsubscribe(AtkEndEventArgs.EventId, OnAtkEnd);
        }

        private void OnAtkEnd(object sender, GameEventArgs e)
        {
            AtkEndEventArgs ne = e as AtkEndEventArgs;
            if (ne == null)
            {
                return;
            }

            Skill skill = GameEntry.Skill.Dic[ne.SkillId];
            GetSkillTarget(skill, GameEntry.Role.GetTarget(ne.CampType)[ne.Seat].GetImpact());
        }

        private void OnActiveSkill(object sender, GameEventArgs e)
        {
            ActiveSkillEventArgs ne = e as ActiveSkillEventArgs;
            if (ne == null)
            {
                return;
            }

            if (ne.CampType == CampType.Player)
            {
                m_SkillList.Add(GameEntry.Role.MyRole[ne.Seat]);
            }

            if (ne.CampType == CampType.Enemy)
            {
                m_SkillList.Add(GameEntry.Role.EnemyRole[ne.Seat]);
            }
        }

        private List<Role> m_SkillList;
        private Stack<Role> m_UseSkill;
        private Stack<Role> m_ExtraSkill;
        private LinkedList<Role> m_SlowAtk;
        
        public List<Role> SkillList
        {
            get { return m_SkillList; }
            set { m_SkillList = value; }
        }

        public Stack<Role> UseSkill
        {
            get { return m_UseSkill; }
            set { m_UseSkill = value; }
        }

        public Stack<Role> ExtraSkill
        {
            get { return m_ExtraSkill; }
            set { m_ExtraSkill = value; }
        }

        public LinkedList<Role> SlowAtk
        {
            get { return m_SlowAtk; }
            set { m_SlowAtk = value; }
        }

        private void GetSkillTarget(Skill skill, RoleImpactData role)
        {
            switch (skill.SkillType)
            {
                case SkillType.Positive:
                    GetAtkTarget(role, skill);
                    break;
                case SkillType.Front:
                    if (!GetFront(role, skill))
                    {
                        GetBack(role, skill);
                    }

                    break;
                case SkillType.Back:
                    if (!GetBack(role, skill))
                    {
                        GetFront(role, skill);
                    }

                    break;
                case SkillType.Chain:
                    GetChain(role, skill);
                    break;
                case SkillType.All:
                    GetAll(role, skill);
                    break;
            }
        }

        /// <summary>
        /// 获取攻击目标
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        private void GetAtkTarget(RoleImpactData role, Skill skill)
        {
            int target = 0;
            if (role.Camp == CampType.Player)
            {
                if (GameEntry.Role.MyAtkDic.TryGetValue(role.Seat, out Stack<int> stack))
                {
                    while (target == 0)
                    {
                        if (stack.Count <= 0)
                        {
                            return;
                        }

                        var peek = stack.Peek();
                        if (GameEntry.Role.EnemyRole[peek].GetImpact().Die)
                        {
                            stack.Pop();
                        }
                        else
                        {
                            target = peek;
                        }
                    }

                    GameEntry.Event.Fire(this,
                        ReferencePool.Acquire<HurtEventArgs>().Fill(new List<int>(1) {target},
                            GetCamp(role.Camp, skill.TargetType), (int) skill.Magnification * role.Attack));
                    if (skill.Buff != Buff.Default)
                    {
                        GameEntry.Event.Fire(this,
                            ReferencePool.Acquire<ExertBuffEventArgs>().Fill(new List<int>(1) {target},
                                GetCamp(role.Camp, skill.TargetType), skill.Buff, skill.BuffTime,
                                (int) skill.BuffValue * role.Attack));
                    }
                }
            }

            if (role.Camp == CampType.Enemy)
            {
                if (GameEntry.Role.EnemyAtkDic.TryGetValue(role.Seat, out Stack<int> stack))
                {
                    while (target == 0)
                    {
                        if (stack.Count <= 0)
                        {
                            return;
                        }

                        var peek = stack.Peek();
                        if (GameEntry.Role.MyRole[peek].GetImpact().Die)
                        {
                            stack.Pop();
                        }
                        else
                        {
                            target = peek;
                        }
                    }

                    GameEntry.Event.Fire(this,
                        ReferencePool.Acquire<HurtEventArgs>().Fill(new List<int>(1) {target},
                            GetCamp(role.Camp, skill.TargetType), (int) skill.Magnification * role.Attack));
                    if (skill.Buff != Buff.Default)
                    {
                        GameEntry.Event.Fire(this,
                            ReferencePool.Acquire<ExertBuffEventArgs>().Fill(new List<int>(1) {target},
                                GetCamp(role.Camp, skill.TargetType), skill.Buff, skill.BuffTime,
                                (int) skill.BuffValue * role.Attack));
                    }
                }
            }
        }

        private bool GetFront(RoleImpactData role, Skill skill)
        {
            if (GameEntry.Role.GetTarget(GetCamp(role.Camp, skill.TargetType))[1].GetImpact().Die &&
                GameEntry.Role.GetTarget(GetCamp(role.Camp, skill.TargetType))[2].GetImpact().Die)
            {
                return false;
            }
            else
            {
                GameEntry.Event.Fire(this,
                    ReferencePool.Acquire<HurtEventArgs>().Fill(new List<int>() {1, 2},
                        GetCamp(role.Camp, skill.TargetType), (int) skill.Magnification * role.Attack));
                if (skill.Buff != Buff.Default)
                {
                    GameEntry.Event.Fire(this,
                        ReferencePool.Acquire<ExertBuffEventArgs>().Fill(new List<int>() {1, 2},
                            GetCamp(role.Camp, skill.TargetType), skill.Buff, skill.BuffTime,
                            (int) skill.BuffValue * role.Attack));
                }

                return true;
            }
        }

        private CampType GetCamp(CampType campType, TargetType targetType)
        {
            if (targetType == TargetType.Teammate)
            {
                return campType;
            }

            if (targetType == TargetType.Enemy)
            {
                if (campType == CampType.Enemy)
                {
                    return CampType.Player;
                }

                if (campType == CampType.Player)
                {
                    return CampType.Enemy;
                }
            }

            return CampType.Unknown;
        }

        private bool GetBack(RoleImpactData role, Skill skill)
        {
            if (GameEntry.Role.GetTarget(GetCamp(role.Camp, skill.TargetType))[3].GetImpact().Die &&
                GameEntry.Role.GetTarget(GetCamp(role.Camp, skill.TargetType))[4].GetImpact().Die &&
                GameEntry.Role.GetTarget(GetCamp(role.Camp, skill.TargetType))[5].GetImpact().Die)
            {
                return false;
            }
            else
            {
                GameEntry.Event.Fire(this,
                    ReferencePool.Acquire<HurtEventArgs>().Fill(new List<int>() {3, 4, 5},
                        GetCamp(role.Camp, skill.TargetType), (int) skill.Magnification * role.Attack));
                if (skill.Buff != Buff.Default)
                {
                    GameEntry.Event.Fire(this,
                        ReferencePool.Acquire<ExertBuffEventArgs>().Fill(new List<int>() {3, 4, 5},
                            GetCamp(role.Camp, skill.TargetType), skill.Buff, skill.BuffTime,
                            (int) skill.BuffValue * role.Attack));
                }

                return true;
            }
        }

        private void GetChain(RoleImpactData role, Skill skill)
        {
            List<int> list = new List<int>();
            List<int> target = new List<int>();
            int i = 3;
            foreach (KeyValuePair<int, Role> keyValuePair in GameEntry.Role.GetTarget(GetCamp(role.Camp,
                skill.TargetType)))
            {
                target.Add(keyValuePair.Key);
            }

            while (i > 0)
            {
                if (target.Count <= 0)
                {
                    break;
                }

                int j = GameFramework.Utility.Random.GetRandom(0, target.Count);
                if (!GameEntry.Role.GetTarget(GetCamp(role.Camp, skill.TargetType))[target[j]].GetImpact().Die &&
                    !list.Contains(j))
                {
                    list.Add(target[j]);
                    i--;
                }

                target.Remove(target[j]);
            }

            GameEntry.Event.Fire(this,
                ReferencePool.Acquire<HurtEventArgs>()
                    .Fill(list, GetCamp(role.Camp, skill.TargetType), (int) skill.Magnification * role.Attack));
            if (skill.Buff != Buff.Default)
            {
                GameEntry.Event.Fire(this,
                    ReferencePool.Acquire<ExertBuffEventArgs>().Fill(list, GetCamp(role.Camp, skill.TargetType),
                        skill.Buff, skill.BuffTime, (int) skill.BuffValue * role.Attack));
            }
        }

        private void GetAll(RoleImpactData role, Skill skill)
        {
            GameEntry.Event.Fire(this,
                ReferencePool.Acquire<HurtEventArgs>().Fill(new List<int>() {1, 2, 3, 4, 5},
                    GetCamp(role.Camp, skill.TargetType), (int) skill.Magnification * role.Attack));
            if (skill.Buff != Buff.Default)
            {
                GameEntry.Event.Fire(this,
                    ReferencePool.Acquire<ExertBuffEventArgs>().Fill(new List<int>() {1, 2, 3, 4, 5},
                        GetCamp(role.Camp, skill.TargetType),
                        skill.Buff, skill.BuffTime, (int) skill.BuffValue * role.Attack));
            }
        }
    }

    public enum SkillType
    {
        Positive, //正面

        Front, //前排

        Back, //后排

        Chain, //链状

        All, //全体
    }

    public enum Buff
    {
        Default,

        /// <summary>
        /// 中毒
        /// </summary>
        Poisoning,

        /// <summary>
        /// 眩晕
        /// </summary>
        Vertigo,

        /// <summary>
        /// 减速
        /// </summary>
        SlowDown,
    }

    public class BuffState
    {
        private Buff m_Buff;

        private int m_Round;

        private int m_Value;

        public Buff Buff
        {
            get { return m_Buff; }
            set { m_Buff = value; }
        }

        public int Round
        {
            get { return m_Round; }
            set { m_Round = value; }
        }

        public int Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public BuffState(Buff buff, int round, int value)
        {
            m_Buff = buff;
            m_Round = round;
            m_Value = value;
        }
    }

    public struct Skill
    {
        public int Id;

        public SkillType SkillType;

        public TargetType TargetType;

        public double Magnification; //技能倍率

        public Buff Buff;

        public int BuffTime;

        public double BuffValue;

        public Skill(int id, SkillType skillType, TargetType targetType, double magnification, Buff buff, int buffTime,
            double buffValue)
        {
            Id = id;
            SkillType = skillType;
            TargetType = targetType;
            Magnification = magnification;
            Buff = buff;
            BuffTime = buffTime;
            BuffValue = buffValue;
        }
    }

    public enum TargetType
    {
        Unknown = 0,

        /// <summary>
        /// 己方阵营。
        /// </summary>
        Teammate,

        /// <summary>
        /// 敌人阵营。
        /// </summary>
        Enemy,
    }
}