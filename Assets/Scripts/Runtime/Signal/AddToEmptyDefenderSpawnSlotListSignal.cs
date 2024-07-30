using UnityEngine;

namespace Runtime.Signal
{
    public readonly struct AddToEmptyDefenderSpawnSlotListSignal
    {
        public readonly Vector3 EmptyDefenderSpawnSlot;
        
        public AddToEmptyDefenderSpawnSlotListSignal(Vector3 emptyDefenderSpawnSlot)
        {
            EmptyDefenderSpawnSlot = emptyDefenderSpawnSlot;
        }
    }
}