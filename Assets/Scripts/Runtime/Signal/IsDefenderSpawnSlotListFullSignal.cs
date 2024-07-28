namespace Runtime.Signal
{
    public readonly struct IsDefenderSpawnSlotListFullSignal
    {
        public readonly bool IsFull;
        
        public IsDefenderSpawnSlotListFullSignal(bool isFull)
        {
            IsFull = isFull;
        }
    }
}