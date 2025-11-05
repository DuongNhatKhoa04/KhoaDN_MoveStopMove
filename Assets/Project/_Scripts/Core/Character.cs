using MoveStopMove.Extensions.FSM;
using MoveStopMove.Extensions.FSM.States;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.Interfaces;
using MoveStopMove.SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoveStopMove.Core
{
    public abstract class Character : MonoBehaviour, IInitializable
    {
        #region -- Fields --

        [Header("Character Animation")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected EAnim currentAnimation;
        [SerializeField] protected MainCore core;
        [SerializeField] protected CharacterData characterData;

        #endregion

        public MainCore Core => core;
        public FiniteStateMachine StateMachine { get; private set; }
        public PlayerIdleState CharacterIdleState { get; private set; }
        public MoveState CharacterMoveState { get; private set; }
        public AttackState CharacterAttackState {  get; private set; }

        #region -- Methods --

        public virtual void Initialize()
        {
            animator.SetTrigger(AnimHashes.Map[currentAnimation]);

            StateMachine = new FiniteStateMachine();

            CharacterIdleState = new PlayerIdleState(this, StateMachine, characterData, EAnim.Idle);
            CharacterMoveState = new MoveState(this, StateMachine, characterData, EAnim.Run);
            CharacterAttackState = new AttackState(this, StateMachine, characterData, EAnim.Attack);
        }

        public void ChangeAnimation(EAnim animationName, float speed = 1)
        {
            animator.speed = Mathf.Max(0f, speed);

            if (currentAnimation == animationName) return;

            animator.ResetTrigger(AnimHashes.Map[currentAnimation]);
            currentAnimation = animationName;
            animator.SetTrigger(AnimHashes.Map[currentAnimation]);
        }

        public void SetAnimationTrigger(EAnim animationName)
        {
            animator.SetTrigger(AnimHashes.Map[animationName]);
        }

        public void ResetAnimationTrigger(EAnim animationName)
        {
            animator.ResetTrigger(AnimHashes.Map[animationName]);
        }

        #endregion
    }
}