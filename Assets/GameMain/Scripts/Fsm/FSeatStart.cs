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
        public List<int> m_PowerList = new List<int>() {2, 4, 6, 8, 10};

        protected override void OnEnter(IFsm<NormalGame> fsm)
        {
            base.OnEnter(fsm);
            //计算下一个目标
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

            ChangeState<FStart>(fsm);
        }

//         /// <summary>
//         /// 判断战斗结束
//         /// </summary>
//         private bool IsGameOver(CampType campType)
//         {
//             switch (campType)
//             {
//                 case CampType.Player:
//                     foreach (Role role in GameEntry.Role.EnemyRole.Values)
//                     {
//                         if (!role.GetImpact().Die)
//                         {
//                             return false;
//                         }
//                     }
//
//                     break;
//                 case CampType.Enemy:
//                     foreach (Role role in GameEntry.Role.MyRole.Values)
//                     {
//                         if (!role.GetImpact().Die)
//                         {
//                             return false;
//                         }
//                     }
//
//                     break;
//             }
//
// // m_Start = false;
//             GameEntry.Event.Fire(this, ReferencePool.Acquire<GameOverEventArgs>().Fill());
//             return true;
//         }




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