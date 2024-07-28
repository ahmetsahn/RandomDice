namespace Runtime.Signal
{
    public readonly struct ReduceCurrentEnergySignal
    {
        public readonly int EnergyAmount;
        
        public ReduceCurrentEnergySignal(int energyAmount)
        {
            EnergyAmount = energyAmount;
        }
    }
}