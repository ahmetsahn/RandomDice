using System;
using Runtime.DefenderSystem.View;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.DefenderSystem.Controller
{
    public class DefenderUpgradeController : IDisposable
    {
        private readonly DefenderViewModel _viewModel;
        
        private const int MAX_LEVEL = 6;
        
        public DefenderUpgradeController(DefenderViewModel viewModel)
        {
            _viewModel = viewModel;
            
            SubscribeEvents();
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
            if(_viewModel.Level == MAX_LEVEL)
            {
                _viewModel.LevelText.fontSize = 15;
                _viewModel.LevelText.text = "MAX";
            }

            else
            {
                _viewModel.LevelText.text = _viewModel.Level.ToString();
            }
            _viewModel.Damage += _viewModel.LevelUpDamageIncrease[level-1];
            _viewModel.AttackInterval -= _viewModel.LevelUpAttackIntervalReduction[level-1];
        }
        
        private void OnSetDefaultLevel()
        {
            _viewModel.Level = 1;
            _viewModel.LevelText.text = _viewModel.Level.ToString();
        }
        
        private void OnUpgradeDefender(int upgradeLevel)
        {
            _viewModel.Damage += _viewModel.UpgradeDamageIncrease[upgradeLevel-2];
            _viewModel.AttackInterval -= _viewModel.UpgradeAttackIntervalReduction[upgradeLevel-2];
        }
        
        private void OnUpgradeNewDefender(int upgradeLevel)
        {
            _viewModel.Damage += _viewModel.UpgradeDamageIncrease[upgradeLevel-2];
            _viewModel.AttackInterval -= _viewModel.UpgradeAttackIntervalReduction[upgradeLevel-2];
        }
        
        private void UnsubscribeEvents()
        {
            _viewModel.LevelUpEvent -= OnLevelUp;
            _viewModel.SetDefaultLevelEvent -= OnSetDefaultLevel;
            _viewModel.UpgradeDefenderEvent -= OnUpgradeDefender;
            _viewModel.UpgradeNewDefenderEvent -= OnUpgradeNewDefender;
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
}