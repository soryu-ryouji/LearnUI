using UnityEngine;

namespace LearnUI.PageScrollView
{
    public class PageScrollViewInputController : MonoBehaviour
    {
        public PageScrollView pageScrollView;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A) && !pageScrollView.IsScrolling)
            {
                pageScrollView.Move(pageScrollView.CurrentPageIndex - 1);
            }
            if (Input.GetKeyDown(KeyCode.D) && !pageScrollView.IsScrolling)
            {
                pageScrollView.Move(pageScrollView.CurrentPageIndex + 1);
            }
        }
    }
}
