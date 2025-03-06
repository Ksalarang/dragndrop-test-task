using System;
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
    public class ObjectDropHandler : IObjectDropHandler, IInitializable, IDisposable
    {
        [Inject]
        private readonly Transform _surfacesParent;

        [Inject]
        private readonly DraggablesConfig _draggablesConfig;

        private readonly List<Surface> _surfaces = new();
        private readonly Dictionary<DraggableObject, CancellationTokenSource> _tokenSources = new();

        public void Initialize()
        {
            _surfaces.AddRange(_surfacesParent.GetComponentsInChildren<Surface>());
        }

        public void Dispose()
        {
            foreach (var source in _tokenSources.Values.Where(source => source.IsCancellationRequested == false))
            {
                source.Cancel();
                source.Dispose();
            }
        }

        public void Drop(DraggableObject draggable)
        {
            var surface = _surfaces.FirstOrDefault(s => s.Collider.OverlapPoint(draggable.BottomPoint));
            var token = GetToken(draggable);

            if (surface != null)
            {
                Debug.Log($"{draggable.name} is on {surface.name}");

                var yOffset = (draggable.DefaultScale.y * _draggablesConfig.DragScale - draggable.DefaultScale.y) / 2f;
                ScaleBackAsync(draggable, _draggablesConfig.DragScaleDuration, yOffset, token).Forget();
            }
            else
            {
                var nearestSurface = GetNearestSurfaceBelow(draggable);
                DropAsync(draggable, nearestSurface, token).Forget();
            }
        }

        public void CancelDrop(DraggableObject draggable)
        {
            CancelToken(draggable);
            draggable.transform.localScale = draggable.DefaultScale;
        }

        private Surface GetNearestSurfaceBelow(DraggableObject draggable)
        {
            Surface nearest = null;
            var minDistance = float.MaxValue;

            var draggablePosition = draggable.BottomPoint;
            var bounds = draggable.Collider.bounds;
            var boundsMin = bounds.min;
            var boundsMax = bounds.max;

            foreach (var surface in _surfaces)
            {
                var surfacePosition = surface.transform.position;
                var surfaceBounds = surface.Collider.bounds;

                if (surfacePosition.y > draggablePosition.y
                    || boundsMax.x < surfaceBounds.min.x || boundsMin.x > surfaceBounds.max.x)
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

            return nearest ?? _surfaces.First();
        }

        private async UniTask DropAsync(DraggableObject draggable, Surface surface, CancellationToken token)
        {
            var position = draggable.BottomPoint;
            var destinationY = surface.Collider.bounds.max.y
                + draggable.Collider.bounds.extents.y / _draggablesConfig.DragScale;
            var destination = new Vector3(position.x, destinationY);
            var distance = Vector3.Distance(position, destination);
            var speed = Mathf.Sqrt(2f * distance * _draggablesConfig.FallAcceleration);
            var duration = distance / speed;
            var scaleDuration = Mathf.Min(_draggablesConfig.DragScaleDuration, duration);

            ScaleBackAsync(draggable, scaleDuration, 0f, token).Forget();
            await draggable.transform.DOMoveY(destination.y, duration)
                .SetEase(Ease.InQuad).WithCancellation(token);
        }

        private CancellationToken GetToken(DraggableObject draggable)
        {
            CancelToken(draggable);
            var newSource = new CancellationTokenSource();
            _tokenSources[draggable] = newSource;
            return newSource.Token;
        }

        private void CancelToken(DraggableObject draggable)
        {
            if (_tokenSources.TryGetValue(draggable, out var tokenSource)
                && tokenSource.IsCancellationRequested == false)
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
        }

        private async UniTask ScaleBackAsync(DraggableObject draggable, float duration, float yOffset,
            CancellationToken token)
        {
            if (yOffset > 0f)
            {
                var y = draggable.transform.localPosition.y - yOffset;
                draggable.transform.DOLocalMoveY(y, duration).WithCancellation(token).Forget();
            }
            await draggable.transform.DOScale(draggable.DefaultScale, duration).WithCancellation(token);
        }
    }
}