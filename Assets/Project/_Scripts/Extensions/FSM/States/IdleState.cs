using MoveStopMove.Core;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.SO;
using UnityEngine;

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
            Core.Movement.Stop();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsMoving)
            {
                StateMachine.ChangeState(Character.CharacterMoveState);
            }

            if (HasTargetInRange() && !IsMoving)
            {
                StateMachine.ChangeState(Character.CharacterAttackState);
            }
        }
    }
}