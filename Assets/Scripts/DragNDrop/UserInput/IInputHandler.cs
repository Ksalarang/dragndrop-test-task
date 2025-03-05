using System;
using UnityEngine;

namespace DragNDrop.UserInput
{
    public interface IInputHandler
    {
        event Action<Collider2D, int> OnPointerDown;
        event Action<int> OnPointerUp;
        event Action<Vector3, int> OnDrag;
    }
}