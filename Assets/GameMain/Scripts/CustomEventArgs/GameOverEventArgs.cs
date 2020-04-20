using GameFramework.Event;

namespace StarForce
{
    public class GameOverEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            
        }
        public static readonly int EventId = typeof(GameOverEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public GameOverEventArgs Fill()
        {

            return this;
        }
    }
}