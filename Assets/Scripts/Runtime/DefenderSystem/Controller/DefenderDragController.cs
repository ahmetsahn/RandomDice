using Runtime.DefenderSystem.View;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.DefenderSystem.Controller
{
    public class DefenderDragController : MonoBehaviour
    {
        private DefenderViewModel _viewModel;
        
        private SignalBus _signalBus;
        
        private Camera _camera;
        
        private bool _dragging;
        
        private Vector3 _offset;

        [Inject]
        private void Construct(DefenderViewModel viewModel, SignalBus signalBus)
        {
            _viewModel = viewModel;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Update()
        {
            if (_dragging)
            {
                _viewModel.transform.position = _camera.ScreenToWorldPoint(Input.mousePosition) + _offset;
            }
        }

        private void OnMouseDown()
        {
            _offset = _viewModel.transform.position - _camera.ScreenToWorldPoint(Input.mousePosition);
            _dragging = true;
            _signalBus.Fire(new ShowMergeableDefendersSignal(_viewModel));
        }

        private void OnMouseUp()
        {
            _dragging = false;
            _signalBus.Fire(new MergeDefendersSignal(_viewModel));
        }
    }
}