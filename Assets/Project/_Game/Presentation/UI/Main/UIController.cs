using MoveStopMove.Core;
using MoveStopMove.Gameplay.Camera;
using UnityEngine;

namespace MoveStopMove.Presentation.UI.Main
{
    public class UIController : UICanvas
    {
        private void OnEnable()
        {
            CameraFollower.Instance.offset = new Vector3(0f, 15f, -20f);
            GameManager.Instance.FindController();
        }
    }
}