using Runtime.Interface;

namespace Runtime.Signal
{
    public readonly struct RemoveDefenderFromListSignal
    {
        public readonly IDefender Defender;
        
        public RemoveDefenderFromListSignal(IDefender defender)
        {
            Defender = defender;
        }
    }
}