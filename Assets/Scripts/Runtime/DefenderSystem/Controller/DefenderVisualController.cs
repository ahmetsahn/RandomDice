using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.DefenderSystem.View;
using UnityEngine;
using Zenject;

namespace Runtime.DefenderSystem.Controller
{
    public class DefenderVisualController : MonoBehaviour
    {
        private DefenderViewModel _viewModel;
        
        private CancellationTokenSource _upgradeCancellationTokenSource = new();
        
        [Inject]
        private void Construct(DefenderViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        
        private void OnEnable()
        {
            SubscribeEvents();
            SetDefaultScale();
        }
        
        private void SetDefaultScale()
        {
            _viewModel.transform.DOScale(_viewModel.DefaultScale, 0.3f).SetEase(Ease.OutBack);
        }
        
        private void SubscribeEvents()
        {
            _viewModel.SetDefaultColorEvent += OnMergeable;
            _viewModel.SetUnMergeableColorEvent += OnUnMergeable;
            _viewModel.UpgradeDefenderEvent += UpgradeDefenderEvent;
            _viewModel.OnMouseDownEvent += OnMouseDownEvent;
            _viewModel.OnMouseUpEvent += OnMouseUpEvent;
        }
        
        private void OnMergeable()
        {
            _viewModel.SpriteRenderer.color = _viewModel.DefaultColor;
        }
        
        private void OnUnMergeable()
        {
            _viewModel.SpriteRenderer.color = _viewModel.UnMergeableColor;
        }
        
        private void UpgradeDefenderEvent(int _)
        {
            _upgradeCancellationTokenSource.Cancel();
            _upgradeCancellationTokenSource = new CancellationTokenSource();
            UpgradeSequence().Forget();
        }
        
        private async UniTask UpgradeSequence()
        {
            _viewModel.LevelText.gameObject.SetActive(false);
            _viewModel.LevelUpgradeText.DOFade(1, 0.5f);
            _viewModel.UpgradeParticleGameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _upgradeCancellationTokenSource.Token);
            _viewModel.LevelUpgradeText.DOFade(0, 0.5f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _upgradeCancellationTokenSource.Token);
            _viewModel.UpgradeParticleGameObject.SetActive(false);
            _viewModel.LevelText.gameObject.SetActive(true);
        }
        
        private void OnMouseDownEvent()
        {
            _viewModel.SpriteRenderer.sortingOrder = _viewModel.SpriteRendererSelectedSortingOrder;
            _viewModel.LevelText.sortingOrder = _viewModel.LevelTextSelectedSortingOrder;
        }
        
        private void OnMouseUpEvent()
        {
            _viewModel.SpriteRenderer.sortingOrder = _viewModel.SpriteRendererDefaultSortingOrder;
            _viewModel.LevelText.sortingOrder = _viewModel.LevelTextDefaultSortingOrder;
        }

        private void UnsubscribeEvents()
        {
            _viewModel.SetDefaultColorEvent -= OnMergeable;
            _viewModel.SetUnMergeableColorEvent -= OnUnMergeable;
            _viewModel.UpgradeDefenderEvent -= UpgradeDefenderEvent;
            _viewModel.OnMouseDownEvent -= OnMouseDownEvent;
            _viewModel.OnMouseUpEvent -= OnMouseUpEvent;
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}