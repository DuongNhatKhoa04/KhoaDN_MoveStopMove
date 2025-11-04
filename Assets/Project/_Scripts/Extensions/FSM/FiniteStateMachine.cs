namespace MoveStopMove.Extensions.FSM
{
    public class FiniteStateMachine
    {
        public State CurrentState { get; private set; }

        public void Initialize(State startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public void ChangeState(State newState)
        {
            if (newState == null || newState == CurrentState) return;
            var prevState = CurrentState;
            prevState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}