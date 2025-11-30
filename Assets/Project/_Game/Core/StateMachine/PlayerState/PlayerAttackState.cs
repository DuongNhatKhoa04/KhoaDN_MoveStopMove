using MoveStopMove.Core.Combat;
using MoveStopMove.Core.Stats;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Audio;
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
                StateMachine.ChangeState(Character.PlayerMoveState);
            }
            else
            {
                if (!HasTargetInRange())
                {
                    StateMachine.ChangeState(Character.PlayerIdleState);
                }

                var entry = Core.Battle.GetAttackRange.PeekEntry();
                if (entry != null)
                {
                    var targetPos = AttackRange.GetTargetPosition(entry.Value);
                    Core.Battle.RotateTowards(targetPos);
                }

                if (Character.HasAnimationLooped(EAnim.Attack, out int loop))
                {
                    Core.Battle.Attack();
                    SoundManager.Instance.PlaySFX(ESfxType.PlayerAttack);
                }
            }
        }

        #endregion
    }
}