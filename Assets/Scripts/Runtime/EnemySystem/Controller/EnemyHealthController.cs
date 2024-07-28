using Runtime.Core.Pool;
using Runtime.EnemySystem.View;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.EnemySystem.Controller
{
    public class EnemyHealthController : MonoBehaviour
    {
        private EnemyViewModel _viewModel;
        
        private SignalBus _signalBus;

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
            _viewModel.OnTakeDamage += TakeDamage;
        }
        
        private void TakeDamage(int damage)
        {
            _viewModel.Health -= damage;
            _viewModel.HealthText.text = _viewModel.Health.ToString();
            
            if (_viewModel.Health <= 0)
            {
                _signalBus.Fire(new IncreaseCurrentEnergySignal(_viewModel.EnergyValue));
                ObjectPoolManager.ReturnObjectToPool(_viewModel.gameObject);
            }
        }
        
        private void UnsubscribeEvents()
        {
            _viewModel.OnTakeDamage -= TakeDamage;
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}