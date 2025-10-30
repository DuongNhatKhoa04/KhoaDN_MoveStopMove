using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Extensions
{
    public class StateMachine
    {
        #region -- Fields --

        public readonly State Root;
        public readonly TransitionSequencer Sequencer;

        private bool m_started;

        #endregion

        #region -- Methods --

        public StateMachine(State root)
        {
            Root = root;
            Sequencer = new (this);
        }

        public void Start()
        {
            if (m_started) return;

            m_started = true;
            Root.Enter();
        }

        public void Tick(float deltaTime)
        {
            if (!m_started) Start();
            InternalTick(deltaTime);
        }

        public void InternalTick(float deltaTime) => Root.Update(deltaTime);

        /// <summary>
        /// Perform actual switch form current state to next state by exiting up to the shared ancestor, the entering
        /// down to the target
        /// </summary>
        /// <param name="currentState">Current state</param>
        /// <param name="nextState">Target state</param>
        public void ChangeState(State currentState, State nextState)
        {
            if (currentState == nextState || currentState == null || nextState == null) return;

            State lca = TransitionSequencer.LowestCommonAncestor(currentState, nextState);

            for (State s = currentState; s != lca; s = s.Parent) s.Exit();

            var stack = new Stack<State>();
            for (State s = nextState; s != lca; s = s.Parent) stack.Push(s);
            while (stack.Count > 0) stack.Pop().Enter();
        }

        #endregion
    }
}
