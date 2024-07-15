using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LearnUI.PageScrollView
{
    public class PageScrollView : MonoBehaviour
    {
        private List<RectTransform> m_PageItems = new();
        [SerializeField] private ScrollRect m_Rect;
        [SerializeField] private GameObject m_Content;
        public int CurrentPageIndex
        {
            get
            {
                if (m_PageItems.Count == 0) return 0;

                var interval = 1f / (m_PageItems.Count - 1);
                int index = Mathf.RoundToInt(m_Rect.horizontalNormalizedPosition / interval);
                return index;
            }
        }

        private bool m_IsScrolling;
        [SerializeField] private float m_ScrollSpeed = 2f;
        [SerializeField] private float m_StopThreshold = 0.001f;

        private void InitView()
        {
            m_PageItems.Clear();
            foreach (RectTransform item in m_Content.transform)
            {
                m_PageItems.Add(item);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) && !m_IsScrolling) StartCoroutine(MoveTo(3));
            if (Input.GetKeyDown(KeyCode.Space) && !m_IsScrolling) StartCoroutine(MoveTo(0));

            if (Input.GetKeyDown(KeyCode.A) && !m_IsScrolling) StartCoroutine(MoveTo(CurrentPageIndex - 1));
            if (Input.GetKeyDown(KeyCode.D) && !m_IsScrolling) StartCoroutine(MoveTo(CurrentPageIndex + 1));
        }

        private void Start()
        {
            InitView();
        }

        public IEnumerator MoveTo(int pageIndex)
        {
            if (pageIndex >= m_PageItems.Count || pageIndex < 0) yield break;

            m_IsScrolling = true;

            var interval = 1 / (m_PageItems.Count - 1f);
            var targetPos = pageIndex * interval;
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
    }
}
