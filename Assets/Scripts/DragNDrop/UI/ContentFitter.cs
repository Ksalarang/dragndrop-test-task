using UnityEngine;

namespace DragNDrop.UI
{
    public class ContentFitter : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _content;

        [SerializeField]
        private RectTransform _background;

        private void Start()
        {
            _content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _background.rect.width);
            Debug.Log(_background.rect);
        }
    }
}