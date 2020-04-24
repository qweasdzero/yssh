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
    public class FSeat : FsmBase
    {
        public IFsm<NormalGame> m_Fsm;

        protected override void OnEnter(IFsm<NormalGame> fsm)
        {
            base.OnEnter(fsm);

            m_Fsm = fsm;

            GetAttacker();
            GameEntry.Event.Subscribe(AtkEndEventArgs.EventId, OnAtkEnd);
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

        /// <summary>
        /// 主动技能释放排序
        /// </summary>
        private void SkillQueue()
        {
            if (m_Fsm.Owner.UseSkill.Count > 0)
            {
                foreach (Role role in m_Fsm.Owner.UseSkill)
                {
                    m_Fsm.Owner.SkillList.Add(role);
                }

                m_Fsm.Owner.UseSkill.Clear();
            }

            for (int i = 0; i < m_Fsm.Owner.SkillList.Count; i++) //最多做R.Length-1趟排序 
            {
                var exchange = false;
                for (int j = m_Fsm.Owner.SkillList.Count - 2; j >= i; j--)
                {
                    if (m_Fsm.Owner.SkillList[j + 1].GetImpact().Seat > m_Fsm.Owner.SkillList[j].GetImpact().Seat ||
                        (m_Fsm.Owner.SkillList[j + 1].GetImpact().Seat == m_Fsm.Owner.SkillList[j].GetImpact().Seat &&
                         (m_Fsm.Owner.SkillList[j + 1].GetImpact().Speed <=
                          m_Fsm.Owner.SkillList[j].GetImpact().Speed))) //交换条件
                    {
                        var temp = m_Fsm.Owner.SkillList[j + 1];
                        m_Fsm.Owner.SkillList[j + 1] = m_Fsm.Owner.SkillList[j];
                        m_Fsm.Owner.SkillList[j] = temp;
                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }
            }

            for (int i = 0; i < m_Fsm.Owner.SkillList.Count; i++)
            {
                m_Fsm.Owner.UseSkill.Push(m_Fsm.Owner.SkillList[i]);
            }

            m_Fsm.Owner.SkillList.Clear();
        }

        private void GetAttacker()
        {
            if (m_Fsm.Owner.SkillList.Count > 0)
            {
                SkillQueue();
            }

            if (m_Fsm.Owner.UseSkill.Count > 0)
            {
                Role role = m_Fsm.Owner.UseSkill.Pop();
                GameEntry.Event.Fire(this,
                    ReferencePool.Acquire<SkillEventArgs>().Fill(role.GetImpact().Seat, role.GetImpact().Camp));
                return;
            }

            if (m_Fsm.Owner.First != null)
            {
                if (!m_Fsm.Owner.First.GetImpact().Die) //判断是否可以攻击
                {
                    int target = GetAtkTarget(m_Fsm.Owner.First.GetImpact());

                    GameEntry.Event.Fire(this,
                        ReferencePool.Acquire<AtkEventArgs>()
                            .Fill(m_Fsm.Owner.Seat, m_Fsm.Owner.First.GetImpact().Camp, target));
                    m_Fsm.Owner.First = null;
                }
            }
            else if (m_Fsm.Owner.Second != null)
            {
                if (!m_Fsm.Owner.First.GetImpact().Die) //判断是否可以攻击
                {
                    int target = GetAtkTarget(m_Fsm.Owner.First.GetImpact());

                    GameEntry.Event.Fire(this,
                        ReferencePool.Acquire<AtkEventArgs>()
                            .Fill(m_Fsm.Owner.Seat, m_Fsm.Owner.First.GetImpact().Camp, target));
                    m_Fsm.Owner.Second = null;
                }
            }
            else
            {
                ChangeState<FSeatStart>(m_Fsm);
            }
        }

        protected override void OnUpdate(IFsm<NormalGame> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<NormalGame> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            GameEntry.Event.Unsubscribe(AtkEndEventArgs.EventId, OnAtkEnd);
        }

        private void OnAtkEnd(object sender, GameEventArgs e)
        {
            AtkEndEventArgs ne = e as AtkEndEventArgs;
            if (ne == null)
            {
                return;
            }

            GetAttacker();
        }
    }
}