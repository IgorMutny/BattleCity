using UnityEngine;

public class Battle : MonoBehaviour
{
    private int _level;
    private int _score;

    private Map _map;
    private HeadQuartersController _headQuartersController;
    private PlayersController _playersController;
    private EnemiesController _enemiesController;
    private BonusesController _bonusesController;
    private UIController _uiController;
    private PopUpController _popUpController;

    private GameObject _inputPanelSample;
    private GameObject _inputPanel;

    private float _scoreScreenDelayTime = 3f;
    private bool _battleFinished = false;
    private bool _isPaused = false;

    private int _extraLifeScore = 20000;

    public void Init()
    {
        EventBus.Subscribe<VictoryEvent>(VictoryHandle);
        EventBus.Subscribe<DefeatEvent>(DefeatHandle);
        EventBus.Subscribe<ScoreAddedEvent>(ScoreAddedHandle);

        _level = PlayerStats.Instance.Level;
        _score = PlayerStats.Instance.Score;

        _map = gameObject.AddComponent<Map>();
        _map.Init(_level);

        _headQuartersController = gameObject.AddComponent<HeadQuartersController>();
        _headQuartersController.Init();

        _playersController = gameObject.AddComponent<PlayersController>();
        _playersController.Init();

        _enemiesController = gameObject.AddComponent<EnemiesController>();
        _enemiesController.Init();

        _bonusesController = gameObject.AddComponent<BonusesController>();
        _bonusesController.Init();

        _uiController = gameObject.AddComponent<UIController>();
        _uiController.Init();

        _popUpController = gameObject.AddComponent<PopUpController>();
        _popUpController.Init();

        _inputPanelSample = Resources.Load<GameObject>("InputPanel");
        _inputPanel = Instantiate(_inputPanelSample, Vector3.zero, Quaternion.identity);

        EventBus.Invoke(new BattleStartedEvent(_level));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isPaused = !_isPaused;
            EventBus.Invoke(new GamePausedEvent(_isPaused));
        }

        if (_battleFinished == true && _scoreScreenDelayTime > 0)
        {
            _scoreScreenDelayTime -= Time.deltaTime;
            if (_scoreScreenDelayTime <= 0)
            {
                FinishBattle();
            }
        }
    }

    private void VictoryHandle(VictoryEvent e)
    {
        SetPlayerStats(true);
        PlayerStats.Instance.SetLastBattleWon(true);
        Invoke(nameof(FinishBattle), _scoreScreenDelayTime);
    }

    private void DefeatHandle(DefeatEvent e)
    {
        SetPlayerStats(false);
        PlayerStats.Instance.SetLastBattleWon(false);
        _battleFinished = true;
    }

    private void ScoreAddedHandle(ScoreAddedEvent e)
    {
        int previousScore = _score;
        _score += e.Score;

        if (_score / _extraLifeScore > previousScore / _extraLifeScore)
        {
            _playersController.AddLife();
            EventBus.Invoke(new SoundEvent(AudioController.ExtraLifeReceivedKey));
        }
    }

    private void FinishBattle()
    {
        EventBus.Invoke(new BattleFinishedEvent());

        Destroy(_playersController);
        Destroy(_enemiesController);
        Destroy(_bonusesController);
        Destroy(_headQuartersController);
        Destroy(_map);
        Destroy(_uiController);
        Destroy(_popUpController);
        Destroy(_inputPanel);

        EventBus.Invoke(new StopSoundEvent(AudioController.EnemyMovingKey));
        EventBus.Invoke(new StopSoundEvent(AudioController.PlayerMovingKey));

        EventBus.Invoke(new GameStateChangedEvent(new ScoreScreenState()));
    }

    private void SetPlayerStats(bool mustBeSaved)
    {
        PlayerStats.Instance.SetScore(_score, mustBeSaved);
        PlayerStats.Instance.SetLevel(_level, mustBeSaved);
        PlayerStats.Instance.SetLives(_playersController.Lives, mustBeSaved);
        PlayerStats.Instance.SetTankType(_playersController.TankType, mustBeSaved);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<VictoryEvent>(VictoryHandle);
        EventBus.Unsubscribe<DefeatEvent>(DefeatHandle);
        EventBus.Unsubscribe<ScoreAddedEvent>(ScoreAddedHandle);
    }
}
