using System.Collections.Generic;
using GameFramework.Event;

namespace StarForce
{
    public class HurtEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            
        }
        public static readonly int EventId = typeof(HurtEventArgs).GetHashCode();
        
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

        public HurtEventArgs Fill(List<int> seat, CampType campType,int hurt)
        {
            Seat = seat;
            CampType = campType;
            Hurt = hurt;
            return this;
        }
    }
}