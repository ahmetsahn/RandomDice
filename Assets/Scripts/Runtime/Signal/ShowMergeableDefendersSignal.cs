using Runtime.DefenderSystem.Model;
using Runtime.DefenderSystem.View;
using Runtime.Enum;
using Runtime.Interface;

namespace Runtime.Signal
{
    public readonly struct ShowMergeableDefendersSignal
    {
        public readonly IDefender Defender;
        
        public ShowMergeableDefendersSignal(IDefender defender)
        {
            Defender = defender;
        }
    }
}