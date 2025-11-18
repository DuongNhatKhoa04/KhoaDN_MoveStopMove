using MoveStopMove.Core.Combat;
using MoveStopMove.Core.Stats;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Extension;

namespace MoveStopMove.Core.StateMachine.PlayerState
{
    public class PlayerAttackState : PlayerGroundedState
    {
        #region -- Methods --

        public PlayerAttackState(Character character, FiniteStateMachine stateMachine, CharacterData playerData, EAnim animation)
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