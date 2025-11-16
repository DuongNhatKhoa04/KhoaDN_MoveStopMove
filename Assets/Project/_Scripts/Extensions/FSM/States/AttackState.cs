using MoveStopMove.Core;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.SO;
using UnityEngine;

namespace MoveStopMove.Extensions.FSM.States
{
    public class AttackState : PlayerGroundedState
    {
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

                if (Character.HasAnimationLooped(EAnim.Attack, out int loop))
                {
                    Core.Combat.Attack();
                    //Debug.Log("Fired at loop: " + loop);
                }
            }
        }
    }
}