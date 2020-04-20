using System;
using System.Collections.Generic;
using System.IO;
using SG1;
using UnityGameFramework.Runtime;

namespace StarForce.Editor
{
    public class ListStringProcessor : ListGenericDataProcessor<VarListString>
    {
        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "varlist<string>",
                "varlist<System.String>",
                "list<string>",
                "list<System.String>",
            };
        }

        public override void WriteToStream(BinaryWriter stream, string value)
        {
            value = value.SubString('(', ')');
            string[] splitValue = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            stream.Write(splitValue.Length);
            for (int i = 0; i < splitValue.Length; i++)
            {
                string element = splitValue[i];
                stream.Write(element);
            }
        }

        public override VarListString Parse(string value)
        {
            value = value.SubString('(', ')');
            string[] splitValue = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            List<string> list = new List<string>();
            for (int i = 0; i < splitValue.Length; i++)
            {
                string element = splitValue[i];
                list.Add(element);
            }

            return new VarListString(list);
        }
    }
}