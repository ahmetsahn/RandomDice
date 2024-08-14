using DG.Tweening;
using Runtime.EnemySystem.View;
using UnityEngine;
using Zenject;

namespace Runtime.EnemySystem.Controller
{
    public class EnemyVisualController : MonoBehaviour
    {
        private EnemyViewModel _viewModel;
        
        [Inject]
        private void Construct(EnemyViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void OnEnable()
        {
            SetDefaultScale();
        }
        
        private void SetDefaultScale()
        {
            _viewModel.transform.DOScale(_viewModel.DefaultScale, 0.3f).SetEase(Ease.OutBack);
        }
        
        private void ResetScale()
        {
            transform.localScale = Vector3.zero;
        }
        
        private void Reset()
        {
            ResetScale();
        }
        
        private void OnDisable()
        {
            Reset();
        }
    }
}