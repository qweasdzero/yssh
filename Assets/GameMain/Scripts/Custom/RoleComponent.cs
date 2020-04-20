using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using SG1;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class RoleComponent : GameFrameworkComponent
    {
        private Dictionary<int, Role> m_MyRole;
        private Dictionary<int, Role> m_EnemyRole;
        private Dictionary<int, Stack<int>> m_MyAtkDic;
        private Dictionary<int, Stack<int>> m_EnemyAtkDic;

        public Dictionary<int, Role> MyRole
        {
            get { return m_MyRole; }
            set { m_MyRole = value; }
        }

        public Dictionary<int, Role> EnemyRole
        {
            get { return m_EnemyRole; }
            set { m_EnemyRole = value; }
        }

        public Dictionary<int, Stack<int>> MyAtkDic
        {
            get { return m_MyAtkDic; }
            set { m_MyAtkDic = value; }
        }

        public Dictionary<int, Stack<int>> EnemyAtkDic
        {
            get { return m_EnemyAtkDic; }
            set { m_EnemyAtkDic = value; }
        }

        public void Start()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        }


        // public void OnDestroy()
        // {
        //     GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        // }

        protected void OnShowEntitySuccess(object sender, GameEventArgs e)
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
                m_EnemyRole.Add(roleImpactData.Seat, role);
                Stack<int> stack = new Stack<int>();
                List<int> list = RoleUtility.GetRole(roleImpactData.Seat);
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    stack.Push(list[i]);
                }

                m_EnemyAtkDic.Add(roleImpactData.Seat, stack);
            }

            if (roleImpactData.Camp == CampType.Player)
            {
                m_MyRole.Add(roleImpactData.Seat, role);
                Stack<int> stack = new Stack<int>();
                List<int> list = RoleUtility.GetRole(roleImpactData.Seat);
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    stack.Push(list[i]);
                }

                m_MyAtkDic.Add(roleImpactData.Seat, stack);
            }

            if (m_MyRole.Count >= 5 && m_EnemyRole.Count >= 5)
            {
                GameEntry.Event.Fire(this,ReferencePool.Acquire<StartBattleEventArgs>());
            }
        }
    }
}