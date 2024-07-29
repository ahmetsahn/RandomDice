using System;
using System.Threading;
using AudioSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Core.Pool;
using Runtime.DefenderSystem.View;
using Runtime.Misc;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.DefenderSystem.Controller
{
    public class DefenderAttackController : MonoBehaviour
    {
        private DefenderViewModel _viewModel;
     
        [SerializeField]
        private GameObject bulletPrefab;
        
        [SerializeField]
        private GameObject bulletHitParticlePrefab;
        
        [SerializeField]
        private GameObject damagePopupPrefab;
        
        private CancellationTokenSource _attackCancellationTokenSource = new();
        
        private SignalBus _signalBus;
        
        [SerializeField]
        private SoundData attackSoundData;
        
        private bool _isBossSequenceRunning;
        
        [Inject]
        private void Construct(DefenderViewModel viewModel, SignalBus signalBus)
        {
            _viewModel = viewModel;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            SubscribeEvents();
            _signalBus.Fire(new SetNewDefenderAttackTargetSignal());
        }
        
        private void SubscribeEvents()
        {
            _signalBus.Subscribe<StopDefenderAttackSignal>(StopAttack);
            _signalBus.Subscribe<StartDefenderAttackSignal>(StartDefenderAttack);
            _signalBus.Subscribe<BossSequenceSignal>(StopAttack);
        }

        private void StartDefenderAttack(StartDefenderAttackSignal signal)
        {
            _attackCancellationTokenSource?.Cancel();
            _attackCancellationTokenSource = new CancellationTokenSource();
            Attack(signal, _attackCancellationTokenSource.Token).Forget();
        }

        private async UniTask Attack(StartDefenderAttackSignal signal, CancellationToken attackCancellationTokenSource)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_viewModel.AttackInterval), cancellationToken: attackCancellationTokenSource);
            
            while (!attackCancellationTokenSource.IsCancellationRequested)
            {
                GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, _viewModel.transform.position, Quaternion.identity);
                bullet.transform.DOMove(signal.Enemy.Transform.position, _viewModel.BulletMoveDuration).OnComplete(() =>
                {
                    CreateBulletHitParticle(signal.Enemy.Transform);
                    CreateDamagePopup(signal.Enemy.Transform, _viewModel.Damage);
                    SoundManager.Instance.CreateSoundBuilder().WithRandomPitch().WithPosition(bullet.transform.position).Play(attackSoundData);
                    ObjectPoolManager.ReturnObjectToPool(bullet);
                    signal.Enemy.TakeDamageEvent?.Invoke(_viewModel.Damage);
                });

                await UniTask.Delay(TimeSpan.FromSeconds(_viewModel.AttackInterval), cancellationToken: attackCancellationTokenSource);
            }
        }
        
        private void CreateBulletHitParticle(Transform target)
        {
            ObjectPoolManager.SpawnObject(bulletHitParticlePrefab, target.position, Quaternion.identity);
        }
        
        private void CreateDamagePopup(Transform target, int damage)
        {
            Vector3 targetPosition = target.position;
            targetPosition.y += 0.5f;
            ObjectPoolManager.SpawnObjectWithZenject(damagePopupPrefab, targetPosition, Quaternion.identity);
            _signalBus.Fire(new SetDamagePopupTextSignal(damage));
        }
        
        private void StopAttack()
        {
            _attackCancellationTokenSource.Cancel();
        }
        
        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<StopDefenderAttackSignal>(StopAttack);
            _signalBus.Unsubscribe<StartDefenderAttackSignal>(StartDefenderAttack);
            _signalBus.Unsubscribe<BossSequenceSignal>(StopAttack);
        }
        
        private void OnDisable()
        {
            StopAttack();
            UnsubscribeEvents();
        }
    }
}