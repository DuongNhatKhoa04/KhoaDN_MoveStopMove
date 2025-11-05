using MoveStopMove.Core;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.SO;

namespace MoveStopMove.Extensions.FSM.States
{
    public class MoveState : PlayerGroundedState
    {
        public MoveState(Character player, FiniteStateMachine stateMachine, CharacterData playerData, EAnim animation)
            : base(player, stateMachine, playerData, animation) { }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsMoving)
            {
                // Tiếp tục di chuyển
                TickMovement();
                return;
            }

            // Thả joystick: dừng lại và chuyển ngay sang AttackState
            Core.Movement.Stop();
            StateMachine.ChangeState(Character.CharacterAttackState);
        }
    }
}