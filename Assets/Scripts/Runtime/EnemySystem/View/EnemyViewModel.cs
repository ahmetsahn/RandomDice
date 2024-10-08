﻿using System;
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
        [Header("View")]
        public TextMeshPro HealthText;
        
        [Header("Model")]
        [SerializeField]
        private EnemySo data;
        
        [HideInInspector]
        public float Speed;
        [HideInInspector]
        public float DefaultScale;
        
        [HideInInspector]
        public int EnergyValue;

        private SignalBus _signalBus;
        
        public int Health { get; set; }
        public Transform Transform => transform;
        
        public Action<int> TakeDamageEvent { get; set; }
        
        public EnemyType EnemyType { get; set; }
        
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
            Reset();
        }
    }
}