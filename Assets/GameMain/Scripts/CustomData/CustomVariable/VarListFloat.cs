using System.Collections.Generic;

namespace StarForce
{
    public class VarListFloat : VarList<float>
    {
        public VarListFloat()
        {
        }

        public VarListFloat(List<float> value) : base(value)
        {
        }
    }
}