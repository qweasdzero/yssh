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
    public class FEnd : FsmBase
    {
        
       protected override void OnEnter(IFsm<NormalGame> fsm)
        {
            base.OnEnter(fsm);
            GameEntry.Event.Fire(this, ReferencePool.Acquire<GameOverEventArgs>().Fill());
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