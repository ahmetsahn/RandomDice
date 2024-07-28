using Runtime.Interface;

namespace Runtime.Signal
{
    public readonly struct AddEnemyToListSignal
    {
        public readonly IEnemy Enemy;
        
        public AddEnemyToListSignal(IEnemy enemy)
        {
            Enemy = enemy;
        }
    }
}