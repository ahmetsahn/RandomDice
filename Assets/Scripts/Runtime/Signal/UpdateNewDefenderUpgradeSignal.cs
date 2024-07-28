using Runtime.Enum;
using Runtime.Interface;

namespace Runtime.Signal
{
    public readonly struct UpdateNewDefenderUpgradeSignal
    {
        public readonly IDefender Defender;
        
        public UpdateNewDefenderUpgradeSignal(IDefender defender)
        {
            Defender = defender;
        }
    }
}