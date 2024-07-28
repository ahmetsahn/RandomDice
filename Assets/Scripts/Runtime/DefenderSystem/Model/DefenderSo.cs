using Runtime.Enum;
using UnityEngine;

namespace Runtime.DefenderSystem.Model
{
    [CreateAssetMenu(fileName = "DefenderData", menuName = "Scriptable Object/DefenderData", order = 0)]
    public class DefenderSo : ScriptableObject
    {
        public DefenderType DefenderType;
        
        public int Damage;
        public float AttackInterval;
        public float BulletMoveDuration;
        public float intervalReductionAmount;
        
        public Color UnMergeableColor;
        public Color DefaultColor;
    }
}