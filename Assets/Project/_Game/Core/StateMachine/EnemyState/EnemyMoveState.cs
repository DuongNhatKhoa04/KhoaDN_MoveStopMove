using MoveStopMove.Core.SaveLoad;
using MoveStopMove.Core.Stats;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Audio;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Core.StateMachine.EnemyState
{
    public class EnemyMoveState : EnemyGroundedState
    {
        private CharacterData m_enemyData;

        #region -- Methods --

        public EnemyMoveState(Character enemy, FiniteStateMachine stateMachine, CharacterData enemyData,
            EAnim animation)
            : base(enemy, stateMachine, enemyData, animation)
        {
            m_enemyData = enemyData;
        }

        public override void Enter()
        {
            base.Enter();

            TargetPosition = GetRandomPointAroundSelf(5f);
            //Debug.Log($"[MoveState] TargetPosition = {TargetPosition}");

            EnemyMovement?.MoveTo(TargetPosition, m_enemyData.speed);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (HasTargetInRange())
            {
                EnemyMovement?.Stop();
                StateMachine.ChangeState(Character.EnemyAttackState);
                return;
            }

            if (EnemyMovement != null && EnemyMovement.HasReachedDestination())
            {
                StateMachine.ChangeState(Character.EnemyIdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            EnemyMovement?.Stop();
        }

        #endregion
    }
}