using GameFramework.Event;

namespace StarForce
{
    public class SkillEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            
        }
        public static readonly int EventId = typeof(SkillEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int Seat;
        public CampType CampType;

        public SkillEventArgs Fill(int seat, CampType campType)
        {
            Seat = seat;
            CampType = campType;
            return this;
        }
    }
}