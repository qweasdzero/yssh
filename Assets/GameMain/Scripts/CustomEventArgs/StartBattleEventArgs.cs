using GameFramework.Event;

namespace StarForce
{
    public class StartBattleEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            
        }
        public static readonly int EventId = typeof(StartBattleEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public StartBattleEventArgs Fill()
        {

            return this;
        }
    }
}