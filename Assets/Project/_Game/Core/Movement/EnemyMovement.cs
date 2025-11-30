using MoveStopMove.Core.Units;
using UnityEngine;
using UnityEngine.AI;

namespace MoveStopMove.Core.Movement
{
    public class EnemyMovement : CoreComponents
    {
        [SerializeField] private NavMeshAgent agent;

        private void Awake()
        {
            if (agent == null)
                agent = GetComponent<NavMeshAgent>();

            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        public void MoveTo(Vector3 destination, float speed)
        {
            if (agent == null) return;

            agent.speed = speed;
            agent.isStopped = false;
            agent.SetDestination(destination);
        }

        public void Stop()
        {
            if (agent == null) return;

            agent.isStopped = true;
            agent.ResetPath();
        }

        public bool HasReachedDestination(float stopDistance = 0.1f)
        {
            if (agent == null) return true;
            if (agent.pathPending) return false;

            if (agent.remainingDistance > stopDistance) return false;
            return !agent.hasPath || agent.velocity.sqrMagnitude <= 0.01f;
        }

        public bool IsMoving()
        {
            if (agent == null) return false;
            return !agent.isStopped && agent.velocity.sqrMagnitude > 0.001f;
        }
    }
}