using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using StarForce;
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

        public Dictionary<int, Role> GetTarget(CampType campType)
        {
            switch (campType)
            {
                case CampType.Unknown:
                    break;
                case CampType.Player:
                    return MyRole;
                case CampType.Enemy:
                    return EnemyRole;
            }

            return null;
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

        [SerializeField]public bl_HUDText HudText;
    }
}