using Runtime.Interface;

namespace Runtime.Signal
{
    public readonly struct SendDefenderToBinSignal
    {
        public readonly IDefender Defender;
        
        public SendDefenderToBinSignal(IDefender defender)
        {
            Defender = defender;
        }
    }
}