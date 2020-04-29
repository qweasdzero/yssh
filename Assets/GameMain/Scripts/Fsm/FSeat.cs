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
            GameEntry.Event.Subscribe(NextRoleEventArgs.EventId, OnAtkEnd);
            GameEntry.Event.Subscribe(ExertBuffEventArgs.EventId, OnAtkEnd);
            m_Fsm = fsm;

            GetAttacker();
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
            if (IsGameOver())
            {
                return;
            }

            if (m_Fsm.Owner.SkillList.Count > 0)
            {
                SkillQueue();
            }

            if (m_Fsm.Owner.UseSkill.Count > 0)
            {
                Role role = m_Fsm.Owner.UseSkill.Pop();
                if (role.GetImpact().Die)
                {
                    GetAttacker();
                    return;
                }

                GameEntry.Event.Fire(this,
                    ReferencePool.Acquire<SkillEventArgs>().Fill(role.GetImpact().Seat, role.GetImpact().Camp));
                return;
            }

            if (m_Fsm.Owner.First != null)
            {
                if (!m_Fsm.Owner.First.GetImpact().Die) //判断是否可以攻击
                {
                    if (m_Fsm.Owner.First.GetImpact().BuffState.ContainsKey(Buff.Vertigo))
                    {
                        Log.Info("Vertigo");
                        m_Fsm.Owner.First.OnActionBuff();
                        m_Fsm.Owner.First = null;
                        GetAttacker();
                    }
                    // else if (m_Fsm.Owner.First.GetImpact().BuffState.ContainsKey(Buff.SlowDown))
                    // {
                    //     m_Fsm.Owner.SlowAtk.AddFirst(m_Fsm.Owner.First);
                    //     m_Fsm.Owner.First.OnActionBuff();
                    //     GetAttacker();
                    // }
                    else
                    {
                        GameEntry.Event.Fire(this,
                            ReferencePool.Acquire<AtkEventArgs>()
                                .Fill(m_Fsm.Owner.Seat, m_Fsm.Owner.First.GetImpact().Camp));
                        m_Fsm.Owner.First = null;
                    }

                    return;
                }
            }

            if (m_Fsm.Owner.Second != null)
            {
                if (!m_Fsm.Owner.Second.GetImpact().Die) //判断是否可以攻击
                {
                    if (m_Fsm.Owner.Second.GetImpact().BuffState.ContainsKey(Buff.Vertigo))
                    {
                        Log.Info("Vertigo");
                        m_Fsm.Owner.Second.OnActionBuff();
                        m_Fsm.Owner.Second = null;
                        GetAttacker();
                    }
                    // else if (m_Fsm.Owner.Second.GetImpact().BuffState.ContainsKey(Buff.SlowDown))
                    // {
                    //     m_Fsm.Owner.SlowAtk.AddFirst(m_Fsm.Owner.Second);
                    //     m_Fsm.Owner.Second.OnActionBuff();
                    //     GetAttacker();
                    // }
                    else
                    {
                        GameEntry.Event.Fire(this,
                            ReferencePool.Acquire<AtkEventArgs>()
                                .Fill(m_Fsm.Owner.Seat, m_Fsm.Owner.Second.GetImpact().Camp));
                        m_Fsm.Owner.Second = null;
                    }

                    return;
                }
            }

            ChangeState<FSeatStart>(m_Fsm);
        }

        protected override void OnUpdate(IFsm<NormalGame> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }


        /// <summary>
        /// 判断战斗结束
        /// </summary>
        private bool IsGameOver()
        {
            foreach (Role role in GameEntry.Role.EnemyRole.Values)
            {
                if (!role.GetImpact().Die)
                {
                    return false;
                }
            }

            foreach (Role role in GameEntry.Role.MyRole.Values)
            {
                if (!role.GetImpact().Die)
                {
                    return false;
                }
            }

            ChangeState<FEnd>(m_Fsm);
            return true;
        }

        protected override void OnLeave(IFsm<NormalGame> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            GameEntry.Event.Unsubscribe(NextRoleEventArgs.EventId, OnAtkEnd);
            GameEntry.Event.Unsubscribe(ExertBuffEventArgs.EventId, OnAtkEnd);
        }

        private void OnAtkEnd(object sender, GameEventArgs e)
        {
            if (e is NextRoleEventArgs || e is ExertBuffEventArgs)
            {
                GetAttacker();
            }
        }
    }
}