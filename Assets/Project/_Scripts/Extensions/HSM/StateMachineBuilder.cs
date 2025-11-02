using System.Collections.Generic;
using System.Reflection;

namespace MoveStopMove.Extensions.HSM
{
    public class StateMachineBuilder
    {
        #region -- Fields --

        private readonly State m_root;

        #endregion

        #region -- Methods --

        public StateMachineBuilder(State root)
        {
            m_root = root;
        }

        public StateMachine Build()
        {
            var machine = new StateMachine(m_root);
            Wire(m_root, machine, new HashSet<State>());
            return machine;
        }

        private void Wire(State currentState, StateMachine machine, HashSet<State> visited)
        {
            if (currentState == null) return;
            if (!visited.Add(currentState)) return;

            var flags = BindingFlags.Instance
                        | BindingFlags.Public
                        | BindingFlags.NonPublic
                        | BindingFlags.FlattenHierarchy;
            var machineField = typeof(State).GetField("m_machine", flags);
            if (machineField != null) machineField.SetValue(currentState, machine);

            foreach (var f in currentState.GetType().GetFields(flags))
            {
                if (!typeof(State).IsAssignableFrom(f.FieldType)) continue;
                if (f.Name == "Parent") continue; // Skip back-edge to parent

                var child = (State) f.GetValue(currentState);
                if (child == null) continue;
                if (!ReferenceEquals(child.Parent, currentState)) continue;

                Wire(child, machine, visited);
            }
        }

        #endregion
    }
}