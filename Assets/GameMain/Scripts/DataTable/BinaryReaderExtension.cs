using System.Collections.Generic;
using StarForce;
using UnityEngine;

namespace System.IO
{
    public static class BinaryReaderExtension
    {
        public static Color32 ReadColor32(this BinaryReader binaryReader)
        {
            return new Color32(binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte(),
                binaryReader.ReadByte());
        }

        public static Color ReadColor(this BinaryReader binaryReader)
        {
            return new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(),
                binaryReader.ReadSingle());
        }

        public static DateTime ReadDateTime(this BinaryReader binaryReader)
        {
            return new DateTime(binaryReader.ReadInt64());
        }

        public static Quaternion ReadQuaternion(this BinaryReader binaryReader)
        {
            return new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(),
                binaryReader.ReadSingle());
        }

        public static Rect ReadRect(this BinaryReader binaryReader)
        {
            return new Rect(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(),
                binaryReader.ReadSingle());
        }

        public static Vector2 ReadVector2(this BinaryReader binaryReader)
        {
            return new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }

        public static Vector3 ReadVector3(this BinaryReader binaryReader)
        {
            return new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }

        public static Vector4 ReadVector4(this BinaryReader binaryReader)
        {
            return new Vector4(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(),
                binaryReader.ReadSingle());
        }
        
        public static VarListInt ReadVarListInt(this BinaryReader binaryReader)
        {
            int count = binaryReader.ReadInt32();
            List<int> list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                int element = binaryReader.ReadInt32();
                list.Add(element);
            }

            return new VarListInt(list);
        }

        public static VarListString ReadVarListString(this BinaryReader binaryReader)
        {
            int count = binaryReader.ReadInt32();
            List<string> list = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                string element = binaryReader.ReadString();
                list.Add(element);
            }

            return new VarListString(list);
        }

        public static VarListFloat ReadVarListFloat(this BinaryReader binaryReader)
        {
            int count = binaryReader.ReadInt32();
            List<float> list = new List<float>(count);
            for (int i = 0; i < count; i++)
            {
                float element = binaryReader.ReadSingle();
                list.Add(element);
            }

            return new VarListFloat(list);
        }

        public static VarListDouble ReadVarListDouble(this BinaryReader binaryReader)
        {
            int count = binaryReader.ReadInt32();
            List<double> list = new List<double>(count);
            for (int i = 0; i < count; i++)
            {
                double element = binaryReader.ReadDouble();
                list.Add(element);
            }

            return new VarListDouble(list);
        }

//        public static VarListVipMachine ReadVarListVipMachine(this BinaryReader binaryReader)
//        {
//            int count = binaryReader.ReadInt32();
//            List<VipMachine> list = new List<VipMachine>(count);
//            for (int i = 0; i < count; i++)
//            {
//                int goodId = binaryReader.ReadInt32();
//                int num = binaryReader.ReadInt32();
//                VipMachine element = new VipMachine(goodId, num);
//                list.Add(element);
//            }
//            
//            return new VarListVipMachine(list);
//        }
    }
}