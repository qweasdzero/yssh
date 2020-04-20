using GameFramework.Event;

namespace StarForce
{
    public class ActiveSkillEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            
        }
        public static readonly int EventId = typeof(ActiveSkillEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int Seat;
        public CampType CampType;

        public ActiveSkillEventArgs Fill(int seat, CampType campType)
        {
            Seat = seat;
            CampType = campType;
            return this;
        }
    }
}