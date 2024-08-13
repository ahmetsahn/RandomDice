using System;
using Runtime.Core.Pool;
using Runtime.EnemySystem.View;
using Runtime.Signal;
using Zenject;

namespace Runtime.EnemySystem.Controller
{
    public class EnemyHealthController : IDisposable
    {
        private readonly EnemyViewModel _viewModel;
        
        private readonly SignalBus _signalBus;

        [Inject]
        public EnemyHealthController(EnemyViewModel viewModel, SignalBus signalBus)
        {
            _viewModel = viewModel;
            _signalBus = signalBus;
            
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _viewModel.TakeDamageEvent += TakeDamage;
        }
        
        private void TakeDamage(int damage)
        {
            _viewModel.Health -= damage;
            _viewModel.HealthText.text = _viewModel.Health.ToString();

            if (_viewModel.Health > 0)
            {
                return;
            }
            
            _signalBus.Fire(new IncreaseCurrentEnergySignal(_viewModel.EnergyValue));
            ObjectPoolManager.ReturnObjectToPool(_viewModel.gameObject);
        }
        
        private void UnsubscribeEvents()
        {
            _viewModel.TakeDamageEvent -= TakeDamage;
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
}