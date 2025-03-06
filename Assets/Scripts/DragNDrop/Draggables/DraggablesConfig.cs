using UnityEngine;

namespace DragNDrop.Draggables
{
    [CreateAssetMenu(fileName = "DraggablesConfig", menuName = "Configs/DraggablesConfig", order = 0)]
    public class DraggablesConfig : ScriptableObject
    {
        [field: SerializeField]
        public float DragScale { get; private set; }

        [field: SerializeField]
        public float DragScaleDuration { get; private set; }

        [field: SerializeField]
        public float FallAcceleration { get; private set; }
    }
}