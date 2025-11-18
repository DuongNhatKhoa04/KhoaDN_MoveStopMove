using MoveStopMove.Core.Stats;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Extension;
using MoveStopMove.Utility.Input;
using UnityEngine;

namespace MoveStopMove.Core.StateMachine.PlayerState
{
    public class PlayerGroundedState : State
    {
        #region -- Fields --

        protected Vector3 Direction;
        protected bool IsGrounded;
        protected bool IsMoving;

        #endregion

        #region -- Methods --

        public PlayerGroundedState(Character character, FiniteStateMachine stateMachine, CharacterData playerData, EAnim animation)
            : base(character, stateMachine, playerData, animation) { }

        public override void DoChecks()
        {
            base.DoChecks();

            IsGrounded = Core.Movement.IsGrounded();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Direction = ControlProvider.Instance.CheckDirection();
            IsMoving = ControlProvider.Instance.IsMoving(Direction);
        }

        #endregion
    }
}