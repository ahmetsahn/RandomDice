using System.Collections.Generic;
using AudioSystem;
using DG.Tweening;
using Runtime.Core.Pool;
using Runtime.Interface;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.DefenderSystem.Manager
{
    public class DefendersManager : MonoBehaviour
    {
        [SerializeField]
        private SoundData mergeSoundData;
        [SerializeField]
        private SoundData trashSoundData;
        
        private readonly List<IDefender> _defenderList = new();

        private SignalBus _signalBus;

        private const int MAX_LEVEL = 6;
        
        [SerializeField]
        private GameObject trashSprite;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _signalBus.Subscribe<AddDefenderToListSignal>(AddDefenderToList);
            _signalBus.Subscribe<RemoveDefenderFromListSignal>(RemoveDefenderFromList);
            _signalBus.Subscribe<ShowMergeableDefendersSignal>(ShowMergeableDefenders);
            _signalBus.Subscribe<MergeDefendersSignal>(MergeDefenders);
            _signalBus.Subscribe<UpgradeDefenderSignal>(UpgradeDefender);
        }
        

        private void AddDefenderToList(AddDefenderToListSignal signal)
        {
            _defenderList.Add(signal.Defender);
        }
        
        private void RemoveDefenderFromList(RemoveDefenderFromListSignal signal)
        {
            _defenderList.Remove(signal.Defender);
        }
        
        private void ShowMergeableDefenders(ShowMergeableDefendersSignal signal)
        {
            trashSprite.SetActive(true);
            
            foreach (IDefender defender in _defenderList)
            {
                if (defender.DefenderType != signal.Defender.DefenderType 
                    || defender.Level != signal.Defender.Level 
                    || defender.Level == MAX_LEVEL
                    && defender != signal.Defender)
                {
                    defender.SetUnMergeableColorEvent?.Invoke();
                }
            }
        }

        private void MergeDefenders(MergeDefendersSignal signal)
        {
            trashSprite.SetActive(false);
            float trashDistance = Vector3.Distance(trashSprite.transform.position, signal.Defender.Transform.position);
            
            if (trashDistance < 0.5f)
            {
                ObjectPoolManager.ReturnObjectToPool(signal.Defender.Transform.gameObject);
                SoundManager.Instance.CreateSoundBuilder().Play(trashSoundData);
                SetDefaultColor();
                _signalBus.Fire(new AddToEmptyDefenderSpawnSlotListSignal(signal.Defender.InitialPosition));
                _signalBus.Fire(new UpdateSpawnDefenderButtonStateSignal());
                return;
            }
            
            foreach (IDefender defender in _defenderList)
            {
                if (defender.DefenderType == signal.Defender.DefenderType 
                    && defender.Level == signal.Defender.Level 
                    && defender != signal.Defender 
                    && defender.Level < MAX_LEVEL 
                    && signal.Defender.Level < MAX_LEVEL)
                {
                    float distance = Vector3.Distance(defender.Transform.position, signal.Defender.Transform.position);
                    if (distance < 0.5f)
                    {
                        _signalBus.Fire(new SpawnMergedDefenderSignal(signal.Defender, defender));
                        ObjectPoolManager.ReturnObjectToPool(defender.Transform.gameObject);
                        ObjectPoolManager.ReturnObjectToPool(signal.Defender.Transform.gameObject);
                        SetDefaultColor();
                        SoundManager.Instance.CreateSoundBuilder().Play(mergeSoundData);
                        _signalBus.Fire(new UpdateSpawnDefenderButtonStateSignal());
                        return;
                    }
                }
            }
            
            SetDefaultColor();
            signal.Defender.Transform.DOMove(signal.Defender.InitialPosition, 0.25f);
        }
        
        private void SetDefaultColor()
        {
            foreach (IDefender defender in _defenderList)
            {
                defender.SetDefaultColorEvent?.Invoke();
            }
        }
        
        private void UpgradeDefender(UpgradeDefenderSignal signal)
        {
            foreach (IDefender defender in _defenderList)
            {
                if (defender.DefenderType == signal.DefenderType)
                {
                    defender.UpgradeDefenderEvent?.Invoke(signal.UpgradeLevel);
                }
            }
        }

        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<AddDefenderToListSignal>(AddDefenderToList);
            _signalBus.Unsubscribe<RemoveDefenderFromListSignal>(RemoveDefenderFromList);
            _signalBus.Unsubscribe<ShowMergeableDefendersSignal>(ShowMergeableDefenders);
            _signalBus.Unsubscribe<MergeDefendersSignal>(MergeDefenders);
            _signalBus.Unsubscribe<UpgradeDefenderSignal>(UpgradeDefender);
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}