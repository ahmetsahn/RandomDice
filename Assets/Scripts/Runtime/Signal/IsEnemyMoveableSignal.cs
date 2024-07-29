namespace Runtime.Signal
{
    public readonly struct IsEnemyMoveableSignal
    {
        public readonly bool IsMoveable;
        
        public IsEnemyMoveableSignal(bool isMoveable)
        {
            IsMoveable = isMoveable;
        }
    }
}