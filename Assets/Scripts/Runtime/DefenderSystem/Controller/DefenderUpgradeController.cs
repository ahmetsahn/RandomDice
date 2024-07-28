using Runtime.DefenderSystem.View;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.DefenderSystem.Controller
{
    public class DefenderUpgradeController : MonoBehaviour
    {
        private DefenderViewModel _viewModel;
        
        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(DefenderViewModel viewModel, SignalBus signalBus)
        {
            _viewModel = viewModel;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            SubscribeEvents();
            _signalBus.Fire(new UpdateNewDefenderUpgradeSignal(_viewModel));
        }
        
        private void SubscribeEvents()
        {
            _viewModel.LevelUpEvent += OnLevelUp;
            _viewModel.SetDefaultLevelEvent += OnSetDefaultLevel;
            _viewModel.UpgradeDefenderEvent += OnUpgradeDefender;
            _viewModel.UpgradeNewDefenderEvent += OnUpgradeNewDefender;
        }
        
        private void OnLevelUp(int level)
        {
            _viewModel.Level = level + 1;
            _viewModel.LevelText.text = _viewModel.Level.ToString();
            _viewModel.Damage += _viewModel.Level;
            _viewModel.AttackInterval -= _viewModel.IntervalReductionAmount;
        }
        
        private void OnSetDefaultLevel()
        {
            _viewModel.Level = 1;
            _viewModel.LevelText.text = _viewModel.Level.ToString();
        }
        
        private void OnUpgradeDefender(int upgradeLevel)
        {
            _viewModel.Damage += upgradeLevel;
            _viewModel.AttackInterval -= _viewModel.IntervalReductionAmount;
        }
        
        private void OnUpgradeNewDefender(int upgradeLevel)
        {
            int cumulativeSum = 0;
            for (int i = 2; i <= upgradeLevel; i++)
            {
                cumulativeSum += i;
            }
            
            _viewModel.Damage += cumulativeSum;
            _viewModel.AttackInterval -= _viewModel.IntervalReductionAmount;
        }
        
        private void UnsubscribeEvents()
        {
            _viewModel.LevelUpEvent -= OnLevelUp;
            _viewModel.SetDefaultLevelEvent -= OnSetDefaultLevel;
            _viewModel.UpgradeDefenderEvent -= OnUpgradeDefender;
            _viewModel.UpgradeNewDefenderEvent -= OnUpgradeNewDefender;
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}