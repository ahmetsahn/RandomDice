using Runtime.DefenderSystem.Model;
using Runtime.DefenderSystem.View;
using UnityEngine;
using Zenject;

namespace Runtime.DefenderSystem.Controller
{
    public class DefenderVisualController : MonoBehaviour
    {
        private DefenderViewModel _viewModel;
        
        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(DefenderViewModel viewModel, SignalBus signalBus)
        {
            _viewModel = viewModel;
            _signalBus = signalBus;
        }
        
        private void OnEnable()
        {
            SubscribeEvents();
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