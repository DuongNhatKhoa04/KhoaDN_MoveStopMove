using MoveStopMove.Core;
using MoveStopMove.Core.CoreComponents;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.SO;

namespace MoveStopMove.Extensions.FSM.States
{
    public class AttackState : PlayerGroundedState
    {
        #region -- Methods --

        public AttackState(Character character, FiniteStateMachine stateMachine, CharacterData playerData, EAnim animation)
            : base(character, stateMachine, playerData, animation) { }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsMoving)
            {
                StateMachine.ChangeState(Character.CharacterMoveState);
            }
            else
            {
                if (!HasTargetInRange())
                {
                    StateMachine.ChangeState(Character.CharacterIdleState);
                }

                var entry = Core.Combat.GetAttackRange.PeekEntry();
                if (entry != null)
                {
                    var targetPos = AttackRange.GetTargetPosition(entry.Value);
                    Core.Combat.RotateTowards(targetPos);
                }

                if (Character.HasAnimationLooped(EAnim.Attack, out int loop))
                {
                    Core.Combat.Attack();
                    //Debug.Log("Fired at loop: " + loop);
                }
            }
        }

        #endregion
    }
}