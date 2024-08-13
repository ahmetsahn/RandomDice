using System;
using Runtime.EnemySystem.Model;
using Runtime.Enum;
using Runtime.Interface;
using Runtime.Signal;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.EnemySystem.View
{
    public class EnemyViewModel : MonoBehaviour, IEnemy
    {
        #region View

        [Header("View")]
        public TextMeshPro HealthText;

        #endregion

        #region Model

        [Header("Model")]
        [SerializeField]
        private EnemySo data;
        
        [HideInInspector]
        public float Speed;
        [HideInInspector]
        public float DefaultScale;
        
        [HideInInspector]
        public int EnergyValue;
        
        public int Health { get; set; }
        
        public Transform Transform => transform;
        
        public EnemyType EnemyType { get; set; }

        #endregion

        #region InternalEvents

        public event Action OnEnableEvent;
        public event Action OnDisableEvent;
        
        public Action<int> TakeDamageEvent { get; set; }

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
            _signalBus.Fire(new AddEnemyToListSignal(this));
            OnEnableEvent?.Invoke();
        }

        private void SetDefaultData()
        {
            Health = data.MaxHealth;
            Speed = data.Speed;
            DefaultScale = data.DefaultScale;
            EnergyValue = data.EnergyValue;
            HealthText.text = Health.ToString();
            EnemyType = data.EnemyType;
        }

        private void Reset()
        {
            Health = data.MaxHealth;
            Speed = data.Speed;
            HealthText.text = Health.ToString();
        }

        private void OnDisable()
        {
            _signalBus.Fire(new RemoveEnemyFromListSignal(this));
            OnDisableEvent?.Invoke();
            Reset();
        }
    }
}