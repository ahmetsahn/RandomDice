using System;
using Runtime.EnemySystem.Model;
using Runtime.Interface;
using Runtime.Signal;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.EnemySystem.View
{
    public class EnemyViewModel : MonoBehaviour, IEnemy
    {
        [Header("View")]
        public TextMeshProUGUI HealthText;
        
        [Header("Model")]
        [SerializeField]
        private EnemySo data;
        
        [HideInInspector]
        public float Health;
        [HideInInspector]
        public float Speed;
        [HideInInspector]
        public float DefaultScale;
        
        [HideInInspector]
        public int EnergyValue;

        private SignalBus _signalBus;
        
        public Transform Transform => transform;
        
        public Action<int> OnTakeDamage { get; set; }
        
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
        }

        private void SetDefaultData()
        {
            Health = data.MaxHealth;
            Speed = data.Speed;
            DefaultScale = data.DefaultScale;
            EnergyValue = data.EnergyValue;
            HealthText.text = Health.ToString();
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
            Reset();
        }
    }
}