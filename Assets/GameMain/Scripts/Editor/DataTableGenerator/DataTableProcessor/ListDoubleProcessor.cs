using System;
using System.Collections.Generic;
using System.IO;
using SG1;
using UnityGameFramework.Runtime;

namespace StarForce.Editor
{
    public class ListDoubleProcessor : ListGenericDataProcessor<VarListDouble>
    {
        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "varlist<double>",
                "varlist<System.Double>",
                "list<double>",
                "list<System.Double>",
            };
        }

        public override void WriteToStream(BinaryWriter stream, string value)
        {
            value = value.SubString('(', ')');
            string[] splitValue = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            stream.Write(splitValue.Length);
            for (int i = 0; i < splitValue.Length; i++)
            {
                double element = double.Parse(splitValue[i]);
                stream.Write(element);
            }
        }

        public override VarListDouble Parse(string value)
        {
            value = value.SubString('(', ')');
            string[] splitValue = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            List<double> list = new List<double>();
            for (int i = 0; i < splitValue.Length; i++)
            {
                double element = double.Parse(splitValue[i]);
                list.Add(element);
            }

            return new VarListDouble(list);
        }
    }
}