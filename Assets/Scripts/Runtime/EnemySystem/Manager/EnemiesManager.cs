using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Core.Pool;
using Runtime.Enum;
using Runtime.Interface;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.EnemySystem.Manager
{
    public class EnemiesManager : MonoBehaviour
    {
        [SerializeField]
        private Transform bossFirstTransform;
        [SerializeField]
        private  Transform enemyFirstTransform;
        
        private readonly List<IEnemy> _enemyList = new();
        
        private SignalBus _signalBus;
        
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
            _signalBus.Subscribe<AddEnemyToListSignal>(AddEnemyToListSignal);
            _signalBus.Subscribe<RemoveEnemyFromListSignal>(RemoveEnemyFromListSignal);
            _signalBus.Subscribe<BossSequenceSignal>(BossSequence);
            _signalBus.Subscribe<SetTargetForNewDefenderSignal>(SetTargetForNewDefender);
        }

        private void AddEnemyToList(IEnemy enemy)
        {
            _enemyList.Add(enemy);
            
            if (_enemyList.Count == 1 && enemy.EnemyType != EnemyType.Boss)
            {
                _signalBus.Fire(new SetTargetSignal(_enemyList[0]));
            }
        }
        
        private void RemoveEnemyFromList(IEnemy enemy)
        {
            _enemyList.Remove(enemy);
            
            if (_enemyList.Count > 0 && _enemyList[^1].EnemyType != EnemyType.Boss)
            {
                _signalBus.Fire(new SetTargetSignal(_enemyList[0]));
            }
            
            if (_enemyList.Count == 0)
            {
                _signalBus.Fire(new SetTargetSignal(null));
            }
        }
        
        private void AddEnemyToListSignal(AddEnemyToListSignal signal)
        {
            AddEnemyToList(signal.Enemy);
        }
        
        private void RemoveEnemyFromListSignal(RemoveEnemyFromListSignal signal)
        {
            RemoveEnemyFromList(signal.Enemy);
        }
        
        private async void BossSequence(BossSequenceSignal signal)
        {
            _signalBus.Fire(new SetTargetSignal(null));
            _signalBus.Fire(new IsEnemyMoveableSignal(false));
            
            int enemiesTotalHealth = 0;
            
            foreach (var enemy in _enemyList)
            {
                enemy.Transform.DOMove(bossFirstTransform.position, 1f);
                enemiesTotalHealth += enemy.Health;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            
            
            GameObject bossGo = ObjectPoolManager.SpawnObjectWithZenject(signal.Boss.Transform.gameObject, Vector3.zero, Quaternion.identity);
            
            while (_enemyList.Count > 1)
            {
                ObjectPoolManager.ReturnObjectToPool(_enemyList[0].Transform.gameObject);
            }
            
            _signalBus.Fire(new IsEnemyMoveableSignal(false));
            
            bossGo.transform.position = bossFirstTransform.position;
            IBoss boss = bossGo.GetComponent<IBoss>();
            boss.SetHealthEvent?.Invoke(enemiesTotalHealth);
            
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            boss.Transform.DOMove(enemyFirstTransform.position, 1f);
            
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
            _signalBus.Fire(new SetTargetSignal(_enemyList[0]));
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            _signalBus.Fire(new IsEnemyMoveableSignal(true));
        }
        
        private void SetTargetForNewDefender(SetTargetForNewDefenderSignal signal)
        {
            if (_enemyList.Count == 0)
            {
                return;
            }
            
            if (_enemyList[0].EnemyType == EnemyType.Boss)
            {
                return;
            }
            
            _signalBus.Fire(new SetTargetSignal(_enemyList[0]));
        }

        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<AddEnemyToListSignal>(AddEnemyToListSignal);
            _signalBus.Unsubscribe<RemoveEnemyFromListSignal>(RemoveEnemyFromListSignal);
            _signalBus.Unsubscribe<BossSequenceSignal>(BossSequence);
            _signalBus.Unsubscribe<SetTargetForNewDefenderSignal>(SetTargetForNewDefender);
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}