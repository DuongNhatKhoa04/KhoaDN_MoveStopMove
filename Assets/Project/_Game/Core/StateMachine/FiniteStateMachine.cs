namespace MoveStopMove.Core.StateMachine
{
    public class FiniteStateMachine
    {
        #region -- Properties --

        public State CurrentState { get; private set; }

        #endregion

        #region -- Methods --

        /// <summary>
        /// Initial current state
        /// </summary>
        /// <param name="startingState">Initial state</param>
        public void Initialize(State startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        /// <summary>
        /// Change current state to new state
        /// </summary>
        /// <param name="newState">New state</param>
        public void ChangeState(State newState)
        {
            if (newState == null || newState == CurrentState) return;
            var prevState = CurrentState;
            prevState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        #endregion
    }
}