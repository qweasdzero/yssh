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
    public class FStart : FsmBase
    {
        protected override void OnEnter(IFsm<NormalGame> fsm)
        {
            base.OnEnter(fsm);
            GameEntry.Role.MyRole = new Dictionary<int, Role>();
            GameEntry.Role.EnemyRole = new Dictionary<int, Role>();
            GameEntry.Role.MyAtkDic = new Dictionary<int, Stack<int>>();
            GameEntry.Role.EnemyAtkDic = new Dictionary<int, Stack<int>>();

            //获取人物信息并生成

            for (int i = 0; i < 5; i++)
            {
                GameEntry.Entity.ShowRole(new RoleData(GameEntry.Entity.GenerateSerialId(), 1000)
                {
                    Camp = CampType.Player,
                    Seat = i + 1,
                    Hp = 200,
                    HpMax = 200,
                    Speed = 100 + i,
                });
            }

            for (int i = 0; i < 5; i++)
            {
                GameEntry.Entity.ShowRole(new RoleData(GameEntry.Entity.GenerateSerialId(), 1000)
                {
                    Camp = CampType.Enemy,
                    Seat = i + 1,
                    Hp = 200,
                    HpMax = 200,
                    Speed = 100 + 2 * i,
                });
            }

            fsm.Owner.Round = 0;
            
            GameEntry.Skill.Skill1Consume = 1;
            GameEntry.Skill.Skill2Consume = 3;
            GameEntry.Skill.Skill3Consume = 5;
            GameEntry.Skill.Skill4Consume = 7;
            GameEntry.Skill.Skill5Consume = 9;

            GameEntry.Skill.Skill1CdMax = 2;
            GameEntry.Skill.Skill2CdMax = 2;
            GameEntry.Skill.Skill3CdMax = 2;
            GameEntry.Skill.Skill4CdMax = 2;
            GameEntry.Skill.Skill5CdMax = 2;
            
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = e as ShowEntitySuccessEventArgs;
            if (ne == null)
            {
                return;
            }


            Role role = ne.Entity.Logic as Role;
            if (role == null)
            {
                return;
            }

            RoleImpactData roleImpactData = role.GetImpact();
            if (roleImpactData.Camp == CampType.Enemy)
            {
                GameEntry.Role.EnemyRole.Add(roleImpactData.Seat, role);
                Stack<int> stack = new Stack<int>();
                List<int> list = RoleUtility.GetRole(roleImpactData.Seat);
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    stack.Push(list[i]);
                }

                GameEntry.Role.EnemyAtkDic.Add(roleImpactData.Seat, stack);
            }

            if (roleImpactData.Camp == CampType.Player)
            {
                GameEntry.Role.MyRole.Add(roleImpactData.Seat, role);
                Stack<int> stack = new Stack<int>();
                List<int> list = RoleUtility.GetRole(roleImpactData.Seat);
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    stack.Push(list[i]);
                }

                GameEntry.Role.MyAtkDic.Add(roleImpactData.Seat, stack);
            }
        }

        protected override void OnUpdate(IFsm<NormalGame> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if (GameEntry.Role.MyRole.Count >= 5 && GameEntry.Role.EnemyRole.Count >= 5)
            {
                ChangeState<FRoundStart>(fsm);
            }
        }

        protected override void OnLeave(IFsm<NormalGame> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }
    }
}