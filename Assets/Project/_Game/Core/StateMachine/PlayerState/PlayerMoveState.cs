using MoveStopMove.Core.Stats;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Extension;

namespace MoveStopMove.Core.StateMachine.PlayerState
{
    public class PlayerMoveState : PlayerGroundedState
    {
        #region -- Methods --

        public PlayerMoveState(Character player, FiniteStateMachine stateMachine, CharacterData playerData, EAnim animation)
            : base(player, stateMachine, playerData, animation) { }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsMoving)
            {
                Core.Movement.Moving(Direction, PlayerData.speed, PlayerData.acceleration);
            }
            else
            {
                StateMachine.ChangeState(Character.CharacterIdleState);
            }
        }

        #endregion
    }
}