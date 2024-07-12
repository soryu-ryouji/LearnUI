using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace LearnUI.PageScrollView
{
    public class PageScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        private List<RectTransform> _pageItems = new();
        [SerializeField] private ScrollRect _rect;
        [SerializeField] private GameObject _content;
        public int CurrentPageIndex
        {
            get
            {
                if (_pageItems.Count == 0) return 0;

                var interval = 1f / (_pageItems.Count - 1);
                int index = Mathf.RoundToInt(_rect.horizontalNormalizedPosition / interval);
                return index;
            }
        }

        private bool _isScrolling;
        [SerializeField] private float _scrollTime = 2f;
        [SerializeField] private float _stopThreshold = 0.001f;
        private IEnumerator _coroutine;

        private void InitView()
        {
            _pageItems.Clear();
            foreach (RectTransform item in _content.transform)
            {
                _pageItems.Add(item);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A) && !_isScrolling)
            {
                _coroutine = MoveTo(CurrentPageIndex - 1);
                StartCoroutine(_coroutine);
            }
            if (Input.GetKeyDown(KeyCode.D) && !_isScrolling)
            {
                _coroutine = MoveTo(CurrentPageIndex + 1);
                StartCoroutine(_coroutine);
            }
        }

        private void Start()
        {
            InitView();
        }

        public IEnumerator MoveTo(int pageIndex)
        {
            if (pageIndex >= _pageItems.Count || pageIndex < 0) yield break;

            _isScrolling = true;

            var interval = 1 / (_pageItems.Count - 1f);
            var targetPos = pageIndex * interval;
            float elapsed = 0f;

            while (elapsed < _scrollTime)
            {
                var tempPos = Mathf.Lerp(_rect.horizontalNormalizedPosition, targetPos, elapsed / _scrollTime);
                _rect.horizontalNormalizedPosition = tempPos;
                elapsed += Time.deltaTime;

                if (Mathf.Abs(_rect.horizontalNormalizedPosition - targetPos) < _stopThreshold)
                {
                    _rect.horizontalNormalizedPosition = targetPos;
                    break;
                }

                yield return null;
            }

            _rect.horizontalNormalizedPosition = targetPos;
            _isScrolling = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            StartCoroutine(MoveTo(CurrentPageIndex));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isScrolling)
            {
                StopCoroutine(_coroutine);
            }
        }
    }
}
