using MoveStopMove.Core.Events;
using MoveStopMove.Core.Interfaces;
using MoveStopMove.Presentation.UI;
using MoveStopMove.Utility.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoveStopMove.Presentation.UI.Main
{
    public class UINotification : UICanvas, IMyObserver<NotificationPopUpEvent>
    {
        #region -- Fields --

        [SerializeField] private TextMeshProUGUI message;

        #endregion

        #region -- Methods --

        public void OnNotify(NotificationPopUpEvent data)
        {
            SoundManager.Instance.PlaySFX(ESfxType.Notification);
            message.text = data.Message;

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
            SoundManager.Instance.PlaySFX(ESfxType.ButtonClick);
            UIManager.Instance.CloseUI<UINotification>();
        }

        #endregion
    }
}