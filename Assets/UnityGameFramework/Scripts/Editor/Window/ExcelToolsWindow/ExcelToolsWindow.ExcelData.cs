using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Excel;
using GameFramework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    public sealed partial class ExcelToolsWindow
    {
        private sealed class ExcelData
        {
            /// <summary>
            /// 表格数据集合
            /// </summary>
            private DataSet m_ResultSet;

            public ExcelData(string excelFile)
            {
                // 文件占用冲突。
                FileStream stream = File.Open(excelFile, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                try
                {
                    m_ResultSet = excelReader.AsDataSet();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                finally
                {
                    excelReader.Close();
                    stream.Close();
                }
            }

            public int TableCount
            {
                get { return m_ResultSet.Tables.Count; }
            }

            public List<string> TableNames
            {
                get
                {
                    List<string> result = new List<string>();
                    
                    for (int i = 0; i < m_ResultSet.Tables.Count; i++)
                    {
                        result.Add(m_ResultSet.Tables[i].TableName);
                    }

                    return result;
                }
            }

            /// <summary>
            /// 获取指定表格。
            /// </summary>
            /// <param name="index">表格编号。</param>
            /// <returns><see cref="DataTable"/></returns>
            /// <exception cref="GameFrameworkException">编号范围错误。</exception>
            public DataTable GetDataTable(int index)
            {
                //判断Excel文件中是否存在数据表
                if (index >= m_ResultSet.Tables.Count || index < 0)
                {
                    throw new GameFrameworkException("Index out of range exception");
                }

                return m_ResultSet.Tables[index];
            }

            /// <summary>
            /// 导出为Txt。
            /// </summary>
            /// <param name="index">表格编号。</param>
            /// <param name="encoding">编码格式。</param>
            public StringBuilder ConvertToTxt(int index, Encoding encoding)
            {
                //判断Excel文件中是否存在数据表
                if (index >= m_ResultSet.Tables.Count || index < 0)
                {
                    throw new GameFrameworkException("Index out of range exception");
                }

                //创建一个StringBuilder存储数据
                StringBuilder stringBuilder = new StringBuilder();

                //默认读取第一个数据表
                DataTable sheet = m_ResultSet.Tables[index];

                //判断数据表内是否存在数据
                if (sheet.Rows.Count < 1)
                {
                    return stringBuilder;
                }

                //读取数据表行数和列数
                int rowCount = sheet.Rows.Count;
                int colCount = sheet.Columns.Count;


                //读取数据
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        //使用","分割每一个数值
                        stringBuilder.Append(sheet.Rows[i][j]);
                        if (j != colCount - 1)
                        {
                            stringBuilder.Append("\t");
                        }
                    }

                    //使用换行符分割每一行
                    stringBuilder.Append("\n");
                }

                return stringBuilder;
            }

            /// <summary>
            /// 导出为Xml。
            /// </summary>
            /// <param name="index">表格编号。</param>
            /// <param name="encoding">编码格式。</param>
            public StringBuilder ConvertToXML(int index, Encoding encoding)
            {
                //判断Excel文件中是否存在数据表
                if (index >= m_ResultSet.Tables.Count || index < 0)
                {
                    throw new GameFrameworkException("Index out of range exception");
                }

                //创建一个StringBuilder存储数据
                StringBuilder stringBuilder = new StringBuilder();

                //默认读取第一个数据表
                DataTable sheet = m_ResultSet.Tables[index];

                //判断数据表内是否存在数据
                if (sheet.Rows.Count < 1)
                {
                    return stringBuilder;
                }

                //读取数据表行数和列数
                int rowCount = sheet.Rows.Count;
                int colCount = sheet.Columns.Count;

                stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>").AppendLine("<Dictionaries>");

                stringBuilder.AppendFormat("  <Dictionary Language=\"{0}\">", sheet.Rows[1][3]).AppendLine();

                //读取数据
                for (int i = 0; i < rowCount; i++)
                {
                    if (sheet.Rows[i][0].ToString().Trim() == "#")
                    {
                        continue;
                    }

                    stringBuilder.AppendFormat("   <String Key=\"{0}\" Value=\"{1}\" />", sheet.Rows[i][1],
                        sheet.Rows[i][3]).AppendLine();
                }

                stringBuilder.AppendLine("  </Dictionary>");
                stringBuilder.AppendLine("</Dictionaries>");


                return stringBuilder;
            }

            public StringBuilder ConvertToEnum(int index, Encoding encoding)
            {
                //判断Excel文件中是否存在数据表
                if (index >= m_ResultSet.Tables.Count || index < 0)
                {
                    throw new GameFrameworkException("Index out of range exception");
                }

                //创建一个StringBuilder存储数据
                StringBuilder stringBuilder = new StringBuilder();

                //默认读取第一个数据表
                DataTable sheet = m_ResultSet.Tables[index];

                //判断数据表内是否存在数据
                if (sheet.Rows.Count < 1)
                {
                    return stringBuilder;
                }

                //读取数据表行数和列数
                int rowCount = sheet.Rows.Count;
                int colCount = sheet.Columns.Count;


                stringBuilder.AppendLine("namespace SG1").AppendLine("{").Append(' ', 4)
                    .AppendFormat("public partial class {0}Game", sheet.TableName).AppendLine().Append(' ', 4)
                    .AppendLine("{");
                stringBuilder.Append(' ', 8).AppendLine("/// <summary>").Append(' ', 8)
                    .AppendFormat("/// {0}", sheet.Rows[0][1]).AppendLine().Append(' ', 8).AppendLine("/// </summary>");
                stringBuilder.Append(' ', 8).AppendFormat("protected enum {0}Enum", sheet.TableName).AppendLine()
                    .Append(' ', 8).AppendLine("{").Append(' ', 12).AppendLine("Unknow = 0,").AppendLine();

                int count = 1;
                //读取数据k
                for (int i = 0; i < rowCount; i++)
                {
                    if (sheet.Rows[i][0].ToString().Trim() == "#")
                    {
                        continue;
                    }

                    stringBuilder.Append(' ', 12).AppendLine("/// <summary>").Append(' ', 12)
                        .AppendFormat("/// {0}", sheet.Rows[i][2]).AppendLine().Append(' ', 12)
                        .AppendLine("/// </summary>");
                    stringBuilder.Append(' ', 12).AppendFormat("{0} = {1},", sheet.Rows[i][3], sheet.Rows[i][1])
                        .AppendLine().AppendLine();
                    count++;
                }


                stringBuilder.Append(' ', 12).AppendLine("/// <summary>").Append(' ', 12).AppendFormat("/// 随机地图的最大值。")
                    .AppendLine().Append(' ', 12).AppendLine("/// </summary>");
                stringBuilder.Append(' ', 12).AppendFormat("Max = {0},", count).AppendLine();
                stringBuilder.Append(' ', 8).AppendLine("}");
                stringBuilder.Append(' ', 4).AppendLine("}");
                stringBuilder.AppendLine("}");

                return stringBuilder;
            }


            public void CreateTxt(string filePath, int index, Encoding encoding)
            {
                StringBuilder stringBuilder = ConvertToTxt(index, encoding);

                //写入文件
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
                    {
                        textWriter.Write(stringBuilder.ToString());
                    }
                }
            }

            public void CreateXML(string filePath, int index, Encoding encoding)
            {
                StringBuilder stringBuilder = ConvertToXML(index, encoding);

                filePath = GameFramework.Utility.Path.GetCombinePath("Assets/GameMain/Localization",
                    Path.GetFileNameWithoutExtension(filePath), "Dictionaries");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                filePath = GameFramework.Utility.Path.GetCombinePath(filePath, "Default.xml");
                //写入文件
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
                    {
                        textWriter.Write(stringBuilder.ToString());
                    }
                }
            }


            public void CreateEnum(string filePath, int index, Encoding encoding)
            {
                StringBuilder stringBuilder = ConvertToEnum(index, encoding);

                filePath = GameFramework.Utility.Path.GetCombinePath("Assets/GameMain/Scripts/Definition/Enum",
                    Path.GetFileName(filePath));
                //写入文件
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
                    {
                        textWriter.Write(stringBuilder.ToString());
                    }
                }
            }
        }
    }
}