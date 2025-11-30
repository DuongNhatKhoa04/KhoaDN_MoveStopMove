using MoveStopMove.Core.Stats;
using MoveStopMove.Core.Units;
using MoveStopMove.Presentation.UI;
using MoveStopMove.Presentation.UI.Main;
using MoveStopMove.Utility.Audio;
using MoveStopMove.Utility.Extension;

namespace MoveStopMove.Core.StateMachine.PlayerState
{
    public class PlayerDeadState : PlayerGroundedState
    {
        public PlayerDeadState(Character character, FiniteStateMachine stateMachine, CharacterData playerData, EAnim animation)
            : base(character, stateMachine, playerData, animation) { }

        public override void Enter()
        {
            base.Enter();
            SoundManager.Instance.PlaySFX(ESfxType.PlayerDie);
            UIManager.Instance.OpenUI<UILose>();
        }
    }
}