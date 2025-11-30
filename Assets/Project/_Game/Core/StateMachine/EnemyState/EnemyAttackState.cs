using MoveStopMove.Core.Combat;
using MoveStopMove.Core.Stats;
using MoveStopMove.Core.Units;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Core.StateMachine.EnemyState
{
    public class EnemyAttackState : EnemyGroundedState
    {
        #region -- Fields --

        private TargetEntry? m_currentTarget;

        #endregion

        #region -- Methods --

        public EnemyAttackState(Character character, FiniteStateMachine stateMachine, CharacterData enemyData, EAnim animation)
            : base(character, stateMachine, enemyData, animation) { }

        public override void Enter()
        {
            base.Enter();
            EnemyMovement?.Stop();
            m_currentTarget = AttackRange.PeekEntry();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!m_currentTarget.HasValue)
            {
                StateMachine.ChangeState(Character.EnemyIdleState);
                return;
            }

            var entry = m_currentTarget.Value;
            if (entry.Target == null || !entry.Target.activeInHierarchy)
            {
                m_currentTarget = AttackRange.PeekEntry();
                return;
            }

            Vector3 targetPos = AttackRange.GetTargetPosition(entry);
            Vector3 dir = (targetPos - Character.transform.position);
            dir.y = 0;
            if (dir.sqrMagnitude > 0.01f)
            {
                var targetRot = Quaternion.LookRotation(dir);
                Character.transform.rotation = Quaternion.Slerp(
                    Character.transform.rotation, targetRot, 10f * Time.deltaTime);
            }

            if (Character.HasAnimationLooped(EAnim.Attack, out int _))
            {
                Core.Battle.Attack();
            }

            if (!HasTargetInRange())
            {
                StateMachine.ChangeState(Character.EnemyIdleState);
            }
        }

        #endregion
    }
}