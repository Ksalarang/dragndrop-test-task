using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DragNDrop.Draggables
{
    public class ObjectDropHandler : IObjectDropHandler, IInitializable
    {
        [Inject]
        private readonly Transform _surfacesParent;

        [Inject]
        private readonly DraggablesConfig _draggablesConfig;

        private readonly List<Surface> _surfaces = new();

        public void Initialize()
        {
            _surfaces.AddRange(_surfacesParent.GetComponentsInChildren<Surface>());
        }

        public void Drop(DraggableObject draggable)
        {
            var surface = _surfaces.FirstOrDefault(s => s.Collider.OverlapPoint(draggable.BottomPoint));

            if (surface != null)
            {
                Debug.Log($"{draggable.name} is on {surface.name}");
            }
            else
            {
                var nearestSurface = GetNearestSurfaceBelow(draggable);
                //fixme: add token;
                DropAsync(draggable, nearestSurface, CancellationToken.None).Forget();
            }
        }

        private Surface GetNearestSurfaceBelow(DraggableObject draggable)
        {
            Surface nearest = null;
            var minDistance = float.MaxValue;
            var draggablePosition = draggable.BottomPoint;

            foreach (var surface in _surfaces)
            {
                var surfacePosition = surface.transform.position;

                if (surfacePosition.y > draggablePosition.y)
                {
                    continue;
                }

                var distance = Vector3.Distance(surfacePosition, draggablePosition);

                if (distance < minDistance)
                {
                    nearest = surface;
                    minDistance = distance;
                }
            }

            return nearest;
        }

        private async UniTask DropAsync(DraggableObject draggable, Surface surface, CancellationToken token)
        {
            var position = draggable.BottomPoint;
            var destinationY = surface.Collider.bounds.max.y + draggable.Collider.bounds.extents.y;
            var destination = new Vector3(position.x, destinationY);
            var distance = Vector3.Distance(position, destination);
            var speed = Mathf.Sqrt((distance * _draggablesConfig.FallAcceleration) / 2f);
            var duration = distance / speed;

            await draggable.transform.DOMoveY(destination.y, duration)
                .SetEase(Ease.InQuad).WithCancellation(token);
        }
    }
}