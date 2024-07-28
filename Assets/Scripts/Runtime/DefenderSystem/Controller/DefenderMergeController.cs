using Runtime.DefenderSystem.Model;
using Runtime.DefenderSystem.View;
using UnityEngine;
using Zenject;

namespace Runtime.DefenderSystem.Controller
{
    public class DefenderMergeController : MonoBehaviour
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
            SetLevelText();
        }
        
        private void SubscribeEvents()
        {
            
        }
        
        private void SetLevelText()
        {
            _viewModel.LevelText.text = _viewModel.Level.ToString();
        }
        
        private void Reset()
        {
            
        }
        
        private void OnDisable()
        {
            Reset();
        }
    }
}