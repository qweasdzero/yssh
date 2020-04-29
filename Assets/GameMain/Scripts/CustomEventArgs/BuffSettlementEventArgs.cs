using GameFramework.Event;

namespace StarForce
{
    public class BuffSettlementEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            
        }
        public static readonly int EventId = typeof(BuffSettlementEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public BuffSettlementEventArgs Fill()
        {
            return this;
        }
    }
}