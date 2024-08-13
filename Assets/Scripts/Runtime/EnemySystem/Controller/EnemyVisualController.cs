using System;
using DG.Tweening;
using Runtime.EnemySystem.View;
using UnityEngine;

namespace Runtime.EnemySystem.Controller
{
    public class EnemyVisualController : IDisposable
    {
        private readonly EnemyViewModel _viewModel;
        
        public EnemyVisualController(EnemyViewModel viewModel)
        {
            _viewModel = viewModel;
            
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _viewModel.OnEnableEvent += SetDefaultScale;
            _viewModel.OnDisableEvent += ResetScale;
        }
        
        private void SetDefaultScale()
        {
            _viewModel.transform.DOScale(_viewModel.DefaultScale, 0.3f).SetEase(Ease.OutBack);
        }
        
        private void ResetScale()
        {
            _viewModel.transform.localScale = Vector3.zero;
        }
        
        private void UnsubscribeEvents()
        {
            _viewModel.OnEnableEvent -= SetDefaultScale;
            _viewModel.OnDisableEvent -= ResetScale;
        }
        
        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
}