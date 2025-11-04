using MoveStopMove.Core;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.MainCharacter.Data;
using MoveStopMove.SO;
using UnityEngine;

namespace MoveStopMove.Extensions.FSM
{
    public abstract class State
    {
        protected MainCore Core;

        protected Character Character;
        protected FiniteStateMachine StateMachine;
        protected CharacterData PlayerData;

        protected bool IsAnimationFinished;
        protected bool IsExitingState;

        protected float StartTime;

        private EAnim m_animation;

        public State(Character character, FiniteStateMachine stateMachine, CharacterData playerData, EAnim animation)
        {
            this.Character = character;
            this.StateMachine = stateMachine;
            this.PlayerData = playerData;
            this.m_animation = animation;
            Core = character.Core;
        }

        public virtual void Enter()
        {
            DoChecks();
            Character.SetAnimationTrigger(m_animation);
            StartTime = Time.time;
            //Debug.Log(animBoolName);
            IsAnimationFinished = false;
            IsExitingState = false;
        }

        public virtual void Exit()
        {
            Character.ResetAnimationTrigger(m_animation);
            //Player.Anim.SetBool(animBoolName, false);
            IsExitingState = true;
        }

        public virtual void LogicUpdate()
        {

        }

        public virtual void PhysicsUpdate()
        {
            DoChecks();
        }

        public virtual void DoChecks() { }

        public virtual void AnimationTrigger() { }

        public virtual void AnimationFinishTrigger() => IsAnimationFinished = true;
    }
}