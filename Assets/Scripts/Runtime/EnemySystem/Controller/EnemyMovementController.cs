using DG.Tweening;
using Runtime.Core.Pool;
using Runtime.EnemySystem.View;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.EnemySystem.Controller
{
    public class EnemyMovementController : MonoBehaviour
    {
        private EnemyViewModel _viewModel;
        
        private Transform _pathArrayParent;
        
        private Vector3[] _pathArray;
        
        private Tween _moveTween;
        
        private SignalBus _signalBus;
        
        private const string PATH_PARENT_NAME = "Path";
        
        [Inject]
        private void Construct(EnemyViewModel viewModel, SignalBus signalBus)
        {
            _viewModel = viewModel;
            _signalBus = signalBus;
        }
        
        private void Awake()
        {
            SetPathArray();
        }

        private void SetPathArray()
        {
            _pathArrayParent = GameObject.Find(PATH_PARENT_NAME).transform;
            
            _pathArray = new Vector3[_pathArrayParent.childCount];

            for (int i = 0; i < _pathArrayParent.childCount; i++)
            {
                _pathArray[i] = _pathArrayParent.GetChild(i).position;
            }
        }
        
        private void OnEnable()
        {
            SetInitialPosition();
            Move();
        }
        
        private void SetInitialPosition()
        {
            _viewModel.transform.position = _pathArray[0];
        }

        private void Move()
        {
            _moveTween = _viewModel.transform.DOPath(_pathArray, 10 / _viewModel.Speed).SetEase(Ease.Linear);
            
            _moveTween.OnComplete(() =>
            {
                ObjectPoolManager.ReturnObjectToPool(_viewModel.gameObject);
                _signalBus.Fire(new DestroyHealthIconSignal());
            });
        }
        
        private void KillMoveTween()
        {
            _moveTween.Kill();
        }

        private void Reset()
        {
            KillMoveTween();
        }
        
        private void OnDisable()
        {
            Reset();
        }
    }
}