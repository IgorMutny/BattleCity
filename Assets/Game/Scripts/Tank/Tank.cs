using System.Collections.Generic;
using UnityEngine;

public abstract class Tank : MonoBehaviour, IObstacle
{
    [SerializeField] private List<TankSO> _tankTypes;

    protected int _type;
    private List<Sprite> _sprites;
    private Color _defaultColor;
    private float _animationTime;
    private int _health;
    private float _speed;
    private GameObject _bulletSample;
    private int _bulletsPerShot;
    private int _scoreForDestruction;

    private TankShooting _tankShooting;
    private TankMoving _tankMoving;
    private TankInvulnerability _tankInvulnerability;
    private TankView _tankView;
    private bool _isBlinking;
    private bool _canCollectBonus;
    private bool _isFrozen = false;
    private bool _isPlayer = false;

    public bool IsPaused { get; private set; }

    private GameObject _bigExplosionSample;
    private Color[] _colorsByHealth = new Color[4]
    {
        Color.white,
        Color.white,
        Color.yellow,
        Color.green
    };

    public virtual void Init(int type, bool isBlinking, bool canCollectBonus)
    {
        foreach (TankSO tankType in _tankTypes)
        {
            if (tankType.Type == type)
            {
                _type = tankType.Type;
                _sprites = tankType.Sprites;
                _defaultColor = tankType.DefaultColor;
                _animationTime = tankType.AnimationTime;
                _health = tankType.Health;
                _speed = tankType.Speed;
                _bulletSample = tankType.BulletSample;
                _bulletsPerShot = tankType.BulletsPerShot;
                _scoreForDestruction = tankType.ScoreForDestruction;
            }
        }

        _isBlinking = isBlinking;
        _canCollectBonus = canCollectBonus;

        EventBus.Subscribe<GamePausedEvent>(GamePausedHandle);
        _isPlayer = GetComponent<Player>() != null;

        _tankShooting = new TankShooting(transform, _bulletSample, _bulletsPerShot);
        _tankMoving = new TankMoving(transform, _speed, _isPlayer);
        _tankInvulnerability = new TankInvulnerability(transform);
        _tankView = new TankView(GetComponent<SpriteRenderer>(), _defaultColor, _sprites, _animationTime, _isBlinking);

        _bigExplosionSample = Resources.Load<GameObject>("BigExplosion");

        IsPaused = false;
    }

    private void GamePausedHandle(GamePausedEvent e)
    {
        IsPaused = e.Paused;
    }

    private void FixedUpdate()
    {
        if (IsPaused == false && _isFrozen == false)
        {
            _tankShooting.FixedUpdate();
            _tankMoving.FixedUpdate();
            _tankInvulnerability.FixedUpdate();
            if (_canCollectBonus)
            {
                TryCollectBonus();
            }
        }
    }

    private void Update()
    {
        if (IsPaused == false && _isFrozen == false)
        {
            _tankView.SetMovement(_tankMoving.IsMoving());
            _tankView.Update();
        }
    }

    public void SetMovement(float direction)
    {
        _tankMoving.SetMovement(direction);
    }

    public Vector2 GetCurrentDirection()
    {
        return _tankMoving.GetCurrentDirection();
    }

    public void TryShoot()
    {
        _tankShooting.TryShoot();
    }

    public bool HasShotBullets()
    {
        return _tankShooting.HasShotBullets();
    }

    public void SetInvulnerability(float time)
    {
        _tankInvulnerability.SetInvulnerability(time);
    }

    public void Freeze()
    {
        _isFrozen = true;
    }

    public void Unfreeze()
    {
        _isFrozen = false;
    }

    public virtual void TakeDamage()
    {
        if (_tankInvulnerability.IsInvulnerable() == false)
        {
            if (_isBlinking == true)
            {
                EventBus.Invoke(new BlinkingEnemyDamagedEvent());
            }

            _health -= 1;

            if (_health > 0)
            {
                EventBus.Invoke(new SoundEvent(AudioController.HeavyTankDamagedKey));
            }

            _tankView.SetColor(_colorsByHealth[_health]);
            if (_health <= 0)
            {
                Die(true);
            }
        }
    }

    public virtual void Die(bool isScoreAdded)
    {
        if (isScoreAdded == true)
        {
            EventBus.Invoke(new ScoreAddedEvent(_scoreForDestruction, transform.position));
        }

        Instantiate(_bigExplosionSample, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void BattleFinishedHandle()
    {
        _tankShooting.DestroyAllBullets();
    }

    private void TryCollectBonus()
    {
        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(transform.position, new Vector2(2, 2), 0, Vector2.zero, 0);

        foreach (RaycastHit2D raycastHit in raycastHits)
        {
            if (raycastHit.collider.GetComponent<Bonus>() != null)
            {
                raycastHit.collider.GetComponent<Bonus>().Collect();
            }
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<GamePausedEvent>(GamePausedHandle);
    }
}
