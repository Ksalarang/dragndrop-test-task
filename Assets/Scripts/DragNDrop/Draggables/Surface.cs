using UnityEngine;

namespace DragNDrop.Draggables
{
    public class Surface : MonoBehaviour
    {
        [field: SerializeField]
        public Collider2D Collider { get; private set; }
    }
}