using System;
using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    /// <summary>
    /// 角色 工具类。
    /// </summary>
    public static class RoleUtility
    {
        public static List<Vector3> MyRolePos = new List<Vector3>()
        {
            Vector3.zero,new Vector3(-1.3f, -1, 0), new Vector3(0.6f, -1, 0), new Vector3(-2.3f, -2, 0), new Vector3(-0.2f, -2, 0),
            new Vector3(1.8f, -2, 0),
        }; 
        
        public static List<Vector3> EnemyRolePos = new List<Vector3>()
        {
            Vector3.zero,new Vector3(-1.1f, 1.8f, 0), new Vector3(0.9f, 1.8f, 0), new Vector3(-2f, 2.6f, 0), new Vector3(-0f, 2.6f, 0),
            new Vector3(1.8f, 2.6f, 0),
        };

        private static List<int> Role1 =new List<int>(5){1,2,4,3,5};
        private static List<int> Role2 =new List<int>(5){2,1,4,5,3};
        private static List<int> Role3 =new List<int>(5){1,2,3,4,5};
        private static List<int> Role4 =new List<int>(5){1,2,4,3,5};
        private static List<int> Role5 =new List<int>(5){2,1,5,4,3};

        public static List<int> GetRole(int i)
        {
            switch (i)
            {
                case 1:
                    return Role1;
                case 2:
                    return Role2;
                case 3:
                    return Role3;
                case 4:
                    return Role4;
                case 5:
                    return Role5;
            }
            return new List<int>();
        }
        
        public const int PowerMax = 100;
    }
}