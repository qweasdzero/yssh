using System.Collections.Generic;

namespace StarForce
{
    public class VarListInt : VarList<int>
    {
        public VarListInt()
        {
        }

        public VarListInt(List<int> value) : base(value)
        {
        }
    }
}