using UnityEngine;

namespace DragNDrop.Extensions
{
    public static class TransformExtensions
    {
        public static void SetLocalX(this Transform transform, float x)
        {
            var position = transform.localPosition;
            position.x = x;
            transform.localPosition = position;
        }

        public static void SetLocalZ(this Transform transform, float z)
        {
            var position = transform.localPosition;
            position.z = z;
            transform.localPosition = position;
        }
    }
}