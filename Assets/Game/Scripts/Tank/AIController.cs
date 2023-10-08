using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static readonly Vector2 NoTargetPosition = new Vector2(-1, -1);

    private Enemy _tank;
    private Map _map;
    private PathFinder _pathFinder;

    private Dictionary<AICommand, Delay> _commands = new Dictionary<AICommand, Delay>();
    private Dictionary<AICommand, Delay> _commandsOnAdding = new Dictionary<AICommand, Delay>();

    private GameObject _target;
    private List<Vector2> _route = new List<Vector2>();

    private void Start()
    {
        _tank = GetComponent<Enemy>();

        _map = FindObjectOfType<Map>();
        _pathFinder = new PathFinder(_map);

        AddCommand(new SetTargetCommand(this), 0.1f);
        AddCommand(new FindPathToTargetCommand(this, _pathFinder), 0.5f);
        AddCommand(new CheckObstaclePositionCommand(this), 1f);
        AddCommand(new CheckTargetPositionCommand(this), 1f);
    }

    private void FixedUpdate()
    {
        if (_tank.IsPaused == false)
        {
            HandleCommandDictionary();
            HandleRoute();
        }
    }

    private void HandleCommandDictionary()
    {
        List<AICommand> onDeleting = new List<AICommand>();

        foreach ((AICommand command, Delay delay) in _commands)
        {
            delay.Time -= Time.fixedDeltaTime;
            if (delay.Time <= 0)
            {
                command.Execute();
                onDeleting.Add(command);
            }
        }

        foreach (AICommand command in onDeleting)
        {
            _commands.Remove(command);
        }

        foreach((AICommand command, Delay delay) in _commandsOnAdding)
        {
            _commands.Add(command, delay);
        }
        _commandsOnAdding.Clear();
    }

    private void HandleRoute()
    {
        if (_route.Count == 0)
        {
            return;
        }

        Vector2 position = GetRoundedPosition();
        Vector2 currentPoint = _route[0];

        if (position == currentPoint)
        {
            _route.RemoveAt(0);
        }
        else
        {
            float direction = 180;
            if (position.x > currentPoint.x) direction = 90;
            if (position.x < currentPoint.x) direction = 270;
            if (position.y > currentPoint.y) direction = 180;
            if (position.y < currentPoint.y) direction = 0;
            _tank.SetMovement(direction);
        }
    }

    private Vector2 GetRoundedPosition()
    {
        Vector2 position = transform.position;
        float x = Mathf.Floor(position.x) + 0.5f;
        float y = Mathf.Floor(position.y) + 0.5f;
        return new Vector2(x, y);
    }

    public void AddCommand(AICommand command, float delay)
    {
        _commandsOnAdding.Add(command, new Delay(delay));
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    public GameObject GetTarget()
    {
        return _target;
    }

    public void SetRoute(List<Vector2> route)
    {
        _route = route;
    }

    public void ClearRoute()
    {
        _route.Clear();
    }

    public void Shoot()
    {
        _tank.TryShoot();
    }

    public bool HasShotBullets()
    {
        return _tank.HasShotBullets();
    }

    public void SetMovement(float direction)
    {
       _tank.SetMovement(direction);
    }

    public Vector2 GetCurrentDirection()
    {
        return _tank.GetCurrentDirection();
    }
}