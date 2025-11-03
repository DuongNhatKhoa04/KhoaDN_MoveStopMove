using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace MoveStopMove.Extensions.HSM
{
    public class TransitionSequencer
    {
        #region -- Fields --

        public readonly StateMachine Machine;
        private ISequence m_sequencer;
        private Action m_nextPhase;
        private (State from, State to)? m_pending;
        private State m_lastFrom, m_lastTo;
        private CancellationTokenSource m_tokenSource;
        public readonly bool UseSequential = true; // true is sequence, false is parallel

        #endregion

        #region -- Methods --

        public TransitionSequencer(StateMachine machine)
        {
            Machine = machine;
        }

        private static List<PhaseStep> GatherPhaseStep(List<State> chain, bool deactivate)
        {
            var steps = new List<PhaseStep>();

            for (int i = 0; i < chain.Count; i++)
            {
                var activity = chain[i].Activities;

                for (int j = 0; j < activity.Count; j++)
                {
                    var act = activity[j];

                    if (deactivate)
                    {
                        if (act.Mode == EActivityMode.Active)
                            steps.Add(cancelToken => act.DeactivateAsync(cancelToken));
                    }
                    else
                    {
                        if (act.Mode == EActivityMode.Inactive)
                            steps.Add(cancelToken => act.ActivateAsync(cancelToken));
                    }
                }
            }
            return steps;
        }

        private static List<State> StateToExit(State from, State lca)
        {
            var list = new List<State>();
            for (var s = from; s != null && s != lca; s = s.Parent) list.Add(s);
            return list;
        }

        private static List<State> StateToEnter(State to, State lca)
        {
            var stack = new Stack<State>();
            for (var s = to; s != lca; s = s.Parent) stack.Push(s);
            return new List<State>(stack);
        }

        /// <summary>
        /// Request a transition from one state to another
        /// </summary>
        /// <param name="from">Current state</param>
        /// <param name="to">Next state</param>
        public void RequestTransition(State from, State to)
        {
            //Machine.ChangeState(from, to);

            if (to == null || from == to) return;

            if (m_sequencer == null) return;
            m_pending = (from, to);
            BeginTransition(from, to);
        }

        private void BeginTransition(State from, State to)
        {
            var lca = LowestCommonAncestor(from, to);
            var exitChain = StateToExit(from, lca);
            var enterChain = StateToEnter(from, lca);

            // Deactivate
            var exitSteps = GatherPhaseStep(exitChain, true);
            //m_sequencer = new NoopPhase();
            m_sequencer = UseSequential
                ? new SequentialPhase(exitSteps, m_tokenSource.Token)
                : new ParallelPhase(exitSteps, m_tokenSource.Token);
            m_sequencer.Start();

            m_nextPhase = () =>
            {
                // Change State
                Machine.ChangeState(from, to);

                // Activate
                var enterSteps = GatherPhaseStep(enterChain, false);
                m_sequencer = UseSequential
                    ? new SequentialPhase(enterSteps, m_tokenSource.Token)
                    : new ParallelPhase(enterSteps, m_tokenSource.Token);
                m_sequencer = new NoopPhase();
                m_sequencer.Start();
            };
        }

        private void EndTransition()
        {
            m_sequencer = null;

            if (m_pending.HasValue)
            {
                (State from, State to) request = m_pending.Value;
                m_pending = null;
                BeginTransition(request.from, request.to);
            }
        }

        public void Tick(float deltaTime)
        {
            if (m_sequencer != null)
            {
                if (!m_sequencer.Update()) return;

                if (m_nextPhase != null)
                {
                    var n = m_nextPhase;
                    m_nextPhase = null;
                    n();
                }
                else
                {
                    EndTransition();
                }
                return;
            }

            Machine.InternalTick(deltaTime);
        }

        /// <summary>
        /// Compute the Lowest Common Ancestor of two state
        /// </summary>
        /// <param name="a">State A</param>
        /// <param name="b">State B</param>
        /// <returns></returns>
        public static State LowestCommonAncestor(State a, State b)
        {
            var aParent = new HashSet<State>();
            for (var s = a; s != null; s = s.Parent) aParent.Add(s);

            for (var s = b; s != null; s = s.Parent)
            {
                if (aParent.Contains(s)) return s;
            }

            return null;
        }

        #endregion
    }
}
