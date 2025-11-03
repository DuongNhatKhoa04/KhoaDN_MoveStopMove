using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Extensions.HSM
{
    public abstract class State
    {
        #region -- Fields --

        public readonly StateMachine Machine;
        public readonly State Parent;

        public State ActiveChild;

        private readonly List<IActivity> m_activities = new();
        public IReadOnlyList<IActivity> Activities => m_activities;

        #endregion

        #region -- Methods --

        public State(StateMachine machine, State parent)
        {
            Machine = machine;
            Parent = parent;
        }

        public void Add(IActivity activity)
        {
            if (activity != null)
                m_activities.Add(activity);
        }

        /// <summary>
        /// Initial child to enter when this state starts
        /// </summary>
        /// <returns><c>null</c>: This is the leaf</returns>
        protected virtual State GetInitialState() => null;

        /// <summary>
        /// Target state to switch to this frame
        /// </summary>
        /// <returns><c>null</c>: Staying in current state</returns>
        protected virtual State GetTransition() => null;

        #region - Lifecycle Methods -
        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }
        protected virtual void OnUpdate(float deltaTime) { }
        #endregion

        public void Enter()
        {
            if (Parent != null) Parent.ActiveChild = this;
            OnEnter();

            State initState = GetInitialState();
            initState?.Enter();
        }

        public void Exit()
        {
            ActiveChild?.Exit();
            ActiveChild = null;
            OnExit();
        }

        public void Update(float deltaTime)
        {
            State nextState = GetTransition();
            if (nextState != null)
            {
                Machine.Sequencer.RequestTransition(this, nextState);
                return;
            }

            ActiveChild?.Update(deltaTime);
            OnUpdate(deltaTime);
        }

        /// <summary>
        /// Return the deepest currently-active descendant state
        /// </summary>
        /// <returns>The leaf of the active path</returns>
        public State Leaf()
        {
            State currentState = this;
            while (currentState.ActiveChild != null) currentState = currentState.ActiveChild;
            return currentState;
        }

        /// <summary>
        /// Yield this state and then each ancestor up to the root
        /// </summary>
        /// <returns>Self -> Parent -> ... -> Root</returns>
        public IEnumerable<State> PathToRoot()
        {
            for (State s = this; s != null; s = s.Parent) yield return s;
        }

        #endregion
    }
}
