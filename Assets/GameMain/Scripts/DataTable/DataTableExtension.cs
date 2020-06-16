//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public static class DataTableExtension
    {
        private const string DataRowClassPrefixName = Constant.AssemblyInfo.Namespace + ".DR";
        internal static readonly char[] DataSplitSeparators = new char[] {'\t'};
        internal static readonly char[] DataTrimSeparators = new char[] {'\"'};

        public static void LoadDataTable(this DataTableComponent dataTableComponent, string dataTableName,
            LoadType loadType, object userData = null)
        {
            if (string.IsNullOrEmpty(dataTableName))
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string[] splitNames = dataTableName.Split('_');
            if (splitNames.Length > 2)
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string dataRowClassName = DataRowClassPrefixName + splitNames[0];

            Type dataRowType = Type.GetType(dataRowClassName);
            if (dataRowType == null)
            {
                Log.Warning("Can not get data row type with class name '{0}'.", dataRowClassName);
                return;
            }

            string dataTableNameInType = splitNames.Length > 1 ? splitNames[1] : null;
            dataTableComponent.LoadDataTable(dataRowType, dataTableName, dataTableNameInType,
                AssetUtility.GetDataTableAsset(dataTableName, loadType), loadType,
                Constant.AssetPriority.DataTableAsset, userData);
        }

        public static Color32 ParseColor32(string value)
        {
            string[] splitValue = value.Split(',');
            return new Color32(byte.Parse(splitValue[0]), byte.Parse(splitValue[1]), byte.Parse(splitValue[2]),
                byte.Parse(splitValue[3]));
        }

        public static Color ParseColor(string value)
        {
            string[] splitValue = value.Split(',');
            return new Color(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]),
                float.Parse(splitValue[3]));
        }

        public static Quaternion ParseQuaternion(string value)
        {
            string[] splitValue = value.Split(',');
            return new Quaternion(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]),
                float.Parse(splitValue[3]));
        }

        public static Rect ParseRect(string value)
        {
            string[] splitValue = value.Split(',');
            return new Rect(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]),
                float.Parse(splitValue[3]));
        }

        public static Vector2 ParseVector2(string value)
        {
            string[] splitValue = value.Split(',');
            return new Vector2(float.Parse(splitValue[0]), float.Parse(splitValue[1]));
        }

        public static Vector3 ParseVector3(string value)
        {
            string[] splitValue = value.Split(',');
            return new Vector3(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]));
        }

        public static Vector4 ParseVector4(string value)
        {
            string[] splitValue = value.Split(',');
            return new Vector4(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]),
                float.Parse(splitValue[3]));
        }

        public static VarListInt ParseVarListInt(string value)
        {
            value = value.SubString('(', ')');
            string[] splitValues = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            List<int> list = new List<int>(splitValues.Length);
            foreach (var splitValue in splitValues)
            {
                int element = int.Parse(splitValue);
                list.Add(element);
            }

            return new VarListInt(list);
        }

        public static VarListString ParseVarListString(string value)
        {
            value = value.SubString('(', ')');
            string[] splitValues = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            List<string> list = new List<string>(splitValues.Length);
            foreach (var splitValue in splitValues)
            {
                string element = splitValue;
                list.Add(element);
            }

            return new VarListString(list);
        }

        public static VarListFloat ParseVarListFloat(string value)
        {
            value = value.SubString('(', ')');
            string[] splitValues = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            List<float> list = new List<float>(splitValues.Length);
            foreach (var splitValue in splitValues)
            {
                float element = float.Parse(splitValue);
                list.Add(element);
            }

            return new VarListFloat(list);
        }

        public static VarListDouble ParseVarListDouble(string value)
        {
            value = value.SubString('(', ')');
            string[] splitValues = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            List<double> list = new List<double>(splitValues.Length);
            foreach (var splitValue in splitValues)
            {
                double element = double.Parse(splitValue);
                list.Add(element);
            }

            return new VarListDouble(list);
        }

//        public static VarListVipMachine ParseVarListVipMachine(string value)
//        {
//            value = value.SubString('(', ')');
//            string[] splitValue = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
//            List<VipMachine> list = new List<VipMachine>();
//            for (int i = 0; i < splitValue.Length; i++)
//            {
//                string rowString = splitValue[i];
//                rowString = rowString.SubString('[', ']');
//                string[] rowSplitValue = rowString.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
//                int goodId = int.Parse(rowSplitValue[0]);
//                int num = int.Parse(rowSplitValue[1]);
//                VipMachine element = new VipMachine(goodId, num);
//                list.Add(element);
//            }
//            
//            return new VarListVipMachine(list);
//        }
    }
}
