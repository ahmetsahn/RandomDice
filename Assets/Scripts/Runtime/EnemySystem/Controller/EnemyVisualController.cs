using System;
using DG.Tweening;
using Runtime.EnemySystem.Model;
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
            _viewModel.transform.DOScale(_viewModel.Scale, 0.3f).SetEase(Ease.OutBack);
        }
        
        private void SetDefaultScale()
        {
            transform.localScale = Vector3.zero;
        }
        
        private void Reset()
        {
            SetDefaultScale();
        }
        
        private void OnDisable()
        {
            Reset();
        }
    }
}