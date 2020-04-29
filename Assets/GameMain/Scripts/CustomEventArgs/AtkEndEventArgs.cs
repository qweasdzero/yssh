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

        public int Seat;
        public CampType CampType;
        public int SkillId;

        public AtkEndEventArgs Fill(int seat, CampType campType,int skillId)
        {
            Seat = seat;
            CampType = campType;
            SkillId = skillId;
            return this;
        }
    }
}