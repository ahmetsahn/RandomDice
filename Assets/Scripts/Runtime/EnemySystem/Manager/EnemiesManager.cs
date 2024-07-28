using System.Collections.Generic;
using Runtime.Interface;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.EnemySystem.Manager
{
    public class EnemiesManager : MonoBehaviour
    {
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
            _signalBus.Subscribe<AddEnemyToListSignal>(AddEnemyToList);
            _signalBus.Subscribe<RemoveEnemyFromListSignal>(RemoveEnemyFromList);
            _signalBus.Subscribe<SetNewDefenderAttackTargetSignal>(SetNewDefenderAttackTarget);
        }
        
        private void AddEnemyToList(AddEnemyToListSignal signal)
        {
            _enemyList.Add(signal.Enemy);
            
            if(_enemyList.Count == 1)
            {
                _signalBus.Fire(new StartDefenderAttackSignal(signal.Enemy));
            }
        }
        
        private void RemoveEnemyFromList(RemoveEnemyFromListSignal signal)
        {
            _enemyList.Remove(signal.Enemy);
            
            if (_enemyList.Count == 0)
            {
                _signalBus.Fire(new EnemyListEmptySignal());
            }

            else
            {
                _signalBus.Fire(new StartDefenderAttackSignal(_enemyList[0]));
            }
        }
        
        private void SetNewDefenderAttackTarget(SetNewDefenderAttackTargetSignal signal)
        {
            if (_enemyList.Count > 0)
            {
                _signalBus.Fire(new StartDefenderAttackSignal(_enemyList[0]));
            }
        }

        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<AddEnemyToListSignal>(AddEnemyToList);
            _signalBus.Unsubscribe<RemoveEnemyFromListSignal>(RemoveEnemyFromList);
            _signalBus.Unsubscribe<SetNewDefenderAttackTargetSignal>(SetNewDefenderAttackTarget);
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}