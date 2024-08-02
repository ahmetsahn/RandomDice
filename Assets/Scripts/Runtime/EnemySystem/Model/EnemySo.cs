using Runtime.Enum;
using UnityEngine;

namespace Runtime.EnemySystem.Model
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Object/EnemyData", order = 0)]
    public class EnemySo : ScriptableObject
    {
        public int MaxHealth;
        public int EnergyValue;
        
        public float Speed;
        public float DefaultScale;
        
        public EnemyType EnemyType;
    }
}