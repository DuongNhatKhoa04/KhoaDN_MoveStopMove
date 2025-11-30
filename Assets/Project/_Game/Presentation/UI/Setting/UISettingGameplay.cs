using System;
using MoveStopMove.Core.SaveLoad;
using TMPro;
using UnityEngine;

namespace MoveStopMove.Presentation.UI.Setting
{
    public class UISettingGameplay : UICanvas
    {
        #region -- Fields --

        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameObject soundOn;
        [SerializeField] private GameObject soundOff;

        #endregion

        #region -- Methods --

        private void OnEnable()
        {
            Time.timeScale = 0f;
            scoreText.text = DataPersistenceManager.Instance.GameData.coins.ToString();
            DataPersistenceManager.Instance.SaveGame();
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
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

        public void OnClickRestartGame()
        {
            UIManager.Instance.CloseUI<UISettingGameplay>();
            //GameManager restart
        }

        public void OnClickResumeGame()
        {
            UIManager.Instance.CloseUI<UISettingGameplay>();
        }

        #endregion
    }
}