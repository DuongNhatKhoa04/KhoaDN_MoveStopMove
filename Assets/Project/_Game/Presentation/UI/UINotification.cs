using MoveStopMove.Core.Events;
using MoveStopMove.Core.Interfaces;
using TMPro;
using UnityEngine;

namespace MoveStopMove.Presentation.UI
{
    public class UINotification : UICanvas, IMyObserver<NotificationPopUpEvent>
    {
        [SerializeField] private TextMeshProUGUI m_message;

        public void OnNotify(NotificationPopUpEvent data)
        {
            m_message.text = data.Message;

            UIManager.Instance.OpenUI<UINotification>();
        }

        private void Awake()
        {
            EventManager.Instance.Subscribe<NotificationPopUpEvent>(this);
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
                EventManager.Instance.Unsubscribe<NotificationPopUpEvent>(this);
        }

        public void OnClickCloseButton()
        {
            UIManager.Instance.CloseUI<UINotification>();
        }
    }
}