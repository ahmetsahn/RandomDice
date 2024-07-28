using Runtime.Interface;

namespace Runtime.Signal
{
    public readonly struct RemoveEnemyFromListSignal
    {
        public readonly IEnemy Enemy;
        
        public RemoveEnemyFromListSignal(IEnemy enemy)
        {
            Enemy = enemy;
        }
    }
}