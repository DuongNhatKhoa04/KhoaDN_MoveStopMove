using MoveStopMove.Core.Combat;
using MoveStopMove.Core.Movement;
using MoveStopMove.Core.Stats;
using MoveStopMove.Core.Units;
using MoveStopMove.Core.Units.EnemyCharacter;
using MoveStopMove.Utility.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace MoveStopMove.Core.StateMachine.EnemyState
{
    public class EnemyGroundedState : State
    {
        #region -- Fields --

        protected bool IsGrounded;
        protected Vector3 TargetPosition { get; set; }

        protected EnemyMovement EnemyMovement => Core.EnemyMovement;
        protected AttackRange AttackRange => Core.Battle.GetAttackRange;

        #endregion

        #region -- Methods --

        protected EnemyGroundedState(Character character, FiniteStateMachine stateMachine, CharacterData enemyData, EAnim animation)
            : base(character, stateMachine, enemyData, animation) { }

        protected Vector3 GetRandomPointAroundSelf(float radius)
        {
            Vector3 origin = Character.transform.position;

            Vector2 random2D = Random.insideUnitCircle * radius;

            Vector3 candidate = new Vector3(
                origin.x + random2D.x,
                origin.y,
                origin.z + random2D.y
            );

            NavMeshHit hit;
            bool found = NavMesh.SamplePosition(candidate, out hit, radius, NavMesh.AllAreas);

            if (found)
            {
                return hit.position;
            }

            return origin;
        }

        protected new bool HasTargetInRange()
        {
            return AttackRange.PeekEntry().HasValue;
        }

        #endregion
    }
}