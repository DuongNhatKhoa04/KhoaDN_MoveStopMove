using MoveStopMove.Core;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.MainCharacter;
using MoveStopMove.MainCharacter.Data;
using MoveStopMove.SO;
using UnityEditor;
using UnityEngine;

namespace MoveStopMove.Extensions.FSM.States
{
    public class GroundedState : State
    {
        protected Vector3 Direction;
        protected bool IsGrounded;
        protected bool IsMoving;

        public GroundedState(Character character, FiniteStateMachine stateMachine, CharacterData playerData, EAnim animation)
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

        protected void TickMovement()
        {
            if (!IsGrounded) return;

            if (IsMoving)
            {
                Core.Movement.Moving(Direction, PlayerData.speed, PlayerData.acceleration);
            }
            else
            {
                Core.Movement.Stop();
            }
        }
    }
}