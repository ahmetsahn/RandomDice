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
            SetLevelText();
        }
        
        private void SetLevelText()
        {
            _viewModel.LevelText.text = _viewModel.Level.ToString();
        }
    }
}