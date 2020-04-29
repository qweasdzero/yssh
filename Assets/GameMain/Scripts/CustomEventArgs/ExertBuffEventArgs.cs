using System.Collections.Generic;
using GameFramework.Event;

namespace StarForce
{
    public class ExertBuffEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            
        }
        public static readonly int EventId = typeof(ExertBuffEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public List<int>  Seat;
        public CampType CampType;
        public Buff Buff;
        public int BuffTime;
        public int BuffValue;

        public ExertBuffEventArgs Fill(List<int> seat, CampType campType,Buff buff,int buffTime,int buffValue)
        {
            Seat = seat;
            CampType = campType;
            Buff = buff;
            BuffTime = buffTime;
            BuffValue = buffValue;
            return this;
        }
    }
}