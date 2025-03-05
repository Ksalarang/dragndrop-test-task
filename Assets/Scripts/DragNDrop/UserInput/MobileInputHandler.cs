using System;
using System.Collections.Generic;
using DragNDrop.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DragNDrop.UserInput
{
    public class MobileInputHandler : IInputHandler, ITickable
    {
        public event Action<Collider2D, int> OnPointerDown;
        public event Action<int> OnPointerUp;
        public event Action<Vector3, int> OnDrag;

        [Inject]
        private readonly Camera _camera;

        private readonly Dictionary<int, Vector2> _prevPositions = new();

        public void Tick()
        {
            for (var i = 0; i < Input.touches.Length; i++)
            {
                var touch = Input.touches[i];
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _prevPositions[i] = touch.position;
                        OnPointerDown?.Invoke(Physics2DUtils.Raycast(_camera, touch.position), i);
                        break;
                    case TouchPhase.Moved:
                        var delta = _camera.ScreenToWorldPoint(touch.position)
                            - _camera.ScreenToWorldPoint(_prevPositions[i]);
                        delta.z = 0;
                        _prevPositions[i] = touch.position;
                        OnDrag?.Invoke(delta, i);
                        break;
                    case TouchPhase.Stationary:
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        OnPointerUp?.Invoke(i);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(touch.phase),
                            $"Case {touch.phase} is not defined");
                }
            }
        }
    }
}