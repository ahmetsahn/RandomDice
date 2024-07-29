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
        
        private bool _bossSequenceIsRunning;
        
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
            _signalBus.Subscribe<SetNewDefenderAttackTargetSignal>(SetNewDefenderAttackTarget);
            _signalBus.Subscribe<BossSequenceSignal>(BossSequence);
        }

        private void AddEnemyToList(IEnemy enemy)
        {
            _enemyList.Add(enemy);
        }
        
        private void RemoveEnemyFromList(IEnemy enemy)
        {
            _enemyList.Remove(enemy);
        }
        
        private void AddEnemyToListSignal(AddEnemyToListSignal signal)
        {
            AddEnemyToList(signal.Enemy);
            
            if(_enemyList.Count == 1)
            {
                _signalBus.Fire(new StartDefenderAttackSignal(signal.Enemy));
            }
        }
        
        private void RemoveEnemyFromListSignal(RemoveEnemyFromListSignal signal)
        {
            RemoveEnemyFromList(signal.Enemy);
            
            if (_enemyList.Count == 0)
            {
                _signalBus.Fire(new StopDefenderAttackSignal());
                return;
            }
            
            if(_enemyList.Count > 0)
            {
                _signalBus.Fire(new StartDefenderAttackSignal(_enemyList[0]));
            }
        }
        
        private void SetNewDefenderAttackTarget(SetNewDefenderAttackTargetSignal signal)
        {
            if (_enemyList.Count <= 0)
            {
                return;
            }

            if (_bossSequenceIsRunning)
            {
                return;
            }
            
            _signalBus.Fire(new StartDefenderAttackSignal(_enemyList[0]));
        }
        
        private async void BossSequence(BossSequenceSignal signal)
        {
            _bossSequenceIsRunning = true;
            _signalBus.Fire(new StopDefenderAttackSignal());
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
                ObjectPoolManager.ReturnObjectToPool(_enemyList[0]?.Transform.gameObject);
            }
            
            _signalBus.Fire(new IsEnemyMoveableSignal(false));
            _signalBus.Fire(new StopDefenderAttackSignal());
            
            bossGo.transform.position = bossFirstTransform.position;
            IBoss boss = bossGo.GetComponent<IBoss>();
            boss.SetHealthEvent?.Invoke(enemiesTotalHealth);
            
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            boss.Transform.DOMove(enemyFirstTransform.position, 1f);
            
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
            _bossSequenceIsRunning = false;
            _signalBus.Fire(new StartDefenderAttackSignal(_enemyList[0]));
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            _signalBus.Fire(new IsEnemyMoveableSignal(true));
        }

        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<AddEnemyToListSignal>(AddEnemyToListSignal);
            _signalBus.Unsubscribe<RemoveEnemyFromListSignal>(RemoveEnemyFromListSignal);
            _signalBus.Unsubscribe<SetNewDefenderAttackTargetSignal>(SetNewDefenderAttackTarget);
            _signalBus.Unsubscribe<BossSequenceSignal>(BossSequence);
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}