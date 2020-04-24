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
    public class FRound : FsmBase
    {
        protected override void OnEnter(IFsm<NormalGame> fsm)
        {
            base.OnEnter(fsm);
            if (fsm.Owner.First != null)
            {
                if (!fsm.Owner.First.GetImpact().Die) //判断是否可以攻击
                {
                    int target = GetAtkTarget(fsm.Owner.First.GetImpact());

                    GameEntry.Event.Fire(this,
                        ReferencePool.Acquire<AtkEventArgs>()
                            .Fill(fsm.Owner.Seat, fsm.Owner.First.GetImpact().Camp, target));
                    fsm.Owner.First = null;
                }
            }
            else if (fsm.Owner.Second != null)
            {
                if (!fsm.Owner.First.GetImpact().Die) //判断是否可以攻击
                {
                    int target = GetAtkTarget(fsm.Owner.First.GetImpact());

                    GameEntry.Event.Fire(this,
                        ReferencePool.Acquire<AtkEventArgs>()
                            .Fill(fsm.Owner.Seat, fsm.Owner.First.GetImpact().Camp, target));
                }
            }
            else
            {
                ChangeState<FRoundEnd>(fsm);
            }
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