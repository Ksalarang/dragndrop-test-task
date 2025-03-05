using UnityEngine;

namespace DragNDrop.Utils
{
    public static class Physics2DUtils
    {
        public static Collider2D Raycast(Camera camera, Vector3 screenPosition)
        {
            var worldPosition = camera.ScreenToWorldPoint(screenPosition);
            var hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            return hit.collider;
        }
    }
}