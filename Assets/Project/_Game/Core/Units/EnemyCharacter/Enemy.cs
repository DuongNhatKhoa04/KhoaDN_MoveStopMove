using MoveStopMove.Core.StateMachine.EnemyState;
using MoveStopMove.Utility.Extension;
using UnityEngine.AI;

namespace MoveStopMove.Core.Units.EnemyCharacter
{
    public class Enemy : Character
    {
        public NavMeshAgent Agent { get; private set; }

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            base.Initialize();
        }

        public override void InitStateMachine()
        {
            base.InitStateMachine();

            EnemyIdleState = new EnemyIdleState(this, StateMachine, characterData, EAnim.Idle);
            EnemyMoveState = new EnemyMoveState(this, StateMachine, characterData, EAnim.Run);
            EnemyAttackState = new EnemyAttackState(this, StateMachine, characterData, EAnim.Attack);
            EnemyDeadState = new EnemyDeadState(this, StateMachine, characterData, EAnim.Dead);
        }

        private void Start()
        {
            StateMachine.Initialize(EnemyIdleState);
        }

        private void Update()
        {
            StateMachine.CurrentState.LogicUpdate();
        }
    }
}