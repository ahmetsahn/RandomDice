using Runtime.Interface;
using UnityEngine;

namespace Runtime.Signal
{
    public readonly struct BossSequenceSignal
    {
        public readonly IEnemy Boss;
        
        public BossSequenceSignal(IEnemy boss)
        {
            Boss = boss;
        }
    }
}