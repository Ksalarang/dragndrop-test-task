using System;
using DragNDrop.Extensions;
using DragNDrop.UserInput;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DragNDrop
{
    public class SceneScroller : IInitializable, IDisposable
    {
        [Inject]
        private readonly IInputHandler _inputHandler;

        [Inject]
        private readonly SpriteRenderer _background;

        [Inject]
        private readonly Camera _camera;

        private bool _dragging;
        private int _pointerIndex;

        public void Initialize()
        {
            _inputHandler.OnPointerDown += OnPointerDown;
            _inputHandler.OnDrag += OnDrag;
        }

        public void Dispose()
        {
            _inputHandler.OnPointerDown -= OnPointerDown;
            _inputHandler.OnDrag -= OnDrag;
        }

        private void OnPointerDown(Collider2D collider2d, int pointerIndex)
        {
            _dragging = collider2d.gameObject.CompareTag("Background");

            if (_dragging)
            {
                _pointerIndex = pointerIndex;
            }
        }

        private void OnDrag(Vector3 delta, int pointerIndex)
        {
            if (!_dragging || pointerIndex != _pointerIndex)
            {
                return;
            }

            var transform = _background.transform;
            var position = transform.localPosition;
            position.x += delta.x;
            transform.localPosition = position;

            var bounds = _background.sprite.bounds;
            var minX = _camera.ScreenToWorldPoint(Vector3.zero).x;
            var maxX = _camera.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;
            var halfWidth = bounds.extents.x * transform.localScale.x;

            if (position.x - halfWidth > minX)
            {
                transform.SetLocalX(minX + halfWidth);
            }
            else if (position.x + halfWidth < maxX)
            {
                transform.SetLocalX(maxX - halfWidth);
            }
        }
    }
}