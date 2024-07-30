using System;
using System.Collections.Generic;
using Runtime.DefenderSystem.Model;
using Runtime.Enum;
using Runtime.Interface;
using Runtime.Signal;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Runtime.DefenderSystem.View
{
    public class DefenderViewModel : MonoBehaviour, IDefender      
    {
        [Header("View")]
        public SpriteRenderer SpriteRenderer;
        
        public TextMeshPro LevelText;
        public TextMeshPro LevelUpgradeText;
        
        public GameObject UpgradeParticleGameObject;
        
        [Header("Model")]
        [SerializeField]
        private DefenderSo model;
        
        
        public int Damage;
        [HideInInspector]
        public int SpriteRendererDefaultSortingOrder;
        [HideInInspector]
        public int SpriteRendererSelectedSortingOrder;
        [HideInInspector]
        public int LevelTextDefaultSortingOrder;
        [HideInInspector]
        public int LevelTextSelectedSortingOrder;
        
        
        public float AttackInterval;
        [HideInInspector]
        public float BulletMoveDuration;
        [HideInInspector]
        public float DefaultScale;
        
        [HideInInspector]
        public Color UnMergeableColor;
        [HideInInspector]
        public Color DefaultColor;
        
        [HideInInspector]
        public List<int> LevelUpDamageIncrease;
        [HideInInspector]
        public List<int> UpgradeDamageIncrease;
        [HideInInspector]
        public List<float> LevelUpAttackIntervalReduction;
        [HideInInspector]
        public List<float> UpgradeAttackIntervalReduction;
        
        public DefenderType DefenderType => model.DefenderType;
        
        public Transform Transform => transform;
        
        public Vector3 InitialPosition { get; set; }
        public int Level { get; set; } = 1;

        public Action OnMouseDownEvent;
        public Action OnMouseUpEvent;
        
        public Action SetDefaultLevelEvent { get; set; }
        public Action SetDefaultColorEvent { get; set; }
        public Action SetUnMergeableColorEvent { get; set; }
        
        public Action<int> LevelUpEvent { get; set; }
        public Action<int> UpgradeDefenderEvent { get; set; }
        public Action<int> UpgradeNewDefenderEvent { get; set; }
        
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void Awake()
        {
            SetDefaultData();
        }

        private void OnEnable()
        {
            _signalBus.Fire(new AddDefenderToListSignal(this));
            InitialPosition = transform.position;
        }

        private void SetDefaultData()
        {
            Damage = model.Damage;
            LevelTextDefaultSortingOrder = model.LevelTextDefaultSortingOrder;
            LevelTextSelectedSortingOrder = model.LevelTextDefaultSortingOrder + 2;
            SpriteRendererDefaultSortingOrder = model.SpriteRendererDefaultSortingOrder;
            SpriteRendererSelectedSortingOrder = model.LevelTextDefaultSortingOrder + 1;
            AttackInterval = model.AttackInterval;
            UnMergeableColor = model.UnMergeableColor;
            DefaultColor = model.DefaultColor;
            BulletMoveDuration = model.BulletMoveDuration;
            DefaultScale = model.DefaultScale;
            LevelUpDamageIncrease = model.LevelUpDamageIncrease;
            UpgradeDamageIncrease = model.UpgradeDamageIncrease;
            LevelUpAttackIntervalReduction = model.LevelUpAttackIntervalReduction;
            UpgradeAttackIntervalReduction = model.UpgradeAttackIntervalReduction;
        }

        private void Reset()
        {
            Damage = model.Damage;
            AttackInterval = model.AttackInterval;
            transform.localScale = Vector3.zero;
        }
        
        private void OnDisable()
        {
            _signalBus.Fire(new RemoveDefenderFromListSignal(this));
            Reset();
        }
    }
}