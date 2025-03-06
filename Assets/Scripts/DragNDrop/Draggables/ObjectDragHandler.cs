using System;
using System.Collections.Generic;
using DragNDrop.UserInput;
using DragNDrop.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DragNDrop.Draggables
{
    public class ObjectDragHandler : IInitializable, IStartable, IDisposable
    {
        [Inject]
        private readonly IInputHandler _inputHandler;

        [Inject]
        private readonly IObjectDropHandler _dropHandler;

        [Inject]
        private readonly Camera _camera;

        [Inject]
        private readonly DraggablesConfig _draggablesConfig;

        private readonly Dictionary<int, DraggableObject> _draggables = new();

        private Vector3 _bottomLeftCorner;
        private Vector3 _topRightCorner;

        public void Initialize()
        {
            _inputHandler.OnPointerDown += OnPointerDown;
            _inputHandler.OnDrag += OnDrag;
            _inputHandler.OnPointerUp += OnPointerUp;
        }

        public void Start()
        {
            _bottomLeftCorner = _camera.ScreenToWorldPoint(Vector3.zero);
            _topRightCorner = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        }

        public void Dispose()
        {
            _inputHandler.OnPointerDown -= OnPointerDown;
            _inputHandler.OnDrag -= OnDrag;
            _inputHandler.OnPointerUp -= OnPointerUp;
        }

        private void OnPointerDown(Collider2D collider, int pointerIndex)
        {
            if (collider.TryGetComponent(out DraggableObject draggable))
            {
                _draggables[pointerIndex] = draggable;

                draggable.transform.localScale = draggable.DefaultScale * _draggablesConfig.DragScale;
            }
        }

        private void OnDrag(Vector3 delta, int pointerIndex)
        {
            if (_draggables.TryGetValue(pointerIndex, out var draggable))
            {
                var position = draggable.transform.position;
                position += delta;
                position = MathUtils.ClampWithinCorners(position, draggable.Collider.bounds.extents,
                    _bottomLeftCorner, _topRightCorner);

                draggable.transform.position = position;
            }
        }

        private void OnPointerUp(int pointerIndex)
        {
            if (_draggables.TryGetValue(pointerIndex, out var draggable))
            {
                _draggables.Remove(pointerIndex);
                _dropHandler.Drop(draggable);
            }
        }
    }
}