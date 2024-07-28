using Runtime.Interface;

namespace Runtime.Signal
{
    public readonly struct StartDefenderAttackSignal
    {
        public readonly IEnemy Enemy;
        
        public StartDefenderAttackSignal(IEnemy enemy)
        {
            Enemy = enemy;
        }
    }
}