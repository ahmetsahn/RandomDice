namespace Runtime.Signal
{
    public readonly struct EnableBinSpriteSignal
    {
        public readonly bool IsEnable;
        
        public EnableBinSpriteSignal(bool isEnable)
        {
            IsEnable = isEnable;
        }
    }
}