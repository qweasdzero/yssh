using System.Collections.Generic;
using GameFramework.Event;

namespace StarForce
{
    public class AtkEndEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            
        }
        public static readonly int EventId = typeof(AtkEndEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public List<int>  Seat;
        public CampType CampType;
        public int Hurt;

        public AtkEndEventArgs Fill(List<int> seat, CampType campType,int hurt)
        {
            Seat = seat;
            CampType = campType;
            Hurt = hurt;
            return this;
        }
    }
}