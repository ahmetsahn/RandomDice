namespace Runtime.Signal
{
    public readonly struct SetDamagePopupTextSignal
    {
        public readonly int Damage;
        
        public SetDamagePopupTextSignal(int damage)
        {
            Damage = damage;
        }
    }
}