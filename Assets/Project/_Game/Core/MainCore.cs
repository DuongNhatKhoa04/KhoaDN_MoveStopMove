using MoveStopMove.Core.Movement;
using UnityEngine;

namespace MoveStopMove.Core
{
    public class MainCore : MonoBehaviour
    {
        #region -- Fields --

        [SerializeField] private PlayerMovement movement;
        [SerializeField] private Combat.Battle battle;

        #endregion

        #region -- Properties --

        public PlayerMovement Movement => movement;
        public Combat.Battle Battle => battle;

        #endregion
    }
}