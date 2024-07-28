using System;
using DG.Tweening;
using Runtime.DefenderSystem.View;
using UnityEngine;
using Zenject;

namespace Runtime.DefenderSystem.Controller
{
    public class DefenderVisualController : MonoBehaviour
    {
        private DefenderViewModel _viewModel;
        
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
        }
        
        private void OnMergeable()
        {
            _viewModel.SpriteRenderer.color = _viewModel.DefaultColor;
        }
        
        private void OnUnMergeable()
        {
            _viewModel.SpriteRenderer.color = _viewModel.UnMergeableColor;
        }

        private void UnsubscribeEvents()
        {
            _viewModel.SetDefaultColorEvent -= OnMergeable;
            _viewModel.SetUnMergeableColorEvent -= OnUnMergeable;
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}