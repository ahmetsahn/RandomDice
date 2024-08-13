using AudioSystem;
using Runtime.Signal;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        private Button speedUpButton;
        [SerializeField]
        private Button restartButton;
        
        [SerializeField]
        private Image speedUpButtonImage;
        
        [SerializeField]
        private TextMeshProUGUI currentEnergyText;
        [SerializeField]
        private TextMeshProUGUI requiredEnergyForSpawnDefenderText;
        [SerializeField] 
        private TextMeshProUGUI waveText;
        
        [SerializeField]
        private GameObject[] healthIcons;
        
        [SerializeField]
        private SoundData buttonClickSoundData;
        
        private int _healthIconCount;
        private int _currentEnergy;
        private int _requiredEnergyForSpawnDefender;
        private int _wave;
        
        private bool _isDefenderSpawnSlotListFull;
        
        private SignalBus _signalBus;
        
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
            _isDefenderSpawnSlotListFull = false;
            _wave = 1;
            waveText.text = "Wave : " + _wave;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            createDefenderButton.onClick.AddListener(CreateDefenderButtonClicked);
            speedUpButton.onClick.AddListener(OnSpeedButtonClicked);
            restartButton.onClick.AddListener(OnRestartButtonClicked);
            _signalBus.Subscribe<DestroyHealthIconSignal>(DestroyHealthIcon);
            _signalBus.Subscribe<IncreaseCurrentEnergySignal>(IncreaseCurrentEnergySignal);
            _signalBus.Subscribe<ReduceCurrentEnergySignal>(ReduceCurrentEnergySignal);
            _signalBus.Subscribe<IsDefenderSpawnSlotListFullSignal>(IsDefenderSpawnSlotListFull);
            _signalBus.Subscribe<UpdateSpawnDefenderButtonStateSignal>(UpdateSpawnDefenderButtonState);
            _signalBus.Subscribe<IncreaseWaveSignal>(IncreaseWave);
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
            UpdateSpawnDefenderButtonState();
            SoundManager.Instance.CreateSoundBuilder().Play(buttonClickSoundData);
        }
        
        private void OnSpeedButtonClicked()
        {
            Time.timeScale = Time.timeScale == 1 ? 2 : 1;
            speedUpButtonImage.color = Time.timeScale == 1 ? Color.red : Color.green;
            SoundManager.Instance.CreateSoundBuilder().Play(buttonClickSoundData);
        }
        
        private void OnRestartButtonClicked()
        {
            RestartLoadScene();
        }
        
        private void UpdateSpawnDefenderButtonState()
        {
            createDefenderButton.interactable = _currentEnergy >= _requiredEnergyForSpawnDefender && !_isDefenderSpawnSlotListFull;
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
            UpdateSpawnDefenderButtonState();
            _signalBus.Fire(new UpdateUpgradeDefenderButtonStateSignal(_currentEnergy));
        }

        private void ReduceCurrentEnergy(int energyAmount)
        {
            _currentEnergy -= energyAmount;
            currentEnergyText.text = _currentEnergy.ToString();
            _signalBus.Fire(new UpdateUpgradeDefenderButtonStateSignal(_currentEnergy));
        }
        
        private void IncreaseCurrentEnergySignal(IncreaseCurrentEnergySignal signal)
        {
            IncreaseCurrentEnergy(signal.EnergyAmount);
        }
        
        private void ReduceCurrentEnergySignal(ReduceCurrentEnergySignal signal)
        {
            ReduceCurrentEnergy(signal.EnergyAmount);
            UpdateSpawnDefenderButtonState();
        }
        
        private void DestroyHealthIcon(DestroyHealthIconSignal signal)
        {
            _healthIconCount--;
            healthIcons[_healthIconCount].SetActive(false);
            
            if (_healthIconCount == 0)
            {
                RestartLoadScene();
            }
        }
        
        private void RestartLoadScene()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
        
        private void IsDefenderSpawnSlotListFull(IsDefenderSpawnSlotListFullSignal signal)
        {
            _isDefenderSpawnSlotListFull = signal.IsFull;
        }
        
        private void IncreaseWave(IncreaseWaveSignal signal)
        {
            _wave++;
            waveText.text = "Wave : " + _wave;
        }
        
        private void UnsubscribeEvents()
        {
            startButton.onClick.RemoveListener(OnStartButtonClicked);
            createDefenderButton.onClick.RemoveListener(CreateDefenderButtonClicked);
            speedUpButton.onClick.RemoveListener(OnSpeedButtonClicked);
            restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _signalBus.Unsubscribe<DestroyHealthIconSignal>(DestroyHealthIcon);
            _signalBus.Unsubscribe<IncreaseCurrentEnergySignal>(IncreaseCurrentEnergySignal);
            _signalBus.Unsubscribe<ReduceCurrentEnergySignal>(ReduceCurrentEnergySignal);
            _signalBus.Unsubscribe<IsDefenderSpawnSlotListFullSignal>(IsDefenderSpawnSlotListFull);
            _signalBus.Unsubscribe<UpdateSpawnDefenderButtonStateSignal>(UpdateSpawnDefenderButtonState);
            _signalBus.Unsubscribe<IncreaseWaveSignal>(IncreaseWave);
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}