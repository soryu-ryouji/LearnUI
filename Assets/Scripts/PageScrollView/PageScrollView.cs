using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LearnUI.PageScrollView
{
    public class PageScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        private List<PageScrollViewItem> m_PageItems = new();
        [SerializeField] private ScrollRect m_Rect;
        [SerializeField] private GameObject m_Content;

        public int CurrentPageIndex
        {
            get
            {
                if (m_PageItems.Count == 0) return 0;

                int index = Mathf.RoundToInt(m_Rect.horizontalNormalizedPosition / Interval);
                return index;
            }
        }

        public float Interval => 1f / (m_PageItems.Count - 1);

        [SerializeField] private float m_ScaleDistance;
        [SerializeField] private float m_MinScale;
        [SerializeField] private float m_MaxScale;
        [SerializeField] float spacing;

        private bool m_IsScrolling;

        public bool IsScrolling { get { return m_IsScrolling; } }

        [SerializeField] private float m_ScrollSpeed = 2f;
        [SerializeField] private float m_StopThreshold = 0.001f;

        private Coroutine m_MoveCoroutine;

        private void InitView()
        {
            m_PageItems = m_Content.GetComponentsInChildren<PageScrollViewItem>().ToList();
        }

        private void Update()
        {
            if (m_IsScrolling) UpdateItemScale();
        }

        private void Start()
        {
            InitView();
        }

        public void Move(int pageIndex)
        {
            m_MoveCoroutine = StartCoroutine(MoveTo(pageIndex));
        }

        public void Stop()
        {
            if (m_MoveCoroutine != null)
            {
                StopCoroutine(m_MoveCoroutine);
            }
        }

        public IEnumerator MoveTo(int pageIndex)
        {
            if (pageIndex >= m_PageItems.Count || pageIndex < 0)
            {
                yield break;
            }

            m_IsScrolling = true;

            var targetPos = pageIndex * Interval;
            float elapsed = 0f;

            while (elapsed < m_ScrollSpeed)
            {
                var tempPos = Mathf.Lerp(m_Rect.horizontalNormalizedPosition, targetPos, elapsed / m_ScrollSpeed);
                m_Rect.horizontalNormalizedPosition = tempPos;
                elapsed += Time.deltaTime;

                if (Mathf.Abs(m_Rect.horizontalNormalizedPosition - targetPos) < m_StopThreshold)
                {
                    m_Rect.horizontalNormalizedPosition = targetPos;
                    break;
                }

                yield return null;
            }

            m_Rect.horizontalNormalizedPosition = targetPos;
            m_IsScrolling = false;
        }

        private void UpdateItemScale()
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            foreach (var item in m_PageItems)
            {
                var distance = item.GetDistance(rectTransform);
                var scale = Mathf.Abs(distance) >= m_ScaleDistance
                    ? m_MinScale
                    : Mathf.Lerp(m_MaxScale, m_MinScale, Mathf.Abs(distance) / m_ScaleDistance);

                item.ScaleTo(scale);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_IsScrolling = true;

            Stop();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_IsScrolling = false;

            Move(CurrentPageIndex);
        }
    }
}
