using MoveStopMove.Core.Stats;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Audio;
using MoveStopMove.Utility.Extension;

namespace MoveStopMove.Core.StateMachine.PlayerState
{
    public class PlayerIdleState : PlayerGroundedState
    {
        #region -- Fields --

        private bool m_hasEnemyInRange;

        #endregion

        #region -- Methods --

        public PlayerIdleState(Character character, FiniteStateMachine stateMachine, CharacterData playerData, EAnim animation)
            : base(character, stateMachine, playerData, animation) { }

        public override void Enter()
        {
            base.Enter();
            Core.Movement.Stop();
            SoundManager.Instance.StopLoopSFX();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsMoving)
            {
                StateMachine.ChangeState(Character.PlayerMoveState);
            }

            if (HasTargetInRange() && !IsMoving)
            {
                StateMachine.ChangeState(Character.PlayerAttackState);
            }
        }

        #endregion
    }
}