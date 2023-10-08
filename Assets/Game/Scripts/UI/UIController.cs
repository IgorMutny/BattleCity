using UnityEngine;

public class UIController: MonoBehaviour
{
    private GameObject _uiLivesSample;
    private GameObject _uiEnemiesSample;
    private GameObject _uiFlagSample;

    private UILives _uiLives;
    private UIEnemies _uiEnemies;
    private UIFlag _uiFlag;

    private Vector3 _uiFlagPosition = new Vector3(27.5f, 3, 0);
    private Vector3 _uiLivesPosition = new Vector3(27.5f, 9.5f, 0);
    private Vector3 _uiEnemiesPosition = new Vector3(27, 24, 0);

    public void Init()
    {
        _uiLivesSample = Resources.Load<GameObject>("UILives");
        _uiEnemiesSample = Resources.Load<GameObject>("UIEnemies");
        _uiFlagSample = Resources.Load<GameObject>("UIFlag");

        EventBus.Subscribe<PlayerLivesChangedEvent>(SetPlayerLives);
        EventBus.Subscribe<RemainingEnemiesChangedEvent>(SetRemainingEnemies);
        EventBus.Subscribe<BattleStartedEvent>(BattleStartedHandle);
        EventBus.Subscribe<BattleFinishedEvent>(BattleFinishedHandle);
        
        _uiFlag = GameObject.Instantiate(
            _uiFlagSample, _uiFlagPosition, Quaternion.identity)
            .GetComponent<UIFlag>();
        _uiFlag.Init();

        _uiEnemies = GameObject.Instantiate(
            _uiEnemiesSample, _uiEnemiesPosition, Quaternion.identity)
            .GetComponent<UIEnemies>();
        _uiEnemies.Init();

        _uiLives = GameObject.Instantiate(
            _uiLivesSample, _uiLivesPosition, Quaternion.identity)
            .GetComponent<UILives>();
        _uiLives.Init();
    }

    private void SetPlayerLives(PlayerLivesChangedEvent e)
    {
         _uiLives.SetLives(e.Lives);
    }

    private void SetRemainingEnemies(RemainingEnemiesChangedEvent e)
    {
        _uiEnemies.SetEnemies(e.RemainingEnemies);
    }

    private void BattleStartedHandle(BattleStartedEvent e)
    {
        _uiFlag.SetLevelNumber(e.Level);
    }

    private void BattleFinishedHandle(BattleFinishedEvent e)
    {
        _uiFlag.BattleFinishedHandle();
        Destroy(_uiFlag.gameObject);

        _uiLives.BattleFinishedHandle();
        Destroy(_uiLives.gameObject);

        _uiEnemies.BattleFinishedHandle();
        Destroy(_uiEnemies.gameObject);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<PlayerLivesChangedEvent>(SetPlayerLives);
        EventBus.Unsubscribe<RemainingEnemiesChangedEvent>(SetRemainingEnemies);
        EventBus.Unsubscribe<BattleStartedEvent>(BattleStartedHandle);
        EventBus.Unsubscribe<BattleFinishedEvent>(BattleFinishedHandle);
    }
}
