using System.Collections.Generic;
using System.Linq;
using DragNDrop.Extensions;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DragNDrop.Draggables
{
    public class ObjectOrderHandler : IObjectOrderHandler, IInitializable
    {
        private const float OrderStep = 0.1f;

        [Inject]
        private readonly Transform _draggablesParent;

        private readonly List<DraggableObject> _draggables = new();

        public void Initialize()
        {
            _draggables.AddRange(_draggablesParent.GetComponentsInChildren<DraggableObject>());
        }

        public void UpdateOrder()
        {
            var list = _draggables.OrderByDescending(d => d.BottomPoint.y).ToList();

            for (var i = 0; i < list.Count; i++)
            {
                list[i].transform.SetLocalZ(-(i * OrderStep + OrderStep));
            }
        }
    }
}