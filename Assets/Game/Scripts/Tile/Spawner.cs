using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;
    private SpawnerView _spawnerView;

    private GameObject _playerSample;
    private GameObject _enemySample;

    private Dictionary<SpawnCommand, Delay> _commands = new Dictionary<SpawnCommand, Delay>();
    private float _spawnDelay = 1f;
    private bool _isBusy = false;

    private bool _isPaused = false;

    public void Init()
    {
        EventBus.Subscribe<GamePausedEvent>(GamePausedHandle);

        _playerSample = Resources.Load<GameObject>("Player");
        _enemySample = Resources.Load<GameObject>("Enemy");

        _spawnerView = new SpawnerView(GetComponent<SpriteRenderer>(), _sprites);
        _spawnerView.Disable();
    }

    private void GamePausedHandle(GamePausedEvent e)
    {
        _isPaused = e.Paused;
    }

    public void CreatePlayer(int type)
    {
        _spawnerView.Enable();
        _isBusy = true;

        PlayerSpawnCommand command = new PlayerSpawnCommand(_playerSample, transform, type, false);
        Delay delay = new Delay(_spawnDelay);
        _commands.Add(command, delay);
    }

    public void CreateEnemy(int type, bool isBlinking)
    {
        _spawnerView.Enable();
        _isBusy = true;

        EnemySpawnCommand command = new EnemySpawnCommand(_enemySample, transform, type, isBlinking);
        Delay delay = new Delay(_spawnDelay);
        _commands.Add(command, delay);
    }

    public bool IsBusy()
    {
        return _isBusy;
    }

    public bool HasFreeSpace()
    {
        RaycastHit2D[] results = Physics2D.BoxCastAll(
            transform.position, new Vector2(1.8f, 1.8f), 0, Vector2.zero, 0);

        foreach (RaycastHit2D result in results)
        {
            if (result.collider.GetComponent<Obstacle>() != null)
            {
                return false;
            }
        }

        return true;
    }

    private void Update()
    {
        if (_isPaused == false)
        {
            _spawnerView.Update();
        }
    }

    private void FixedUpdate()
    {
        if (_isPaused == false)
        {
            foreach ((SpawnCommand command, Delay delay) in _commands)
            {
                delay.Time -= Time.fixedDeltaTime;
            }

            if (HasFreeSpace() == true)
            {
                List<SpawnCommand> onDeleting = new List<SpawnCommand>();

                foreach ((SpawnCommand command, Delay delay) in _commands)
                {
                    if (delay.Time <= 0)
                    {
                        command.Execute();
                        _isBusy = false;
                        _spawnerView.Disable();
                        onDeleting.Add(command);
                    }
                }

                foreach (SpawnCommand command in onDeleting)
                {
                    _commands.Remove(command);
                }
            }
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<GamePausedEvent>(GamePausedHandle);
    }

}

