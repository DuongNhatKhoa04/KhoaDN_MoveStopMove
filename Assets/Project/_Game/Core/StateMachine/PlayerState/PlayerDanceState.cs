using MoveStopMove.Core.Stats;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Audio;
using MoveStopMove.Utility.Extension;

namespace MoveStopMove.Core.StateMachine.PlayerState
{
    public class PlayerDanceState : PlayerGroundedState
    {
        #region -- Methods --

        public PlayerDanceState(Character character, FiniteStateMachine stateMachine, CharacterData playerData, EAnim animation)
            : base(character, stateMachine, playerData, animation) { }

        public override void Enter()
        {
            base.Enter();
            SoundManager.Instance.PlayLoopSFX(ESfxType.PlayerDance);
        }

        #endregion
    }
}