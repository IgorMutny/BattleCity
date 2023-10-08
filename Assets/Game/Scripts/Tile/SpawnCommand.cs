using UnityEngine;

public abstract class SpawnCommand
{
    protected GameObject _sample;
    protected Transform _transform;
    protected int _type;
    protected bool _isBlinking;

    public abstract void Execute();
}

public class PlayerSpawnCommand : SpawnCommand
{
    private float _respawnInvulnerabilityTime = 3f;

    public PlayerSpawnCommand(GameObject sample, Transform transform, int type, bool isBlinking)
    {
        _sample = sample;
        _transform = transform;
        _type = type;
        _isBlinking = isBlinking;
    }

    public override void Execute()
    {
        Player newPlayer = GameObject.Instantiate(
            _sample, _transform.position, Quaternion.identity).GetComponent<Player>();
        newPlayer.Init(_type, false, true);
        newPlayer.SetInvulnerability(_respawnInvulnerabilityTime);
        EventBus.Invoke(new PlayerSpawnedEvent(newPlayer));
    }
}

public class EnemySpawnCommand : SpawnCommand
{
    public EnemySpawnCommand(GameObject sample, Transform transform, int type, bool isBlinking)
    {
        _sample = sample;
        _transform = transform;
        _type = type;
        _isBlinking = isBlinking;
    }

    public override void Execute()
    {
        Enemy newEnemy = GameObject.Instantiate(
            _sample, _transform.position, Quaternion.identity).GetComponent<Enemy>();
        newEnemy.Init(_type + 4, _isBlinking, false);
        EventBus.Invoke(new EnemySpawnedEvent(newEnemy));
    }
}