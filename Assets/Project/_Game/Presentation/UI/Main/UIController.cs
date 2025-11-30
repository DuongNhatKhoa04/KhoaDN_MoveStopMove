using MoveStopMove.Core;
using MoveStopMove.Gameplay.Camera;
using MoveStopMove.Presentation.UI.Setting;
using UnityEngine;

namespace MoveStopMove.Presentation.UI.Main
{
    public class UIController : UICanvas
    {
        #region -- Methods --

        private void OnEnable()
        {
            CameraFollower.Instance.offset = new Vector3(0f, 15f, -20f);
            GameManager.Instance.FindController();
        }

        public void OnClickSettingButton()
        {
            UIManager.Instance.OpenUI<UISettingGameplay>();
        }

        #endregion
    }
}