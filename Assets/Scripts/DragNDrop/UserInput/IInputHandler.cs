using System;
using UnityEngine;

namespace DragNDrop.UserInput
{
    public interface IInputHandler
    {
        event Action<Collider2D> OnPointerDown;
        event Action OnPointerUp;
        event Action<Vector3> OnDrag;
    }
}