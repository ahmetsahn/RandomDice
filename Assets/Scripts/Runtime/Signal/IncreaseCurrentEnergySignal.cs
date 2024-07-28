namespace Runtime.Signal
{
    public readonly struct IncreaseCurrentEnergySignal
    {
        public readonly int EnergyAmount;

        public IncreaseCurrentEnergySignal(int energyAmount)
        {
            EnergyAmount = energyAmount;
        }
    }
}