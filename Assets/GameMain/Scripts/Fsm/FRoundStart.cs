using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using SG1;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class FRoundStart : FsmBase
    {
        public List<int> m_PowerList = new List<int>() {2, 4, 6, 8, 10};

        protected override void OnEnter(IFsm<NormalGame> fsm)
        {
            base.OnEnter(fsm);
            //计算下一个目标

            while (fsm.Owner.First==null)
            {
                if (fsm.Owner.Seat / 5 >= 1)
                {
                    fsm.Owner.Round += 1;
                    if (fsm.Owner.Round >= 5)
                    {
                        GameEntry.Skill.Power += 10;
                    }
                    else
                    {
                        GameEntry.Skill.Power = m_PowerList[fsm.Owner.Round - 1];
                    }

                    GameEntry.Event.Fire(this, ReferencePool.Acquire<NextRoundEventArgs>().Fill());
                }

                fsm.Owner.Seat = (fsm.Owner.Seat % 5) + 1;
                
                if (m_SkillList.Count > 0)
                {
                    SkillQueue();
                }

                if (m_UseSkill.Count > 0)
                {
                    Role role = m_UseSkill.Pop();
                    GameEntry.Event.Fire(this,
                        ReferencePool.Acquire<SkillEventArgs>().Fill(role.GetImpact().Seat, role.GetImpact().Camp));
                    return;
                }

                Atk(fsm);
            }
        }

        private void Atk(IFsm<NormalGame> fsm)
        {
            if (fsm.Owner.First == null)
            {
                Role role1 = GameEntry.Role.MyRole[fsm.Owner.Seat];
                Role role2 = GameEntry.Role.EnemyRole[fsm.Owner.Seat];
                if (role1.GetImpact().Speed >= role2.GetImpact().Speed)
                {
                    fsm.Owner.First = role1;
                    fsm.Owner.Second = role2;
                }
                else
                {
                    fsm.Owner.First = role2;
                    fsm.Owner.Second = role1;
                }

                if (!fsm.Owner.First.GetImpact().Die)
                {
                    int target = GetAtkTarget(fsm.Owner.First.GetImpact());
                    if (target == 0)
                    {
                        IsGameOver(fsm.Owner.First.GetImpact().Camp);
                        return;
                    }
                    else
                    {
                        GameEntry.Event.Fire(this,
                            ReferencePool.Acquire<AtkEventArgs>()
                                .Fill(fsm.Owner.Seat, fsm.Owner.First.GetImpact().Camp, target));
                    }
                }
            }
            else
            {
                if (!fsm.Owner.Second.GetImpact().Die)
                {
                    int target = GetAtkTarget(fsm.Owner.Second.GetImpact());
                    if (target == 0)
                    {
                        if (!IsGameOver(fsm.Owner.Second.GetImpact().Camp))
                        {
                            fsm.Owner.First = null;
                            fsm.Owner.Second = null;
                        }

                        return;
                    }
                    else
                    {
                        GameEntry.Event.Fire(this,
                            ReferencePool.Acquire<AtkEventArgs>()
                                .Fill(fsm.Owner.Seat, fsm.Owner.Second.GetImpact().Camp, target));
                    }
                }

                if (!IsGameOver(fsm.Owner.Second.GetImpact().Camp))
                {
                    fsm.Owner.First = null;
                    fsm.Owner.Second = null;
                }
            }
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

            // m_Start = false;
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


        protected override void OnUpdate(IFsm<NormalGame> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<NormalGame> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }
    }
}