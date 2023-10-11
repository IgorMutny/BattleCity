using System.Collections.Generic;
using UnityEngine;

public abstract class AICommand
{
    protected readonly float _timeToSetTarget = 4f;
    protected readonly float _timeToFindNewPath = 4f;
    protected readonly float _timeToCheckObstaclePosition = 0.5f;
    protected readonly float _timeToCheckTargetPosition = 0.5f;
    protected readonly float _timeToShoot = 0.5f;
    protected readonly float _timeToRepeatShooting = 0.5f;

    public abstract void Execute();
}

public class SetTargetCommand : AICommand
{
    private AIController _tank;

    public SetTargetCommand(AIController tank)
    {
        _tank = tank;
    }

    public override void Execute()
    {
        if (_tank.GetTarget() == null)
        {
            GameObject player = TryFindObjectOfType<Player>();
            GameObject headQuarters = TryFindObjectOfType<HeadQuarters>();
            GameObject target = null;

            int result = Random.Range(0, 4);
            if (result == 0)
            {
                if (headQuarters != null)
                {
                    target = headQuarters;
                }
            }
            else
            {
                if (player != null)
                {
                    target = player;
                }
            }

            _tank.SetTarget(target);
        }

        _tank.AddCommand(new SetTargetCommand(_tank), _timeToSetTarget);
    }

    private GameObject TryFindObjectOfType<T>() where T : Component
    {
        GameObject result = null;

        try
        {
            result = GameObject.FindAnyObjectByType<T>().gameObject;
        }
        catch
        { }

        return result;
    }
}

public class FindPathToTargetCommand : AICommand
{
    private AIController _tank;
    private PathFinder _pathFinder;

    public FindPathToTargetCommand(AIController tank, PathFinder pathFinder)
    {
        _tank = tank;
        _pathFinder = pathFinder;
    }

    public override void Execute()
    {
        _tank.ClearRoute();

        if (_tank.GetTarget() != null)
        {
            Vector2 targetPosition = _tank.GetTarget().transform.position;
            if (targetPosition != AIController.NoTargetPosition)
            {
                _tank.SetRoute(_pathFinder.FindPath(_tank.transform.position, targetPosition));
            }
            else
            {
                _tank.SetRoute(new List<Vector2>() { AIController.NoTargetPosition });
            }
        }

        _tank.AddCommand(new FindPathToTargetCommand(_tank, _pathFinder), _timeToFindNewPath);
    }
}

public class ShootCommand : AICommand
{
    private AIController _tank;

    public ShootCommand(AIController tank)
    {
        _tank = tank;
    }

    public override void Execute()
    {
        if (_tank.HasShotBullets() == false)
        {
            _tank.Shoot();
        }
        else
        {
            _tank.AddCommand(new ShootCommand(_tank), _timeToRepeatShooting);
        }
    }
}

public class CheckObstaclePositionCommand : AICommand
{
    private AIController _tank;

    public CheckObstaclePositionCommand(AIController tank)
    {
        _tank = tank;
    }

    public override void Execute()
    {
        RaycastHit2D[] results = Physics2D.RaycastAll(_tank.transform.position, _tank.GetCurrentDirection(), 1f);
        foreach (RaycastHit2D result in results)
        {
            if (result.collider.GetComponent<Brick>() != null)
            {
                _tank.AddCommand(new ShootCommand(_tank), _timeToShoot);
            }
        }

        _tank.AddCommand(new CheckObstaclePositionCommand(_tank), _timeToCheckObstaclePosition);
    }
}

public class CheckTargetPositionCommand : AICommand
{
    private AIController _tank;

    public CheckTargetPositionCommand(AIController tank)
    {
        _tank = tank;
    }

    public override void Execute()
    {
        CheckDirection(Vector2.up, Map.Width, 0);
        CheckDirection(Vector2.down, Map.Width, 180);
        CheckDirection(Vector2.left, Map.Width, 90);
        CheckDirection(Vector2.right, Map.Width, 270);

        _tank.AddCommand(new CheckTargetPositionCommand(_tank), _timeToCheckTargetPosition);
    }

    private void CheckDirection(Vector2 direction, int maxRange, float movementAngle)
    {
        for (int x = 0; x < maxRange; x++)
        {
            RaycastHit2D[] results = Physics2D.RaycastAll(_tank.transform.position, direction, x);
            foreach (RaycastHit2D result in results)
            {
                if (result.collider.GetComponent<Player>() != null 
                    || result.collider.GetComponent<HeadQuarters>() != null)
                {
                    _tank.ClearRoute();
                    _tank.SetMovement(movementAngle);
                    _tank.AddCommand(new ShootCommand(_tank), _timeToShoot);
                }

                if (result.collider.GetComponent<Obstacle>() != null
                    && result.collider.GetComponent<Water>() == null
                    && result.collider.GetComponent<AIController>() != _tank)
                {
                    return;
                }
            }
        }
    }

}


