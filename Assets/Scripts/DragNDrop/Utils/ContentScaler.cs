using UnityEngine;

namespace DragNDrop.Utils
{
    public class ContentScaler : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        private void Start()
        {
            var topLeft = _camera.ScreenToWorldPoint(new Vector3(0, Screen.height));
            var bottomLeft = _camera.ScreenToWorldPoint(Vector3.zero);
            var scale = topLeft.y - bottomLeft.y;

            transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}