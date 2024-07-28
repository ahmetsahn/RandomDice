using UnityEngine;

namespace Runtime.EnemySystem.Model
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Object/EnemyData", order = 0)]
    public class EnemySo : ScriptableObject
    {
        public float MaxHealth;
        public float Speed;
        public float Scale;
        
        public int EnergyValue;
    }
}