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
    public class FRoundEnd : FsmBase
    {
        public List<int> m_PowerList = new List<int>() {2, 4, 6, 8, 10};

        protected override void OnEnter(IFsm<NormalGame> fsm)
        {
            base.OnEnter(fsm);
           
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