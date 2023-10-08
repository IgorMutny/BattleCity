using System.Collections.Generic;
using UnityEngine;

public class PlayersController : MonoBehaviour
{
    public readonly static int MaxType = 3;
    public readonly static int MaxLives = 99;

    private GameObject _spawnerSample;
    private List<Spawner> _spawners = new List<Spawner>();
    private List<Vector3> _spawnerPositions = new List<Vector3>()
    {
        new Vector3(8.5f, 0.5f, 0),
        new Vector3(16.5f, 0.5f, 0)
    };

    private Player _player;

    private float _bonusHelmetTime = 10;

    public int TankType { get; private set; }
    public int Lives { get; private set; }

    public void Init()
    {
        _spawnerSample = Resources.Load<GameObject>("Spawner");

        EventBus.Subscribe<BattleStartedEvent>(BattleStartedHandle);
        EventBus.Subscribe<BattleFinishedEvent>(BattleFinishedHandle);
        EventBus.Subscribe<PlayerSpawnedEvent>(PlayerSpawnedHandle);
        EventBus.Subscribe<PlayerDestroyedEvent>(PlayerDestroyedHandle);
        EventBus.Subscribe<BonusCollectedEvent>(BonusCollectedHandle);

        AddSpawners();

        Lives = PlayerStats.Instance.Lives;
        TankType = PlayerStats.Instance.TankType;
    }

    public void AddLife()
    {
        Lives += 1;
        EventBus.Invoke(new PlayerLivesChangedEvent(Lives));
    }

    private void BattleStartedHandle(BattleStartedEvent e)
    {
        CreatePlayer(TankType);
        EventBus.Invoke(new PlayerLivesChangedEvent(Lives));
    }

    private void BattleFinishedHandle(BattleFinishedEvent e)
    {
        if (_player != null)
        {
            _player.BattleFinishedHandle();
            Destroy(_player.gameObject);
        }

        foreach (Spawner spawner in _spawners)
        {
            Destroy(spawner.gameObject);
        }
    }

    private void PlayerSpawnedHandle(PlayerSpawnedEvent e)
    {
        _player = e.Tank;
    }

    private void PlayerDestroyedHandle(PlayerDestroyedEvent e)
    {
        Lives -= 1;
        EventBus.Invoke(new PlayerLivesChangedEvent(Lives));

        if (Lives > 0)
        {
            TankType = 0;
            CreatePlayer(TankType);
        }
        else
        {
            EventBus.Invoke(new DefeatEvent());
        }
    }

    private void BonusCollectedHandle(BonusCollectedEvent e)
    {
        if (e.Type == BonusType.Star)
        {
            if (TankType < MaxType)
            {
                TankType += 1;
                _player.Init(TankType, false, true);
            }
        }

        if (e.Type == BonusType.Tank)
        {
            if (Lives < MaxLives)
            {
                AddLife();
            }
        }

        if (e.Type == BonusType.Helmet)
        {
            _player.SetInvulnerability(_bonusHelmetTime);
        }
    }

    private void AddSpawners()
    {
        foreach (Vector3 position in _spawnerPositions)
        {
            Spawner spawner = Instantiate(
                _spawnerSample, position, Quaternion.identity)
                .GetComponent<Spawner>();
            spawner.Init();
            _spawners.Add(spawner);
        }
    }

    private void CreatePlayer(int type)
    {
        if (_spawners[0].HasFreeSpace() == true)
        {
            _spawners[0].CreatePlayer(type);
        }
        else
        {
            _spawners[1].CreatePlayer(type);
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<BattleStartedEvent>(BattleStartedHandle);
        EventBus.Unsubscribe<BattleFinishedEvent>(BattleFinishedHandle);
        EventBus.Unsubscribe<PlayerSpawnedEvent>(PlayerSpawnedHandle);
        EventBus.Unsubscribe<PlayerDestroyedEvent>(PlayerDestroyedHandle);
        EventBus.Unsubscribe<BonusCollectedEvent>(BonusCollectedHandle);
    }
}
