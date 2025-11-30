using MoveStopMove.Core.SaveLoad;
using MoveStopMove.Utility.Audio;
using TMPro;
using UnityEngine;

namespace MoveStopMove.Presentation.UI.Main
{
    public class UILose : UICanvas
    {
        #region -- Fields --

        [SerializeField] private TextMeshProUGUI scoreText;

        #endregion

        #region -- Methods --

        private void OnEnable()
        {
            SoundManager.Instance.PlaySFX(ESfxType.Lose);
            Time.timeScale = 0;
            scoreText.text = DataPersistenceManager.Instance.GameData.coins.ToString();
            DataPersistenceManager.Instance.SaveGame();
        }

        public void OnClickRetryButton()
        {
            Time.timeScale = 1;
            UIManager.Instance.CloseUI<UILose>();
            //GameManager restart
        }

        public void OnClickBackToMenu()
        {
            Time.timeScale = 1;
            UIManager.Instance.CloseUI<UILose>();
            UIManager.Instance.OpenUI<UIMain>();
        }

        #endregion
    }
}