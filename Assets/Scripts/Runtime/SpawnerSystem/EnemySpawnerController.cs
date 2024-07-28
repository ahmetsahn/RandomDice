using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Core.Pool;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.SpawnerSystem
{
    public class EnemySpawnerController : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        [SerializeField]
        private List<WaveData> waveDataList;
        
        private int _currentWaveIndex;
        
        [Inject]
        private void Construct(
            SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _signalBus.Subscribe<StartSpawnEnemySignal>(StartSpawnEnemySignal);
        }
        
        private void StartSpawnEnemySignal(StartSpawnEnemySignal signal)
        {
            StartWavesWithAsync();
        }

        private async void StartWavesWithAsync()
        {
            foreach (WaveData waveData in waveDataList)
            {
                await SpawnEnemyWithAsync(waveData);
                await UniTask.Delay(TimeSpan.FromSeconds(waveData.WaitingTimeAfterWaveEnds));
            }
        }

        private async UniTask SpawnEnemyWithAsync(WaveData waveData)
        {
            for (int i = 0; i < waveData.Enemies.Count; i++)
            {
                int spawnCount = waveData.SpawnCount[i];
                float spawnInterval = waveData.SpawnInterval[i];

                for (int j = 0; j < spawnCount; j++)
                {
                    ObjectPoolManager.SpawnObjectWithZenject(waveData.Enemies[i].gameObject, transform.position, Quaternion.identity);
                    await UniTask.Delay(TimeSpan.FromSeconds(spawnInterval));
                }

                if (i + 1 >= waveData.Enemies.Count || waveData.WaitingTimeBetweenEnemyTypes == null) continue;
                
                float extraWaitTime = waveData.WaitingTimeBetweenEnemyTypes[i];
                await UniTask.Delay(TimeSpan.FromSeconds(extraWaitTime));
            }

            await UniTask.Delay(TimeSpan.FromSeconds(waveData.WaitingTimeAfterWaveEnds));
        }

        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<StartSpawnEnemySignal>(StartSpawnEnemySignal);
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}