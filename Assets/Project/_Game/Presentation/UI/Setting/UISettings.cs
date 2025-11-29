using UnityEngine;

namespace MoveStopMove.Presentation.UI.Setting
{
    public class UISettings : UICanvas
    {
        private void OnEnable()
        {
            Time.timeScale = 0;
        }
    }
}