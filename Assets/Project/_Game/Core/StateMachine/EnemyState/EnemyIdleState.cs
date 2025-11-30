using MoveStopMove.Core.Stats;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Core.StateMachine.EnemyState
{
    public class EnemyIdleState : EnemyGroundedState
    {
        #region -- Fields --

        private float m_idleTime;
        private float m_timer;

        #endregion

        #region -- Methods --

        public EnemyIdleState(Character character, FiniteStateMachine stateMachine, CharacterData enemyData, EAnim animation)
            : base(character, stateMachine, enemyData, animation) { }

        public override void Enter()
        {
            base.Enter();
            m_idleTime = Random.Range(0.3f, 1.0f);
            m_timer = 0f;
            EnemyMovement?.Stop();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (HasTargetInRange())
            {
                StateMachine.ChangeState(Character.EnemyAttackState);
                return;
            }

            m_timer += Time.deltaTime;
            if (m_timer >= m_idleTime)
            {
                TargetPosition = GetRandomPointAroundSelf(5f);
                StateMachine.ChangeState(Character.EnemyMoveState);
            }
        }

        #endregion
    }
}