using System.Collections.Generic;

namespace StarForce
{
    public class VarListString : VarList<string>
    {
        public VarListString()
        {
        }

        public VarListString(List<string> value) : base(value)
        {
        }
    }
}