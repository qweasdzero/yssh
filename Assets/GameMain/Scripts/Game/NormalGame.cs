using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
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

        public List<int> m_PowerList = new List<int>() {2, 4, 6, 8, 10};

        //攻击角色
        private Role m_First;
        private Role m_Second;

        public override void Initialize()
        {
            base.Initialize();
            GameEntry.Role.MyRole = new Dictionary<int, Role>();
            GameEntry.Role.EnemyRole = new Dictionary<int, Role>();
            GameEntry.Role.MyAtkDic = new Dictionary<int, Stack<int>>();
            GameEntry.Role.EnemyAtkDic = new Dictionary<int, Stack<int>>();
            m_SkillList = new List<Role>();
            m_UseSkill = new Stack<Role>();

            for (int i = 0; i < 5; i++)
            {
                GameEntry.Entity.ShowRole(new RoleData(GameEntry.Entity.GenerateSerialId(), 1000)
                {
                    Camp = CampType.Player,
                    Seat = i + 1,
                    Hp = 200,
                    HpMax = 200,
                    Speed = 100 + i,
                    SkillType=i,
                });
            }

            for (int i = 0; i < 5; i++)
            {
                GameEntry.Entity.ShowRole(new RoleData(GameEntry.Entity.GenerateSerialId(), 1000)
                {
                    Camp = CampType.Enemy,
                    Seat = i + 1,
                    Hp = 200,
                    HpMax = 200,
                    Speed = 100 + 2 * i,
                    SkillType=i,
                });
            }

            GameEntry.Event.Subscribe(AtkEndEventArgs.EventId, OnAtkEnd);
            GameEntry.Event.Subscribe(ActiveSkillEventArgs.EventId, OnActiveSkill);
            GameEntry.Event.Subscribe(StartBattleEventArgs.EventId, OnStartBattle);
        }

        private void Start()
        {
            m_Seat = 1;
            m_Round = 1;
            m_Start = true;

            GameEntry.Skill.Power = m_PowerList[m_Round - 1];
            GameEntry.Event.Fire(this, ReferencePool.Acquire<NextRoundEventArgs>().Fill());
        }

        public override void Shutdown()
        {
            base.Shutdown();
            GameEntry.Event.Unsubscribe(AtkEndEventArgs.EventId, OnAtkEnd);
            GameEntry.Event.Unsubscribe(ActiveSkillEventArgs.EventId, OnActiveSkill);
            GameEntry.Event.Unsubscribe(StartBattleEventArgs.EventId, OnStartBattle);
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

        private void OnAtkEnd(object sender, GameEventArgs e)
        {
            AtkEndEventArgs ne = e as AtkEndEventArgs;
            if (ne == null)
            {
                return;
            }

            m_Wait = false;
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
            if (m_Start)
            {
                if (!m_Wait)
                {
                    if (m_SkillList.Count > 0)
                    {
                        SkillQueue();
                    }

                    if (m_UseSkill.Count > 0)
                    {
                        Role role = m_UseSkill.Pop();
                        m_Wait = true;
                        GameEntry.Event.Fire(this,
                            ReferencePool.Acquire<SkillEventArgs>().Fill(role.GetImpact().Seat, role.GetImpact().Camp));
                        return;
                    }

                    Atk();
                }
            }
        }

        /// <summary>
        /// 寻找攻击者
        /// </summary>
        private void Atk()
        {
            if (m_First == null)
            {
                Next();
                Role role1 = GameEntry.Role.MyRole[m_Seat];
                Role role2 = GameEntry.Role.EnemyRole[m_Seat];
                if (role1.GetImpact().Speed >= role2.GetImpact().Speed)
                {
                    m_First = role1;
                    m_Second = role2;
                }
                else
                {
                    m_First = role2;
                    m_Second = role1;
                }

                if (!m_First.GetImpact().Die)
                {
                    int target = GetAtkTarget(m_First.GetImpact());
                    if (target == 0)
                    {
                        IsGameOver(m_First.GetImpact().Camp);
                        return;
                    }
                    else
                    {
                        GameEntry.Event.Fire(this,
                            ReferencePool.Acquire<AtkEventArgs>()
                                .Fill(m_Seat, m_First.GetImpact().Camp, target));
                        m_Wait = true;
                    }
                }
            }
            else
            {
                if (!m_Second.GetImpact().Die)
                {
                    int target = GetAtkTarget(m_Second.GetImpact());
                    if (target == 0)
                    {
                        if (!IsGameOver(m_Second.GetImpact().Camp))
                        {
                            m_First = null;
                            m_Second = null;
                        }

                        return;
                    }
                    else
                    {
                        GameEntry.Event.Fire(this,
                            ReferencePool.Acquire<AtkEventArgs>()
                                .Fill(m_Seat, m_Second.GetImpact().Camp, target));
                        m_Wait = true;
                    }
                }

                if (!IsGameOver(m_Second.GetImpact().Camp))
                {
                    m_First = null;
                    m_Second = null;
                }
            }
        }

        /// <summary>
        /// 下一个位置
        /// </summary>
        private void Next()
        {
            if (m_Seat / 5 >= 1)
            {
                m_Round += 1;
                GameEntry.Event.Fire(this, ReferencePool.Acquire<NextRoundEventArgs>().Fill());
                if (m_Round >= 5)
                {
                    GameEntry.Skill.Power += 10;
                }
                else
                {
                    GameEntry.Skill.Power += m_PowerList[m_Round - 1];
                }
            }

            m_Seat = (m_Seat % 5) + 1;
        }

        /// <summary>
        /// 判断战斗结束
        /// </summary>
        private bool IsGameOver(CampType campType)
        {
            switch (campType)
            {
                case CampType.Player:
                    foreach (Role role in GameEntry.Role.EnemyRole.Values)
                    {
                        if (!role.GetImpact().Die)
                        {
                            return false;
                        }
                    }

                    break;
                case CampType.Enemy:
                    foreach (Role role in GameEntry.Role.MyRole.Values)
                    {
                        if (!role.GetImpact().Die)
                        {
                            return false;
                        }
                    }

                    break;
            }

            m_Start = false;
            GameEntry.Event.Fire(this, ReferencePool.Acquire<GameOverEventArgs>().Fill());
            return true;
        }

        private List<Role> m_SkillList;
        private Stack<Role> m_UseSkill;

        /// <summary>
        /// 主动技能释放排序
        /// </summary>
        private void SkillQueue()
        {
            if (m_UseSkill.Count > 0)
            {
                foreach (Role role in m_UseSkill)
                {
                    m_SkillList.Add(role);
                }

                m_UseSkill.Clear();
            }

            for (int i = 0; i < m_SkillList.Count; i++) //最多做R.Length-1趟排序 
            {
                var exchange = false;
                for (int j = m_SkillList.Count - 2; j >= i; j--)
                {
                    if (m_SkillList[j + 1].GetImpact().Seat > m_SkillList[j].GetImpact().Seat ||
                        (m_SkillList[j + 1].GetImpact().Seat == m_SkillList[j].GetImpact().Seat &&
                         (m_SkillList[j + 1].GetImpact().Speed <= m_SkillList[j].GetImpact().Speed))) //交换条件
                    {
                        var temp = m_SkillList[j + 1];
                        m_SkillList[j + 1] = m_SkillList[j];
                        m_SkillList[j] = temp;
                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }
            }

            for (int i = 0; i < m_SkillList.Count; i++)
            {
                m_UseSkill.Push(m_SkillList[i]);
            }

            m_SkillList.Clear();
        }

        /// <summary>
        /// 获取攻击目标
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        private int GetAtkTarget(RoleImpactData role)
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
                            return 0;
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
                            return 0;
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
                }
            }


            return target;
        }

        private void OnStartBattle(object sender, GameEventArgs e)
        {
            StartBattleEventArgs ne = e as StartBattleEventArgs;
            if (ne == null)
            {
                return;
            }

            Start();
        }
    }
}