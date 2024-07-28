using System;
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
        private SoundData buttonClickSoundData;
        
        private Button _button;
        
        private int _upgradeLevel;
        private int _upgradeCost;
        
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
            _upgradeCost = 100;
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
            _signalBus.Fire(new ReduceCurrentEnergySignal(_upgradeCost));
            _upgradeLevel++;
            upgradeLevelText.text = "Lv " + _upgradeLevel;
            _upgradeCost += 100;
            upgradeCostText.text = _upgradeCost.ToString();
            _signalBus.Fire(new UpgradeDefenderSignal(defenderT, _upgradeLevel));
            
            if(_upgradeLevel == 5)
            {
                _button.interactable = false;
            }
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
            _button.interactable = signal.CurrentEnergy >= _upgradeCost;
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