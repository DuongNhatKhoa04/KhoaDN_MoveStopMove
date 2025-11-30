using MoveStopMove.Core.Combat;
using MoveStopMove.Core.Movement;
using UnityEngine;

namespace MoveStopMove.Core
{
    public class MainCore : MonoBehaviour
    {
        #region -- Fields --

        [SerializeField] private PlayerMovement movement;
        [SerializeField] private Battle battle;

        #endregion

        #region -- Properties --

        public PlayerMovement Movement => movement;
        public Battle Battle => battle;

        #endregion
    }
}