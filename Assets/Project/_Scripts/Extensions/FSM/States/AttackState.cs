using MoveStopMove.Core;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.MainCharacter;
using MoveStopMove.MainCharacter.Data;
using MoveStopMove.SO;

namespace MoveStopMove.Extensions.FSM.States
{
    public class AttackState : GroundedState
    {
        public AttackState(Character character, FiniteStateMachine stateMachine, CharacterData playerData, EAnim animation)
            : base(character, stateMachine, playerData, animation) { }

        /*public override void DoChecks()
        {
            base.DoChecks();

            //IsGrounded = Core.Movement.IsGrounded();

            if (!IsMoving || !IsGrounded)
            {
                Core.Movement.Stop();
                //base.ChangeAnimation(EAnim.Attack);
                StateMachine.ChangeState(Character.CharacterAttackState);
                return;
            }

            Core.Movement.Moving(Direction, PlayerData.speed, PlayerData.acceleration);
            StateMachine.ChangeState(Character.CharacterMoveState);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            Direction = ControlProvider.Instance.CheckDirection();
            IsMoving = ControlProvider.Instance.IsMoving(Direction);
        }*/
    }
}