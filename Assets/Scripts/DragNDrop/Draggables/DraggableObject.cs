using UnityEngine;

namespace DragNDrop.Draggables
{
    public class DraggableObject : MonoBehaviour
    {
        [field: SerializeField]
        public Collider2D Collider { get; private set; }

        public Vector3 BottomPoint
        {
            get
            {
                var bounds = Collider.bounds;
                return new Vector3(bounds.center.x, bounds.min.y);
            }
        }

        public Vector3 DefaultScale { get; private set; }

        private void Awake()
        {
            DefaultScale = transform.localScale;
        }
    }
}