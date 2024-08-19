using System.Collections.Generic;
using AudioSystem;
using Runtime.Enum;
using UnityEngine;

namespace Runtime.DefenderSystem.Model
{
    [CreateAssetMenu(fileName = "DefenderData", menuName = "Scriptable Object/DefenderData", order = 0)]
    public class DefenderSo : ScriptableObject
    {
        public DefenderType DefenderType;
        
        public int MaxLevel;
        public int Damage;
        public int SpriteRendererDefaultSortingOrder;
        public int LevelTextDefaultSortingOrder;
        
        public float AttackInterval;
        public float BulletMoveDuration;
        public float DefaultScale;
        
        public Color UnMergeableColor;
        public Color DefaultColor;
        
        public List<int> LevelUpDamageIncrease;
        public List<int> UpgradeDamageIncrease;
        public List<float> LevelUpAttackIntervalReduction;
        public List<float> UpgradeAttackIntervalReduction;
        
        public GameObject BulletPrefab;
        public GameObject BulletHitParticlePrefab;
        public GameObject DamagePopupPrefab;
        
        public SoundData AttackSoundData;
    }
}