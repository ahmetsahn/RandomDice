namespace Runtime.Signal
{
    public readonly struct UpdateUpgradeDefenderButtonStateSignal
    {
        public readonly int CurrentEnergy;
        
        public UpdateUpgradeDefenderButtonStateSignal(int currentEnergy)
        {
            CurrentEnergy = currentEnergy;
        }
    }
}