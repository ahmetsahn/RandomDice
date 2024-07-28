using Runtime.Interface;

namespace Runtime.Signal
{
    public readonly struct MergeDefendersSignal
    {
        public readonly IDefender Defender;

        public MergeDefendersSignal(IDefender defender)
        {
            Defender = defender;
        }
    }
}