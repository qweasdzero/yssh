using GameFramework.Event;

namespace StarForce
{
    public class NextRoleEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            
        }
        public static readonly int EventId = typeof(NextRoleEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public NextRoleEventArgs Fill()
        {
            return this;
        }
    }
}