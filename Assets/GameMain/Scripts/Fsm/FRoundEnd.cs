﻿using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;

namespace StarForce
{
    public class FRoundEnd : FsmBase
    {
        protected override void OnEnter(IFsm<NormalGame> fsm)
        {
            base.OnEnter(fsm);
            if (fsm.Owner.SlowAtk.Count > 0)
            {
                
            }
            
            GameEntry.Event.Fire(this, ReferencePool.Acquire<BuffSettlementEventArgs>().Fill());
            ChangeState<FRoundStart>(fsm);
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