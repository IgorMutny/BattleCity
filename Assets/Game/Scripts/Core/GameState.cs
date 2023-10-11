using UnityEngine;

public abstract class GameStateBase
{
    public abstract void Enter();
    public abstract void Exit();
}

public class MainMenuState : GameStateBase
{
    private static GameObject _mainMenuSample;
    private GameObject _mainMenu;

    static MainMenuState()
    {
        _mainMenuSample = Resources.Load<GameObject>("MainMenu");
    }

    public override void Enter()
    {
        PlayerStats.Instance.Load();
        _mainMenu = GameObject.Instantiate(_mainMenuSample);
    }

    public override void Exit()
    {
        GameObject.Destroy(_mainMenu);
    }
}

public class NewGameMenuState : GameStateBase
{
    private static GameObject _newGameMenuSample;
    private GameObject _newGameMenu;

    static NewGameMenuState()
    {
        _newGameMenuSample = Resources.Load<GameObject>("NewGameMenu");
    }

    public override void Enter()
    {
        _newGameMenu = GameObject.Instantiate(_newGameMenuSample);
    }

    public override void Exit()
    {
        GameObject.Destroy(_newGameMenu);
    }
}

public class NewGameState : GameStateBase
{
    public override void Enter()
    {
        PlayerStats.Instance.Clear();
        EventBus.Invoke(new GameStateChangedEvent(new ContinueGameState()));
    }

    public override void Exit() { }
}

public class ContinueGameState : GameStateBase
{
    private static GameObject _levelHeaderSample;
    private GameObject _levelHeader;

    static ContinueGameState()
    {
        _levelHeaderSample = Resources.Load<GameObject>("LevelHeader");
    }
    public override void Enter()
    {
        _levelHeader = GameObject.Instantiate(_levelHeaderSample);
    }

    public override void Exit()
    {
        GameObject.Destroy(_levelHeader);
    }
}

public class BattleState : GameStateBase
{
    private Battle _battle;

    public override void Enter()
    {
        GameObject gameManager = GameObject.FindObjectOfType<EntryPoint>().gameObject;
        _battle = gameManager.AddComponent<Battle>();
        _battle.Init();

        Camera.main.transform.position = new Vector3(10.5f, 12.5f, -10);
    }

    public override void Exit()
    {
        Camera.main.transform.position = new Vector3(13.5f, 12.5f, -10);
        GameObject.Destroy(_battle);
    }
}

public class ScoreScreenState : GameStateBase
{
    private static GameObject _scoreScreenSample;
    private GameObject _scoreScreen;

    static ScoreScreenState()
    {
        _scoreScreenSample = Resources.Load<GameObject>("ScoreScreen");
    }

    public override void Enter()
    {
        _scoreScreen = GameObject.Instantiate(_scoreScreenSample);
    }

    public override void Exit()
    {
        GameObject.Destroy(_scoreScreen);
    }
}

public class GameOverScreenState : GameStateBase
{
    private static GameObject _gameOverScreenSample;
    private GameObject _gameOverScreen;

    static GameOverScreenState()
    {
        _gameOverScreenSample = Resources.Load<GameObject>("GameOverScreen");
    }

    public override void Enter()
    {
        _gameOverScreen = GameObject.Instantiate(_gameOverScreenSample);
    }

    public override void Exit()
    {
        GameObject.Destroy(_gameOverScreen);
    }
}

public class CongratulationsScreenState : GameStateBase
{
    private static GameObject _victoryScreenSample;
    private GameObject _victoryScreen;

    static CongratulationsScreenState()
    {
        _victoryScreenSample = Resources.Load<GameObject>("VictoryScreen");
    }

    public override void Enter()
    {
        _victoryScreen = GameObject.Instantiate(_victoryScreenSample);
    }

    public override void Exit()
    {
        GameObject.Destroy(_victoryScreen);
        PlayerStats.Instance.Clear();
    }
}

public class ExitGameState : GameStateBase
{
    public override void Enter()
    {
        Application.Quit();
    }

    public override void Exit() { }
}