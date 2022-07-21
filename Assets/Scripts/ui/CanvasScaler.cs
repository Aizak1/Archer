using UnityEngine;

namespace ui {
    public class CanvasScaler : MonoBehaviour{
        [SerializeField]
        private RectTransform[] canvasTransforms;
        [SerializeField]
        private RectTransform refCanvasTransform;

        private void Start() {
            float width = refCanvasTransform.rect.width;
            float height = refCanvasTransform.rect.height;

            Vector2 newSize = new Vector2(width, height);

            for (int i = 0; i < canvasTransforms.Length; i++) {
                canvasTransforms[i].sizeDelta = newSize;
            }
        }
    }
}