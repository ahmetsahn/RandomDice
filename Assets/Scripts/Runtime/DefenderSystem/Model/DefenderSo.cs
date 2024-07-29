using Runtime.Enum;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.DefenderSystem.Model
{
    [CreateAssetMenu(fileName = "DefenderData", menuName = "Scriptable Object/DefenderData", order = 0)]
    public class DefenderSo : ScriptableObject
    {
        public DefenderType DefenderType;
        
        public int Damage;
        public int SpriteRendererDefaultSortingOrder;
        public int LevelTextDefaultSortingOrder;
        
        public float AttackInterval;
        public float BulletMoveDuration;
        public float IntervalReductionAmount;
        public float DefaultScale;
        
        
        public Color UnMergeableColor;
        public Color DefaultColor;
    }
}