using System;
using Runtime.DefenderSystem.View;
using Runtime.Signal;
using UnityEngine;
using Zenject;

namespace Runtime.DefenderSystem.Controller
{
    public class DefenderDragController : ITickable, IDisposable
    {
        private readonly DefenderViewModel _viewModel;
        
        private Camera _camera;
        
        private bool _dragging;
        
        private Vector3 _offset;
        
        public DefenderDragController(DefenderViewModel viewModel)
        {
            _viewModel = viewModel;
            
            SubscribeEvents();
            Initialize();
        }
        
        private void SubscribeEvents()
        {
            _viewModel.OnMouseDownEvent += OnMouseDown;
            _viewModel.OnMouseUpEvent += OnMouseUp;
        }

        private void Initialize()
        {
            _camera = Camera.main;
        }

        private void OnMouseDown()
        {
            _offset = _viewModel.transform.position - _camera.ScreenToWorldPoint(Input.mousePosition);
            _dragging = true;
        }

        private void OnMouseUp()
        {
            _dragging = false;
        }

        public void Tick()
        {
            if (_dragging)
            {
                _viewModel.transform.position = _camera.ScreenToWorldPoint(Input.mousePosition) + _offset;
            }
        }
        
        private void UnsubscribeEvents()
        {
            _viewModel.OnMouseDownEvent -= OnMouseDown;
            _viewModel.OnMouseUpEvent -= OnMouseUp;
        }
        
        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
}