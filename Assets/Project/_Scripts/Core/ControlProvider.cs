using MoveStopMove.Extensions.Singleton;
using UnityEngine;

namespace MoveStopMove.Core
{
    public class ControlProvider : Singleton<ControlProvider>
    {
        [SerializeField] private FixedJoystick fixedJoystick;

        public Vector3 CheckDirection()
        {
            return new Vector3(fixedJoystick.Horizontal, 0, fixedJoystick.Vertical);
        }

        public bool IsMoving(Vector3 direction)
        {
            return direction.sqrMagnitude > 1e-4f;
        }
    }
}