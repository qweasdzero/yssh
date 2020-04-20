using GameFramework.Event;

namespace StarForce
{
    public class RoleDieEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            
        }
        public static readonly int EventId = typeof(RoleDieEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int Seat;
        public CampType CampType;

        public RoleDieEventArgs Fill(int seat, CampType campType)
        {
            Seat = seat;
            CampType = campType;
            return this;
        }
    }
}