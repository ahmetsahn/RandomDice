using System;
using AudioSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Core.Pool;
using Runtime.DefenderSystem.View;
using Runtime.Interface;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.DefenderSystem.Controller
{
    public class DefenderAttackController : MonoBehaviour
    {
        private DefenderViewModel _viewModel;
        
        private SignalBus _signalBus;
        
        private bool _isBossSequenceRunning;
        
        private IEnemy _currenEnemy;
        
        [Inject]
        private void Construct(DefenderViewModel viewModel, SignalBus signalBus)
        {
            _viewModel = viewModel;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            SubscribeEvents();
            _signalBus.Fire(new SetTargetForNewDefenderSignal());
            StartDefenderAttack();
        }
        
        private void SubscribeEvents()
        {
            _signalBus.Subscribe<SetTargetSignal>(SetTarget);
        }
        
        private void SetTarget(SetTargetSignal signal)
        {
            if (_currenEnemy == null)
            {
                _currenEnemy = signal.Enemy;
                StartDefenderAttack();
                return;
            }
            
            _currenEnemy = signal.Enemy;
        }

        private void StartDefenderAttack()
        {
            Attack().Forget();
        }
        
        private async UniTask Attack()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
            
            while (_currenEnemy != null)
            {
                GameObject bullet = ObjectPoolManager.SpawnObject(_viewModel.BulletPrefab, _viewModel.transform.position, Quaternion.identity);
                bullet.transform.DOMove(_currenEnemy.Transform.position, _viewModel.BulletMoveDuration).OnComplete(() =>
                {
                    CreateBulletHitParticle(bullet.transform);
                    CreateDamagePopup(bullet.transform, _viewModel.Damage);
                    SoundManager.Instance.CreateSoundBuilder().WithRandomPitch().WithPosition(bullet.transform.position).Play(_viewModel.AttackSoundData);
                    ObjectPoolManager.ReturnObjectToPool(bullet);
                    _currenEnemy?.TakeDamageEvent?.Invoke(_viewModel.Damage);
                });

                await UniTask.Delay(TimeSpan.FromSeconds(_viewModel.AttackInterval));
            }
        }
        
        private void CreateBulletHitParticle(Transform target)
        {
            ObjectPoolManager.SpawnObject(_viewModel.BulletHitParticlePrefab, target.position, Quaternion.identity);
        }
        
        private void CreateDamagePopup(Transform target, int damage)
        {
            Vector3 targetPosition = target.position;
            targetPosition.y += 0.5f;
            ObjectPoolManager.SpawnObjectWithZenject(_viewModel.DamagePopupPrefab, targetPosition, Quaternion.identity);
            _signalBus.Fire(new SetDamagePopupTextSignal(damage));
        }
        
        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<SetTargetSignal>(SetTarget);
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
            _currenEnemy = null;
        }
    }
}