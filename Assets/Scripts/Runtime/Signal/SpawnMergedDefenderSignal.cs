using Runtime.Interface;

namespace Runtime.Signal
{
    public readonly struct SpawnMergedDefenderSignal
    {
        public readonly IDefender MergingDefender;
        public readonly IDefender MergedDefender;
        
        public SpawnMergedDefenderSignal(IDefender mergingDefender, IDefender mergedDefender)
        {
            MergingDefender = mergingDefender;
            MergedDefender = mergedDefender;
        }
    }
}