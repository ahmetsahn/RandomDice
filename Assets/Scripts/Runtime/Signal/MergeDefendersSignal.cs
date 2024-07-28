using Runtime.DefenderSystem.Model;
using Runtime.DefenderSystem.View;
using Runtime.Interface;
using UnityEngine;

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