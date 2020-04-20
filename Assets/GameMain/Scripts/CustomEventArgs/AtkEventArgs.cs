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
        public int EnemySeat;

        public AtkEventArgs Fill(int seat, CampType campType,int enemyseat)
        {
            Seat = seat;
            CampType = campType;
            EnemySeat = enemyseat;
            return this;
        }
    }
}