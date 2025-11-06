using MoveStopMove.Core;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.SO;
using UnityEngine;

namespace MoveStopMove.Extensions.FSM.States
{
    public class MoveState : PlayerGroundedState
    {
        public MoveState(Character player, FiniteStateMachine stateMachine, CharacterData playerData, EAnim animation)
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
    }
}