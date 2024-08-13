using System;
using System.Collections.Generic;
using AudioSystem;
using Runtime.DefenderSystem.Model;
using Runtime.Enum;
using Runtime.Interface;
using Runtime.Signal;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.DefenderSystem.View
{
    public class DefenderViewModel : MonoBehaviour, IDefender      
    {
        #region View

        [Header("View")]
        public SpriteRenderer SpriteRenderer;
        
        public TextMeshPro LevelText;
        public TextMeshPro LevelUpgradeText;

        #endregion
        
        #region Model

        [Header("Model")]
        [SerializeField]
        private DefenderSo data;
        
        [HideInInspector]
        public int Damage;
        [HideInInspector]
        public int SpriteRendererDefaultSortingOrder;
        [HideInInspector]
        public int SpriteRendererSelectedSortingOrder;
        [HideInInspector]
        public int LevelTextDefaultSortingOrder;
        [HideInInspector]
        public int LevelTextSelectedSortingOrder;
        
        [HideInInspector]
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
        
        public GameObject UpgradeParticleGameObject;
        public GameObject BulletPrefab;
        public GameObject BulletHitParticlePrefab;
        public GameObject DamagePopupPrefab;
        
        public SoundData AttackSoundData;
        
        public DefenderType DefenderType => data.DefenderType;
        
        public Transform Transform => transform;
        
        public Vector3 InitialPosition { get; set; }
        
        public int Level { get; set; } = 1;

        #endregion

        #region Internal Events
        
        public event Action OnMouseDownEvent;
        public event Action OnMouseUpEvent;
        public event Action OnEnableEvent;
        public event Action OnDisableEvent;
        
        public Action SetDefaultLevelEvent { get; set; }
        public Action SetDefaultColorEvent { get; set; }
        public Action SetUnMergeableColorEvent { get; set; }
        
        public Action<int> LevelUpEvent { get; set; }
        public Action<int> UpgradeDefenderEvent { get; set; }
        public Action<int> UpgradeNewDefenderEvent { get; set; }
        
        #endregion
        
        #region Variables
        
        private SignalBus _signalBus;
        
        #endregion
        
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
            _signalBus.Fire(new UpdateNewDefenderUpgradeSignal(this));
            _signalBus.Fire(new SetTargetForNewDefenderSignal());
            OnEnableEvent?.Invoke();
            SetInitialPosition();
        }

        private void SetInitialPosition()
        {
            InitialPosition = transform.position;
        }

        private void OnMouseDown()
        {
            OnMouseDownEvent?.Invoke();
            _signalBus.Fire(new ShowMergeableDefendersSignal(this));
        }
        
        private void OnMouseUp()
        {
            OnMouseUpEvent?.Invoke();
            _signalBus.Fire(new MergeDefendersSignal(this));
        }

        private void SetDefaultData()
        {
            Damage = data.Damage;
            LevelTextDefaultSortingOrder = data.LevelTextDefaultSortingOrder;
            LevelTextSelectedSortingOrder = data.LevelTextDefaultSortingOrder + 2;
            SpriteRendererDefaultSortingOrder = data.SpriteRendererDefaultSortingOrder;
            SpriteRendererSelectedSortingOrder = data.LevelTextDefaultSortingOrder + 1;
            AttackInterval = data.AttackInterval;
            UnMergeableColor = data.UnMergeableColor;
            DefaultColor = data.DefaultColor;
            BulletMoveDuration = data.BulletMoveDuration;
            DefaultScale = data.DefaultScale;
            LevelUpDamageIncrease = data.LevelUpDamageIncrease;
            UpgradeDamageIncrease = data.UpgradeDamageIncrease;
            LevelUpAttackIntervalReduction = data.LevelUpAttackIntervalReduction;
            UpgradeAttackIntervalReduction = data.UpgradeAttackIntervalReduction;
        }

        private void Reset()
        {
            Damage = data.Damage;
            AttackInterval = data.AttackInterval;
            transform.localScale = Vector3.zero;
        }
        
        private void OnDisable()
        {
            _signalBus.Fire(new RemoveDefenderFromListSignal(this));
            OnDisableEvent?.Invoke();
            Reset();
        }
    }
}