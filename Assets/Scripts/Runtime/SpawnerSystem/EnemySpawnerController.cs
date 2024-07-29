using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        private Transform enemyFirstSpawnTransform;
        
        [SerializeField] 
        private Transform bossSpawnTransform;
        
        [SerializeField]
        private List<WaveData> waveDataList;
        
        private int _currentWaveIndex;
        
        private bool _bossIsDead;
        
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
            _signalBus.Subscribe<BossDeadSignal>(BossDead);
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
                await UniTask.Delay(TimeSpan.FromSeconds(waveData.WaitingTimeBossSpawn));
                _signalBus.Fire(new BossSequenceSignal(waveData.Boss));
                await new WaitUntil(() => _bossIsDead);
                await UniTask.Delay(TimeSpan.FromSeconds(waveData.WaitingTimeAfterWaveEnds));
                _signalBus.Fire(new IncreaseWaveSignal());
                _bossIsDead = false;
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
        }
        
        private void BossDead(BossDeadSignal signal)
        {
            _bossIsDead = true;
        }

        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<StartSpawnEnemySignal>(StartSpawnEnemySignal);
            _signalBus.Unsubscribe<BossDeadSignal>(BossDead);
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}