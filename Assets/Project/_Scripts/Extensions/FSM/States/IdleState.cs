using MoveStopMove.Core;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.SO;

namespace MoveStopMove.Extensions.FSM.States
{
    public class PlayerIdleState : PlayerGroundedState
    {
        private bool m_hasEnemyInRange;

        public PlayerIdleState(Character character, FiniteStateMachine stateMachine, CharacterData playerData, EAnim animation)
            : base(character, stateMachine, playerData, animation) { }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Nếu người chơi đẩy joystick -> Move ngay
            if (IsMoving)
            {
                StateMachine.ChangeState(Character.CharacterMoveState);
                return;
            }

            // Không có input: tiếp tục đếm
            /*if (countdown > 0f)
            {
                //countdown -= Time.deltaTime;
                // TODO (tuỳ bạn): phát UI 3-2-1 tại đây nếu cần
                return;
            }*/

            // Hết đếm: nếu có mục tiêu trong tầm -> Attack ngay
            if (HasTargetInRange())
            {
                StateMachine.ChangeState(Character.CharacterAttackState);
                return;
            }

            // Nếu hết đếm mà chưa có ai trong tầm:
            // vẫn ở Idle cho đến khi:
            // - có input -> Move, hoặc
            // - có mục tiêu lọt vào tầm -> Attack (kiểm tra lại mỗi frame)
        }

        private bool HasTargetInRange()
        {
            return Core.Combat != null; //&& Core.Combat.HasTargetInRange();
        }
    }
}