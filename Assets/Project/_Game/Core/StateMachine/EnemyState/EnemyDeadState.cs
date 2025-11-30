using MoveStopMove.Core.Stats;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Extension;

namespace MoveStopMove.Core.StateMachine.EnemyState
{
    public class EnemyDeadState : EnemyGroundedState
    {
        public EnemyDeadState(Character character, FiniteStateMachine stateMachine, CharacterData enemyData, EAnim animation)
            : base(character, stateMachine, enemyData, animation) { }
    }
}