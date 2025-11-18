using MoveStopMove.Core.Movement;
using UnityEngine;

namespace MoveStopMove.Core
{
    public class MainCore : MonoBehaviour
    {
        #region -- Fields --

        [SerializeField] private PlayerMovement movement;
        [SerializeField] private Combat.Combat combat;

        #endregion

        #region -- Properties --

        public PlayerMovement Movement => movement;
        public Combat.Combat Combat => combat;

        #endregion
    }
}