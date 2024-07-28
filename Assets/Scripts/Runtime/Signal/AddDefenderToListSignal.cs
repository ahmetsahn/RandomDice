using Runtime.Interface;

namespace Runtime.Signal
{
    public readonly struct AddDefenderToListSignal
    {
        public readonly IDefender Defender;
        
        public AddDefenderToListSignal(IDefender defender)
        {
            Defender = defender;
        }
    }
}