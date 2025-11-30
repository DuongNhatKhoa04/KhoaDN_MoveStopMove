using MoveStopMove.Core.SaveLoad;
using TMPro;
using UnityEngine;

namespace MoveStopMove.Presentation.UI.Main
{
    public class UIWin : UICanvas
    {
        [SerializeField] private TextMeshProUGUI scoreText;

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
    }
}