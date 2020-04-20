using System.Collections.Generic;

namespace StarForce
{
    public class VarListDouble : VarList<double>
    {
        public VarListDouble()
        {
        }

        public VarListDouble(List<double> value) : base(value)
        {
        }
    }
}