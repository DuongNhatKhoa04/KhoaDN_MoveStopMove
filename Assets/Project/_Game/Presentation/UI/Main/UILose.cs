using MoveStopMove.Core.SaveLoad;
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
            Time.timeScale = 0;
            scoreText.text = DataPersistenceManager.Instance.GameData.kills.ToString();
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