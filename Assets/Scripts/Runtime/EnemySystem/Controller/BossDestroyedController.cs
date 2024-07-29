using System;
using Cysharp.Threading.Tasks;
using Runtime.EnemySystem.View;
using Runtime.Interface;
using Runtime.Signal;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.EnemySystem.Controller
{
    public class BossDestroyedController : MonoBehaviour, IBoss
    {
        private EnemyViewModel _viewModel;
        
        private SignalBus _signalBus;
        
        public Transform Transform => transform;
        
        public Action<int> SetHealthEvent { get; set; }
        
        [Inject]
        private void Construct(EnemyViewModel viewModel, SignalBus signalBus)
        {
            _viewModel = viewModel;
            _signalBus = signalBus;
        }
        
        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            SetHealthEvent += SetHealth;
        }
        
        private void SetHealth(int health)
        {
            _viewModel.Health += health;
            _viewModel.HealthText.text = _viewModel.Health.ToString();
        }

        private void UnsubscribeEvents()
        {
            SetHealthEvent -= SetHealth;
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
            _signalBus.Fire(new BossDeadSignal());
        }
    }
}