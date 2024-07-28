using AudioSystem;
using Runtime.Signal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.UISystem
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button createDefenderButton;
        
        [SerializeField]
        private TextMeshProUGUI currentEnergyText;
        [SerializeField]
        private TextMeshProUGUI requiredEnergyForSpawnDefenderText;
        
        [SerializeField]
        private GameObject[] healthIcons;
        
        private int _healthIconCount;
        private int _currentEnergy;
        private int _requiredEnergyForSpawnDefender;
        
        private SignalBus _signalBus;
        
        [SerializeField]
        private SoundData buttonClickSoundData;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _healthIconCount = healthIcons.Length;
            _currentEnergy = 100;
            _requiredEnergyForSpawnDefender = 10;
            currentEnergyText.text = _currentEnergy.ToString();
            requiredEnergyForSpawnDefenderText.text = _requiredEnergyForSpawnDefender.ToString();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            createDefenderButton.onClick.AddListener(CreateDefenderButtonClicked);
            _signalBus.Subscribe<DestroyHealthIconSignal>(DestroyHealthIcon);
            _signalBus.Subscribe<IncreaseCurrentEnergySignal>(IncreaseCurrentEnergySignal);
            _signalBus.Subscribe<ReduceCurrentEnergySignal>(ReduceCurrentEnergySignal);
        }
        
        private void OnStartButtonClicked()
        {
            _signalBus.Fire<StartSpawnEnemySignal>();
            startButton.gameObject.SetActive(false);
            SoundManager.Instance.CreateSoundBuilder().Play(buttonClickSoundData);
        }
        
        private void CreateDefenderButtonClicked()
        {
            _signalBus.Fire<SpawnDefenderRandomLocationSignal>();
            ReduceCurrentEnergy(_requiredEnergyForSpawnDefender);
            IncreaseRequiredEnergy(10);
            SoundManager.Instance.CreateSoundBuilder().Play(buttonClickSoundData);
        }
        
        private void UpdateCreateDefenderButtonState()
        {
            createDefenderButton.interactable = _currentEnergy >= _requiredEnergyForSpawnDefender;
        }
        
        private void IncreaseRequiredEnergy(int energyAmount)
        {
            _requiredEnergyForSpawnDefender += energyAmount;
            requiredEnergyForSpawnDefenderText.text = _requiredEnergyForSpawnDefender.ToString();
        }
        
        private void IncreaseCurrentEnergy(int energyAmount)
        {
            _currentEnergy += energyAmount;
            currentEnergyText.text = _currentEnergy.ToString();
            UpdateCreateDefenderButtonState();
            _signalBus.Fire(new UpdateUpgradeDefenderButtonStateSignal(_currentEnergy));
        }

        private void ReduceCurrentEnergy(int energyAmount)
        {
            _currentEnergy -= energyAmount;
            currentEnergyText.text = _currentEnergy.ToString();
            UpdateCreateDefenderButtonState();
            _signalBus.Fire(new UpdateUpgradeDefenderButtonStateSignal(_currentEnergy));
        }
        
        private void IncreaseCurrentEnergySignal(IncreaseCurrentEnergySignal signal)
        {
            IncreaseCurrentEnergy(signal.EnergyAmount);
        }
        
        private void ReduceCurrentEnergySignal(ReduceCurrentEnergySignal signal)
        {
            ReduceCurrentEnergy(signal.EnergyAmount);
        }
        
        private void DestroyHealthIcon(DestroyHealthIconSignal signal)
        {
            _healthIconCount--;
            healthIcons[_healthIconCount].SetActive(false);
            
            if (_healthIconCount == 0)
            {
                Time.timeScale = 0;
            }
        }
        
        private void UnsubscribeEvents()
        {
            startButton.onClick.RemoveListener(OnStartButtonClicked);
            createDefenderButton.onClick.RemoveListener(CreateDefenderButtonClicked);
            _signalBus.Unsubscribe<DestroyHealthIconSignal>(DestroyHealthIcon);
            _signalBus.Unsubscribe<IncreaseCurrentEnergySignal>(IncreaseCurrentEnergySignal);
            _signalBus.Unsubscribe<ReduceCurrentEnergySignal>(ReduceCurrentEnergySignal);
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}