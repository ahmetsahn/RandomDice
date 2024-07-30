using System.Collections.Generic;
using Runtime.Core.Pool;
using Runtime.DefenderSystem.View;
using Runtime.Interface;
using Runtime.Signal;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Runtime.SpawnerSystem
{
    public class DefenderSpawnerController : MonoBehaviour
    {
        [SerializeField]
        private Transform emptyDefenderSpawnSlotParent;
        
        [SerializeField]
        private DefenderViewModel[] defenderPrefabArray; 
        
        [SerializeField]
        private int[] weights;
        
        private List<Vector3> _emptyDefenderSpawnSlotList;
        
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _emptyDefenderSpawnSlotList = new List<Vector3>();
            foreach (Transform child in emptyDefenderSpawnSlotParent)
            {
                _emptyDefenderSpawnSlotList.Add(child.position);
            }
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _signalBus.Subscribe<SpawnDefenderRandomLocationSignal>(SpawnDefenderRandomLocation);
            _signalBus.Subscribe<SpawnMergedDefenderSignal>(SpawnMergedDefender);
            _signalBus.Subscribe<AddToEmptyDefenderSpawnSlotListSignal>(AddToEmptyDefenderSpawnSlotListSignal);
        }
        
        private void AddToEmptyDefenderSpawnSlotListSignal(AddToEmptyDefenderSpawnSlotListSignal signal)
        {
            AddToEmptyDefenderSpawnSlotList(signal.EmptyDefenderSpawnSlot);
        }
        
        private void AddToEmptyDefenderSpawnSlotList(Vector3 position)
        {
            _emptyDefenderSpawnSlotList.Add(position);
            
            if(_emptyDefenderSpawnSlotList.Count == 1)
            {
                _signalBus.Fire(new IsDefenderSpawnSlotListFullSignal(false));
            }
        }
        
        private void RemoveFromEmptyDefenderSpawnSlotList(int index)
        {
            _emptyDefenderSpawnSlotList.RemoveAt(index);
            
            if(_emptyDefenderSpawnSlotList.Count == 0)
            {
                _signalBus.Fire(new IsDefenderSpawnSlotListFullSignal(true));
            }
        }

        private void SpawnDefenderRandomLocation()
        {
            if (_emptyDefenderSpawnSlotList.Count == 0)
            { 
                return;
            }
            
            int randomIndex = Random.Range(0, _emptyDefenderSpawnSlotList.Count);
            Vector3 randomSpawnPoint = _emptyDefenderSpawnSlotList[randomIndex];
            RemoveFromEmptyDefenderSpawnSlotList(randomIndex);
            IDefender defender = SpawnRandomDefender(randomSpawnPoint);
            defender.SetDefaultLevelEvent?.Invoke();
            
        }

        private IDefender SpawnRandomDefender(Vector3 spawnPosition)
        {
            GameObject defenderGo = ObjectPoolManager.SpawnObjectWithZenject(defenderPrefabArray[GetRandomIndex()].gameObject, spawnPosition, Quaternion.identity);
            IDefender defender = defenderGo.GetComponent<IDefender>();
            return defender;
        }
        
        private int GetRandomIndex()
        {
            int totalWeight = 0;
            foreach (int weight in weights)
            {
                totalWeight += weight;
            }
            
            int randomValue = Random.Range(0, totalWeight);
            int cumulativeWeight = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                cumulativeWeight += weights[i];
                if (randomValue < cumulativeWeight)
                {
                    return i;
                }
            }
            
            return -1;
        }

        private void SpawnMergedDefender(SpawnMergedDefenderSignal signal)
        {
            IDefender defender = SpawnRandomDefender(signal.MergedDefender.InitialPosition);
            defender.LevelUpEvent?.Invoke(signal.MergingDefender.Level);
            AddToEmptyDefenderSpawnSlotList(signal.MergingDefender.InitialPosition);
        }
        
        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<SpawnDefenderRandomLocationSignal>(SpawnDefenderRandomLocation);
            _signalBus.Unsubscribe<SpawnMergedDefenderSignal>(SpawnMergedDefender);
            _signalBus.Unsubscribe<AddToEmptyDefenderSpawnSlotListSignal>(AddToEmptyDefenderSpawnSlotListSignal);
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}