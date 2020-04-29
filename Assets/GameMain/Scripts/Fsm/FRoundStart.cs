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
            fsm.Owner.Round += 1;
            fsm.Owner.Seat = 0;
            if (fsm.Owner.Round >= 5)
            {
                GameEntry.Skill.Power += 10;
            }
            else
            {
                GameEntry.Skill.Power = m_PowerList[fsm.Owner.Round - 1];
            }
            GameEntry.Event.Fire(this, ReferencePool.Acquire<NextRoundEventArgs>().Fill());
            ChangeState<FSeatStart>(fsm);
            //计算下一个目标
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