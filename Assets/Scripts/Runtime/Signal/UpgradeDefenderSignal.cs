using Runtime.Enum;

namespace Runtime.Signal
{
    public readonly struct UpgradeDefenderSignal
    {
        public readonly DefenderType DefenderType;
        
        public readonly int UpgradeLevel;
        
        public UpgradeDefenderSignal(DefenderType defenderType, int upgradeLevel)
        {
            DefenderType = defenderType;
            UpgradeLevel = upgradeLevel;
        }
    }
}