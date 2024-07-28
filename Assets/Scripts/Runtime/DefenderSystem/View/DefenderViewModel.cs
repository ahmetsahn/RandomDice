using System;
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
        [Header("View")]
        public SpriteRenderer SpriteRenderer;
        
        public TextMeshProUGUI LevelText;
        
        [Header("Model")]
        [SerializeField]
        private DefenderSo model;
        
        [HideInInspector]
        public int Damage;
        [HideInInspector]
        public float AttackInterval;
        [HideInInspector]
        public float BulletMoveDuration;
        [HideInInspector]
        public float IntervalReductionAmount;
        [HideInInspector]
        public float DefaultScale;
        
        [HideInInspector]
        public Color UnMergeableColor;
        [HideInInspector]
        public Color DefaultColor;
        
        public DefenderType DefenderType => model.DefenderType;
        
        public Transform Transform => transform;
        
        public Vector3 InitialPosition { get; set; }
        public int Level { get; set; } = 1;
        
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
            AttackInterval = model.AttackInterval;
            UnMergeableColor = model.UnMergeableColor;
            DefaultColor = model.DefaultColor;
            BulletMoveDuration = model.BulletMoveDuration;
            IntervalReductionAmount = model.IntervalReductionAmount;
            DefaultScale = model.DefaultScale;
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