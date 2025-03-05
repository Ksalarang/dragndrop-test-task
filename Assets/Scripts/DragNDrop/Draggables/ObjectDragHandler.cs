using System;
using System.Collections.Generic;
using DragNDrop.UserInput;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DragNDrop.Draggables
{
    public class ObjectDragHandler : IStartable, IDisposable
    {
        [Inject]
        private readonly IInputHandler _inputHandler;

        [Inject]
        private readonly IObjectDropHandler _dropHandler;

        private readonly Dictionary<int, DraggableObject> _draggables = new();

        //todo: replace with Initialize
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
                //todo: limit position within the screen
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