using UnityEngine;

namespace MoveStopMove.Presentation.UI
{
    public abstract class UICanvas : MonoBehaviour
    {
        //public bool IsAvoidBackKey = false;
        public bool IsDestroyOnClose = false;

        protected RectTransform RectTransform;

        private Animator m_animator;
        private bool m_isInit = false;
        private float m_offsetY = 0;

        private void Start()
        {
            Init();
        }

        protected void Init()
        {
            RectTransform = GetComponent<RectTransform>();
            m_animator = GetComponent<Animator>();

            //float ratio = (float)Screen.height / (float)Screen.width;

            //// xu ly tai tho
            //if (ratio > 2.1f)
            //{
            //    Vector2 leftBottom = m_RectTransform.offsetMin;
            //    Vector2 rightTop = m_RectTransform.offsetMax;
            //    rightTop.y = -100f;
            //    m_RectTransform.offsetMax = rightTop;
            //    leftBottom.y = 0f;
            //    m_RectTransform.offsetMin = leftBottom;
            //    m_OffsetY = 100f;
            //}
            //m_IsInit = true;
        }

        public virtual void Setup()
        {
            UIManager.Instance.AddBackUI(this);
            UIManager.Instance.PushBackAction(this, BackKey);
        }

        public virtual void BackKey() { }

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            UIManager.Instance.RemoveBackUI(this);

            gameObject.SetActive(false);
            if (IsDestroyOnClose)
            {
                Destroy(gameObject);
            }
        }
    }
}