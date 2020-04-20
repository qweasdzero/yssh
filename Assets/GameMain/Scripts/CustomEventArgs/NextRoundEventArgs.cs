using GameFramework.Event;

namespace StarForce
{
    public class NextRoundEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            
        }
        public static readonly int EventId = typeof(NextRoundEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public NextRoundEventArgs Fill()
        {
            return this;
        }
    }
}