public class GameStateMachine
{
    private GameStateBase _currentState;

    public GameStateMachine(GameStateBase state)
    {
        EventBus.Subscribe<GameStateChangedEvent>(SetState);

        _currentState = state;
        _currentState.Enter();
    }

    public void SetState(GameStateChangedEvent e)
    {
        _currentState.Exit();
        _currentState = e.GameState;
        _currentState.Enter();
    }
}

