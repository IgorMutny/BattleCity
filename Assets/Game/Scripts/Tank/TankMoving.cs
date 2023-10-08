using UnityEngine;

public class TankMoving
{
    private Transform _transform;
    private float _speed;
    private bool _isPlayer;

    private Quaternion _rotationPrepared;
    private float _movementPrepared;
    private bool _canMove = false;

    private bool _isMoving = false;
    private bool _isOnIce = false;
    private float _slideTimeCurrent;
    private float _slideTime = 0.45f;

    public TankMoving(Transform transform, float speed, bool isPlayer)
    {
        _transform = transform;
        _speed = speed;
        _isPlayer = isPlayer;
    }

    public void FixedUpdate()
    {
        TryCorrectPosition();
        TryChangeDirection();
        CheckWay();
        CheckIce();
        TrySlide();
        TryMove();
    }

    public void SetMovement(float direction)
    {
        _rotationPrepared = Quaternion.Euler(0, 0, direction);
        _movementPrepared = _speed * Time.fixedDeltaTime;
    }

    public bool IsMoving()
    {
        return _canMove == true && _movementPrepared > 0;
    }

    private void TryCorrectPosition()
    {
        float x = _transform.position.x;
        float y = _transform.position.y;
        float direction = Mathf.Repeat(_transform.rotation.eulerAngles.z, 360f);

        if (direction == 0 || direction == 180)
        {
            x = Mathf.Floor(_transform.position.x) + 0.5f;
        }
        if (direction == 90 || direction == 270)
        {
            y = Mathf.Floor(_transform.position.y) + 0.5f;
        }

        _transform.position = new Vector3(x, y, 0);
    }

    private void TryChangeDirection()
    {
        if (_isPlayer == true && _transform.rotation != _rotationPrepared && _isOnIce == true)
        {
            EventBus.Invoke(new SoundEvent(AudioController.IceRotationKey));
        }

        _transform.rotation = _rotationPrepared;
    }

    private void CheckWay()
    {
        _canMove = IsWayFree();
    }

    private bool IsWayFree()
    {
        RaycastHit2D[] results = Physics2D.RaycastAll(_transform.position, GetCurrentDirection(), 1f);
        foreach (RaycastHit2D result in results)
        {
            if (result.collider.GetComponent<Obstacle>() != null && result.collider.transform != _transform)
            {
                return false;
            }
        }

        return true;
    }

    public Vector2 GetCurrentDirection()
    {
        float rotation = Mathf.Repeat(_transform.rotation.eulerAngles.z, 360f);

        Vector2 direction = Vector2.zero;
        switch (rotation)
        {
            case 0: direction = new Vector2(0, 2); break;
            case 180: direction = new Vector2(0, -2); break;
            case 90: direction = new Vector2(-2, 0); break;
            case 270: direction = new Vector2(2, 0); break;
        }

        return direction;
    }

    private void CheckIce()
    {
        _isOnIce = IsOnIce();
    }

    private bool IsOnIce()
    {
        RaycastHit2D[] results = Physics2D.BoxCastAll(_transform.position, new Vector2(1, 1), 0, Vector2.zero, 0);
        foreach (RaycastHit2D result in results)
        {
            if (result.collider.GetComponent<Ice>() != null)
            {
                return true;
            }
        }

        return false;
    }

    private void TrySlide()
    {
        if (_canMove == true && _movementPrepared == 0 && _slideTimeCurrent > 0 && _isOnIce)
        {
            _transform.Translate(Vector3.up * _speed * Time.fixedDeltaTime);
            _slideTimeCurrent -= Time.fixedDeltaTime;
        }
    }

    private void TryMove()
    {
        bool isMovingBefore = _isMoving;

        if (_canMove == true && _movementPrepared > 0)
        {
            _transform.Translate(Vector3.up * _movementPrepared);
            _movementPrepared = 0;

            if (_isOnIce == true)
            {
                _slideTimeCurrent = _slideTime;
            }

            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }

        TryPlaySound(isMovingBefore);
    }

    private void TryPlaySound(bool isMovingBefore)
    {
        if (_isPlayer == true)
        {
            if (isMovingBefore == false && _isMoving == true)
            {
                EventBus.Invoke(new SoundEvent(AudioController.PlayerMovingKey));
            }

            if (isMovingBefore == true && _isMoving == false)
            {
                EventBus.Invoke(new StopSoundEvent(AudioController.PlayerMovingKey));
            }
        }
        else
        {
            if (isMovingBefore == false && _isMoving == true)
            {
                EventBus.Invoke(new SoundEvent(AudioController.EnemyMovingKey));
            }

            if (isMovingBefore == true && _isMoving == false)
            {
                EventBus.Invoke(new StopSoundEvent(AudioController.EnemyMovingKey));
            }
        }
    }
}
