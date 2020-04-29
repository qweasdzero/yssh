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
    public class FSeatStart : FsmBase
    {
        protected override void OnEnter(IFsm<NormalGame> fsm)
        {
            base.OnEnter(fsm);
            //计算下一个目标
            if (fsm.Owner.Seat >= 5)
            {
                ChangeState<FRoundStart>(fsm);
                return;
            }
            
            fsm.Owner.Seat = (fsm.Owner.Seat % 5) + 1;
            Atk(fsm);
        }

        private void Atk(IFsm<NormalGame> fsm)
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

            ChangeState<FSeat>(fsm);
        }
        
        protected override void OnLeave(IFsm<NormalGame> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }
    }
}