﻿using System.Collections.Generic;
using AudioSystem;
using Runtime.Enum;
using Runtime.Signal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.UISystem
{
    public class DefenderUpgradeButton : MonoBehaviour
    {
        [SerializeField]
        private DefenderType defenderType;

        [SerializeField]
        private TextMeshProUGUI upgradeLevelText;
        [SerializeField]
        private TextMeshProUGUI upgradeCostText;
        
        [SerializeField]
        private Image energyIcon;
        
        [SerializeField]
        private SoundData buttonClickSoundData;
        
        [SerializeField]
        private List<int> upgradeCostList;
        
        private Button _button;
        
        private int _upgradeLevel;
        private int _upgradeCost;
        
        private const int MAX_LEVEL = 5;
        
        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _upgradeLevel = 1;
            _upgradeCost = upgradeCostList[0];
            upgradeLevelText.text = "Lv " + _upgradeLevel;
            upgradeCostText.text = _upgradeCost.ToString();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _button.onClick.AddListener (() => UpgradeDefender(defenderType));
            _button.onClick.AddListener(PlayAudioClip);
            _signalBus.Subscribe<UpdateNewDefenderUpgradeSignal>(UpdateNewDefenderUpgrade);
            _signalBus.Subscribe<UpdateUpgradeDefenderButtonStateSignal>(UpdateUpgradeDefenderButtonState);
        }
        
        private void UpgradeDefender(DefenderType defenderT)
        {
            _upgradeLevel++;
            if(_upgradeLevel == MAX_LEVEL)
            {
                _signalBus.Fire(new ReduceCurrentEnergySignal(_upgradeCost));
                _signalBus.Fire(new UpgradeDefenderSignal(defenderT, _upgradeLevel));
                upgradeLevelText.text = "Max";
                upgradeCostText.text = "";
                energyIcon.gameObject.SetActive(false);
                return;
            }
            
            int reduceEnergy = _upgradeCost;
            _upgradeCost = upgradeCostList[_upgradeLevel - 1];
            _signalBus.Fire(new ReduceCurrentEnergySignal(reduceEnergy));
            _signalBus.Fire(new UpgradeDefenderSignal(defenderT, _upgradeLevel));
            upgradeLevelText.text = "Lv " + _upgradeLevel;
            upgradeCostText.text = _upgradeCost.ToString();
        }
        
        private void PlayAudioClip()
        {
            SoundManager.Instance.CreateSoundBuilder().Play(buttonClickSoundData);
        }
        
        private void UpdateNewDefenderUpgrade(UpdateNewDefenderUpgradeSignal signal)
        {
            if (signal.Defender.DefenderType == defenderType && _upgradeLevel > 1)
            {
                signal.Defender.UpgradeNewDefenderEvent?.Invoke(_upgradeLevel);
            }
        }

        private void UpdateUpgradeDefenderButtonState(UpdateUpgradeDefenderButtonStateSignal signal)
        {
            _button.interactable = signal.CurrentEnergy >= _upgradeCost && _upgradeLevel < MAX_LEVEL;
        }
        
        private void UnsubscribeEvents()
        {
            _button.onClick.RemoveAllListeners();
            _signalBus.Unsubscribe<UpdateNewDefenderUpgradeSignal>(UpdateNewDefenderUpgrade);
            _signalBus.Unsubscribe<UpdateUpgradeDefenderButtonStateSignal>(UpdateUpgradeDefenderButtonState);
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}