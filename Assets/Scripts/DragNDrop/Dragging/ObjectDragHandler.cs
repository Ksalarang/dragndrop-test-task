using System;
using System.Collections.Generic;
using DragNDrop.UserInput;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DragNDrop.Dragging
{
    public class ObjectDragHandler : IStartable, IDisposable
    {
        [Inject]
        private readonly IInputHandler _inputHandler;

        private readonly Dictionary<int, DraggableObject> _draggables = new();

        public void Start()
        {
            _inputHandler.OnPointerDown += OnPointerDown;
            _inputHandler.OnDrag += OnDrag;
            _inputHandler.OnPointerUp += OnPointerUp;
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
            }
        }

        private void OnDrag(Vector3 delta, int pointerIndex)
        {
            if (_draggables.TryGetValue(pointerIndex, out var draggable))
            {
                draggable.transform.position += delta;
            }
        }

        private void OnPointerUp(int pointerIndex)
        {
            if (_draggables.ContainsKey(pointerIndex))
            {
                _draggables.Remove(pointerIndex);
            }
        }
    }
}