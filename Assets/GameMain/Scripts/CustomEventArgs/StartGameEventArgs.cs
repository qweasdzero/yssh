using GameFramework.Event;

namespace StarForce
{
    public class StartGameEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            
        }
        public static readonly int EventId = typeof(StartGameEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public StartGameEventArgs Fill()
        {

            return this;
        }
    }
}