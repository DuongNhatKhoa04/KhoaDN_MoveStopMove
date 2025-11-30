using System;
using MoveStopMove.Core;
using MoveStopMove.Core.Events;
using MoveStopMove.Core.SaveLoad;
using MoveStopMove.Utility.Audio;
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
            //DataPersistenceManager.Instance.SaveGame();
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        public void OnClickTurnOffSound()
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
            SoundManager.Instance.SetMute(true);
        }

        public void OnClickTurnOnSound()
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
            SoundManager.Instance.SetMute(false);
        }

        public void OnClickRestartGame()
        {
            SoundManager.Instance.PlaySFX(ESfxType.ButtonClick);
            UIManager.Instance.CloseUI<UISettingGameplay>();

            EventManager.Instance.Notify(new RestartGame(new Vector3(0,1,0)));
            GameManager.Instance.SpawnEnemy();
        }

        public void OnClickResumeGame()
        {
            SoundManager.Instance.PlaySFX(ESfxType.ButtonClick);
            UIManager.Instance.CloseUI<UISettingGameplay>();
        }

        #endregion
    }
}