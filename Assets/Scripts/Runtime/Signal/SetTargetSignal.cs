using Runtime.Interface;

namespace Runtime.Signal
{
    public readonly struct SetTargetSignal
    {
        public readonly IEnemy Enemy;
        
        public SetTargetSignal(IEnemy enemy)
        {
            Enemy = enemy;
        }
    }
}