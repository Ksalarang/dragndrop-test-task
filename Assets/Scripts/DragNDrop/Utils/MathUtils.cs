using UnityEngine;

namespace DragNDrop.Utils
{
    public static class MathUtils
    {
        public static Vector3 ClampWithinCorners(Vector3 position, Vector3 extents, Vector3 bottomLeftCorner,
            Vector3 topRightCorner)
        {
            if (position.x - extents.x < bottomLeftCorner.x)
            {
                position.x = bottomLeftCorner.x + extents.x;
            }
            else if (position.x + extents.x > topRightCorner.x)
            {
                position.x = topRightCorner.x - extents.x;
            }
            if (position.y - extents.y < bottomLeftCorner.y)
            {
                position.y = bottomLeftCorner.y + extents.y;
            }
            else if (position.y + extents.y > topRightCorner.y)
            {
                position.y = topRightCorner.y - extents.y;
            }

            return position;
        }
    }
}