using System;
using DragNDrop.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DragNDrop.UserInput
{
    public class DesktopInputHandler : IInputHandler, ITickable
    {
        public event Action<Collider2D, int> OnPointerDown;
        public event Action<int> OnPointerUp;
        public event Action<Vector3, int> OnDrag;

        [Inject]
        private readonly Camera _camera;

        private bool _isPressing;
        private bool _wasPressing;
        private Vector3 _prevMousePosition;

        public void Tick()
        {
            _wasPressing = _isPressing;
            _isPressing = Input.GetMouseButton(0);

            if (_isPressing)
            {
                var mousePosition = Input.mousePosition;

                if (_wasPressing == false)
                {
                    _prevMousePosition = mousePosition;
                    OnPointerDown?.Invoke(Physics2DUtils.Raycast(_camera, mousePosition), 0);
                }
                else
                {
                    var delta = _camera.ScreenToWorldPoint(mousePosition)
                        - _camera.ScreenToWorldPoint(_prevMousePosition);
                    delta.z = 0;
                    _prevMousePosition = mousePosition;
                    OnDrag?.Invoke(delta, 0);
                }
            }
            else
            {
                if (_wasPressing)
                {
                    OnPointerUp?.Invoke(0);
                }
            }
        }
    }
}