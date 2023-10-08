using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public static readonly int MaxEnemiesInMap = 3;
    public static readonly int TotalEnemiesInLevel = 20;

    private GameObject _spawnerSample;

    private List<Spawner> _spawners = new List<Spawner>();
    private List<Vector3> _spawnerPositions = new List<Vector3>()
    {
        new Vector3(0.5f, 24.5f, 0),
        new Vector3(12.5f, 24.5f, 0),
        new Vector3(24.5f, 24.5f, 0),
    };
    private int _currentSpawner = 1;

    private List<Enemy> _enemies = new List<Enemy>();
    private bool _hasPreparedEnemy = false;
    private int[] _tankTypesToSpawnCount;
    private int _remainingEnemiesCount;
    private float _respawnTime;
    private float _respawnTimeCurrent;
    private List<int> _blinkingTanks = new List<int>() { 2, 9, 16 };

    private float _freezeTime = 10;
    private float _freezeTimeCurrent = 0;

    private bool _isFrozen = false;
    private bool _isPaused = false;

    public void Init()
    {
        _spawnerSample = Resources.Load<GameObject>("Spawner");

        EventBus.Subscribe<GamePausedEvent>(GamePausedHandle);
        EventBus.Subscribe<BattleStartedEvent>(BattleStartedHandle);
        EventBus.Subscribe<BattleFinishedEvent>(BattleFinishedHandle);
        EventBus.Subscribe<EnemySpawnedEvent>(EnemySpawnedHandle);
        EventBus.Subscribe<EnemyDestroyedEvent>(EnemyDestroyedHandle);
        EventBus.Subscribe<BonusCollectedEvent>(BonusCollectedHandle);

        AddSpawners();

        int level = PlayerStats.Instance.Level;
        SetSpawnOptions(level);
    }

    private void AddSpawners()
    {
        foreach (Vector3 position in _spawnerPositions)
        {
            Spawner newSpawner = GameObject.Instantiate(
                _spawnerSample, position, Quaternion.identity)
                .GetComponent<Spawner>();
            newSpawner.Init();
            _spawners.Add(newSpawner);
        }
    }

    private void SetSpawnOptions(int level)
    {
        if (level > Map.MaxLevel)
        {
            level = Map.MaxLevel;
        }

        _tankTypesToSpawnCount = MapInfoLoader.Load(level);
        _remainingEnemiesCount = TotalEnemiesInLevel;
        _respawnTime = (float)(190 - 4 * level) / 60;
    }

    private void GamePausedHandle(GamePausedEvent e)
    {
        _isPaused = e.Paused;
    }

    private void BattleStartedHandle(BattleStartedEvent e)
    {
        CreateEnemy();
    }

    private void BattleFinishedHandle(BattleFinishedEvent e)
    {
        foreach (Enemy tank in _enemies)
        {
            tank.BattleFinishedHandle();
            Destroy(tank.gameObject);
        }

        foreach (Spawner spawner in _spawners)
        {
            Destroy(spawner.gameObject);
        }

        _enemies.Clear();
        _spawners.Clear();
    }

    private void EnemySpawnedHandle(EnemySpawnedEvent e)
    {
        _enemies.Add(e.Tank);
        _hasPreparedEnemy = false;

        if (_isFrozen == true)
        {
            e.Tank.Freeze();
        }
    }

    private void EnemyDestroyedHandle(EnemyDestroyedEvent e)
    {
        foreach (Enemy tank in _enemies)
        {
            if (tank == e.Tank)
            {
                _enemies.Remove(tank);
                PlayerStats.Instance.AddDestroyedEnemy(e.Type - 4);
                if (_remainingEnemiesCount == 0 && _enemies.Count == 0)
                {
                    EventBus.Invoke(new VictoryEvent());
                }
                break;
            }
        }
    }

    private void BonusCollectedHandle(BonusCollectedEvent e)
    {
        if (e.Type == BonusType.Grenade)
        {
            while (_enemies.Count > 0)
            {
                _enemies[0].Die(false);
            }
        }

        if (e.Type == BonusType.Clock)
        {
            _isFrozen = true;
            foreach (Enemy enemy in _enemies)
            { 
                enemy.Freeze();
                _freezeTimeCurrent = _freezeTime;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isPaused == false)
        {
            if (_enemies.Count < MaxEnemiesInMap
                && _remainingEnemiesCount > 0
                && _respawnTimeCurrent <= 0
                && _hasPreparedEnemy == false)
            {
                CreateEnemy();
            }

            _respawnTimeCurrent -= Time.fixedDeltaTime;

            if (_isFrozen == true)
            {
                _freezeTimeCurrent -= Time.fixedDeltaTime;
                if (_freezeTimeCurrent <= 0)
                {
                    Unfreeze();
                }
            }
        }
    }

    private void CreateEnemy()
    {
        Spawner spawner = _spawners[_currentSpawner];

        if (spawner.IsBusy() == false && spawner.HasFreeSpace() == true)
        {
            _remainingEnemiesCount -= 1;
            EventBus.Invoke(new RemainingEnemiesChangedEvent(_remainingEnemiesCount));

            bool isBlinking = IsBlinking();

            int tankType = GetAvailableTankType();
            spawner.CreateEnemy(tankType, isBlinking);

            _hasPreparedEnemy = true;
            _respawnTimeCurrent = _respawnTime;
            ChangeCurrentSpawner();
        }
    }

    private bool IsBlinking()
    {
        foreach (int number in _blinkingTanks)
        {
            if (_remainingEnemiesCount == number)
            {
                return true;
            }
        }

        return false;
    }

    private int GetAvailableTankType()
    {
        List<int> availableTankTypes = new List<int>();

        for (int i = 0; i < _tankTypesToSpawnCount.Length; i++)
        {
            if (_tankTypesToSpawnCount[i] > 0)
            {
                availableTankTypes.Add(i);
            }
        }

        int tankType = availableTankTypes[Random.Range(0, availableTankTypes.Count)];
        _tankTypesToSpawnCount[tankType] -= 1;
        return tankType;
    }

    private void ChangeCurrentSpawner()
    {
        _currentSpawner += 1;
        if (_currentSpawner >= _spawners.Count)
        {
            _currentSpawner = 0;
        }
    }

    private void Unfreeze()
    {
        _isFrozen = false;
        foreach (Enemy enemy in _enemies)
        {
            enemy.Unfreeze();
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<GamePausedEvent>(GamePausedHandle);
        EventBus.Unsubscribe<BattleStartedEvent>(BattleStartedHandle);
        EventBus.Unsubscribe<BattleFinishedEvent>(BattleFinishedHandle);
        EventBus.Unsubscribe<EnemySpawnedEvent>(EnemySpawnedHandle);
        EventBus.Unsubscribe<EnemyDestroyedEvent>(EnemyDestroyedHandle);
        EventBus.Unsubscribe<BonusCollectedEvent>(BonusCollectedHandle);
    }
}
