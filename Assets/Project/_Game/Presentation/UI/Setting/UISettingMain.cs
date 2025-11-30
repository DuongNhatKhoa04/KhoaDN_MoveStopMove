using MoveStopMove.Presentation.UI.Main;
using UnityEngine;

namespace MoveStopMove.Presentation.UI.Setting
{
    public class UISettingMain : UICanvas
    {
        #region -- Fields --

        [SerializeField] private GameObject soundOn;
        [SerializeField] private GameObject soundOff;

        #endregion

        #region -- Methods --

        private void OnEnable()
        {
            Time.timeScale = 0;
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
        }

        public void OnClickBackButton()
        {
            UIManager.Instance.OpenUI<UIMain>();
            UIManager.Instance.CloseUI<UISettingMain>();
        }

        public void OnClickTurnOffSound()
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }

        public void OnClickTurnOnSound()
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }

        #endregion
    }
}