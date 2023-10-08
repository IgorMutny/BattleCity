using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    private GameStateMachine _gameStateMachine;
    private PlayerStats _playerStats;

    private void Start()
    {
        Application.targetFrameRate = 60;
        _playerStats = new PlayerStats();
        _gameStateMachine = new GameStateMachine(new MainMenuState());
    }
}
