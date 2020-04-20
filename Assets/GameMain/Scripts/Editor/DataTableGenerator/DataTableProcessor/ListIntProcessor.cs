using System;
using System.Collections.Generic;
using System.IO;
using SG1;
using UnityGameFramework.Runtime;

namespace StarForce.Editor
{
    public class ListIntProcessor : ListGenericDataProcessor<VarListInt>
    {
        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "varlist<int>",
                "varlist<Int32>",
                "varlist<System.Int32>",
                "list<int>",
                "list<Int32>",
                "list<System.Int32>",
            };
        }

        public override void WriteToStream(BinaryWriter stream, string value)
        {
            value = value.SubString('(', ')');
            string[] splitValue = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            stream.Write(splitValue.Length);
            for (int i = 0; i < splitValue.Length; i++)
            {
                int element = int.Parse(splitValue[i]);
                stream.Write(element);
            }
        }

        public override VarListInt Parse(string value)
        {
            value = value.SubString('(', ')');
            string[] splitValue = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            List<int> list = new List<int>();
            for (int i = 0; i < splitValue.Length; i++)
            {
                int element = int.Parse(splitValue[i]);
                list.Add(element);
            }

            return new VarListInt(list);
        }
    }
}