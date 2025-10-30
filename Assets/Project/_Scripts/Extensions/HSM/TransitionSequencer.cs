using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Extensions
{
    public class TransitionSequencer : MonoBehaviour
    {
        #region -- Fields --

        public readonly StateMachine Machine;

        #endregion

        #region -- Methods --

        public TransitionSequencer(StateMachine machine)
        {
            Machine = machine;
        }

        /// <summary>
        /// Request a transition from one state to another
        /// </summary>
        /// <param name="from">Current state</param>
        /// <param name="to">Next state</param>
        public void RequestTransition(State from, State to)
        {
            Machine.ChangeState(from, to);
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
