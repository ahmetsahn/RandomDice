namespace Runtime.Signal
{
    public readonly struct ResumeTimerSignal
    {
        public readonly float WaveTime;
        
        public ResumeTimerSignal(float waveTime)
        {
            WaveTime = waveTime;
        }
    }
}