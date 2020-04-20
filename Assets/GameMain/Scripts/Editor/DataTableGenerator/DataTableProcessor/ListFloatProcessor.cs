using System;
using System.Collections.Generic;
using System.IO;
using SG1;
using UnityGameFramework.Runtime;

namespace StarForce.Editor
{
    public class ListFloatProcessor : ListGenericDataProcessor<VarListFloat>
    {
        public override string[] GetTypeStrings()
        {
            return new string[]
            {
                "varlist<float>",
                "varlist<System.Single>",
                "list<float>",
                "list<System.Single>",
            };
        }

        public override void WriteToStream(BinaryWriter stream, string value)
        {
            value = value.SubString('(', ')');
            string[] splitValue = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            stream.Write(splitValue.Length);
            for (int i = 0; i < splitValue.Length; i++)
            {
                float element = float.Parse(splitValue[i]);
                stream.Write(element);
            }
        }

        public override VarListFloat Parse(string value)
        {
            value = value.SubString('(', ')');
            string[] splitValue = value.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            List<float> list = new List<float>();
            for (int i = 0; i < splitValue.Length; i++)
            {
                int element = int.Parse(splitValue[i]);
                list.Add(element);
            }

            return new VarListFloat(list);
        }
    }
}