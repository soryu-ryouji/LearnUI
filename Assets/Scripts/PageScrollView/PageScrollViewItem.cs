using UnityEngine;

namespace LearnUI.PageScrollView
{
    public class PageScrollViewItem : MonoBehaviour
    {
        public float GetDistance(RectTransform center)
        {
            var center1 = GetRectTransformCenter(center);
            var center2 = GetRectTransformCenter(gameObject.GetComponent<RectTransform>());

            return Mathf.Abs(center1.x - center2.x);
        }

        private Vector2 GetRectTransformCenter(RectTransform rectTransform)
        {
            var worldPosition = rectTransform.TransformPoint(rectTransform.rect.center);
            return (Vector2)worldPosition;
        }

        public void ScaleTo(float scale)
        {
            gameObject.transform.localScale = new Vector3(scale, scale);
        }
    }
}