using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Utility.Input
{
    public class ControlProvider : Singleton<ControlProvider>
    {
        #region -- Fields --

        [SerializeField] private FixedJoystick fixedJoystick;

        #endregion

        #region -- Methods --

        /// <summary>
        /// Check direction of joystick input
        /// </summary>
        /// <returns>Vector3</returns>
        public Vector3 CheckDirection()
        {
            return new Vector3(fixedJoystick.Horizontal, 0, fixedJoystick.Vertical);
        }

        /// <summary>
        /// Check joystick input is moving or not
        /// </summary>
        /// <param name="direction">Direction input</param>
        /// <returns>Bool</returns>
        public bool IsMoving(Vector3 direction)
        {
            return direction.sqrMagnitude > 1e-4f;
        }

        #endregion
    }
}