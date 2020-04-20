//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using GameFramework;
using StarForce;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor.DataTableTools;

    namespace StarForce.Editor.DataTableTools
{
    public sealed class DataTableGeneratorMenu
    {
        private static string[] s_DataTableNames = new string[]
        {
        };

        [MenuItem(Constant.AssemblyInfo.Namespace + "/Generate DataTables")]
        private static void GenerateDataTables()
        {
            List<string> dataTableNames = new List<string>(s_DataTableNames);
            dataTableNames.AddRange(ProcedurePreload.DataTableNames);

            foreach (string dataTableName in dataTableNames)
            {
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }

                DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
                DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }

            AssetDatabase.Refresh();
        }
    }
}