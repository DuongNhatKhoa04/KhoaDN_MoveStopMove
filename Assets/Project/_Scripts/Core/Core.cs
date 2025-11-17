using MoveStopMove.Core.CoreComponents;
using UnityEngine;

namespace MoveStopMove.Core
{
    public class MainCore : MonoBehaviour
    {
        #region -- Fields --

        [SerializeField] private Movement movement;
        [SerializeField] private Combat combat;

        #endregion

        #region -- Properties --

        public Movement Movement => movement;
        public Combat Combat => combat;

        #endregion
    }
}