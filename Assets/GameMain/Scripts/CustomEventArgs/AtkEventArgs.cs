using GameFramework.Event;

namespace StarForce
{
    public class AtkEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            
        }
        public static readonly int EventId = typeof(AtkEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int Seat;
        public CampType CampType;

        public AtkEventArgs Fill(int seat, CampType campType)
        {
            Seat = seat;
            CampType = campType;
            return this;
        }
    }
}