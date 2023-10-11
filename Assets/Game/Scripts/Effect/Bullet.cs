using UnityEngine;

public abstract class Bullet : MonoBehaviour, IObstacle, IDamageableObstacle
{
    [SerializeField] private float _speed;
    [SerializeField] private bool _powered;

    private GameObject _littleExplosionSample;
    protected Transform _transform;
    protected float _size = 0.25f;

    private bool _isPaused = false;
    private bool _isOfPlayer = false;

    public void Init()
    {
        EventBus.Subscribe<GamePausedEvent>(GamePausedHandle);
        _littleExplosionSample = Resources.Load<GameObject>("LittleExplosion");
        _transform = transform;

        _isOfPlayer = GetComponent<PlayerBullet>() != null;
        if (_isOfPlayer == true)
        {
            EventBus.Invoke(new SoundEvent(AudioController.BulletShotKey));
        }
    }

    private void GamePausedHandle(GamePausedEvent e)
    {
        _isPaused = e.Paused;
    }

    private void FixedUpdate()
    {
        if (_isPaused == false)
        {
            bool tankHit = CheckTankHits();
            bool tileHit = CheckTileHits();

            _transform.Translate(Vector3.up * _speed * Time.fixedDeltaTime);

            if (tankHit == true || tileHit == true)
            {
                TakeDamage(Quaternion.identity, false);
            }
        }
    }

    protected abstract bool CheckTankHits();

    private bool CheckTileHits()
    {
        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(
        _transform.position, new Vector2(_size, _size), 0, Vector2.zero, 0);

        bool bulletDestroyed = false;
        bool objectDestroyed = false;

        foreach (RaycastHit2D raycastHit in raycastHits)
        {
            IDamageableObstacle damageable = raycastHit.collider.GetComponent<IDamageableObstacle>();
            if (damageable != null && damageable != (IDamageableObstacle)this)
            {
                objectDestroyed = damageable.TakeDamage(transform.rotation, _powered);
            }

            IObstacle obstacle = raycastHit.collider.GetComponent<IObstacle>();
            bool isTank = raycastHit.collider.GetComponent<Tank>();
            bool isWater = raycastHit.collider.GetComponent<Water>();
            if (obstacle != null && obstacle != (IObstacle)this && isTank == false && isWater == false)
            {
                bulletDestroyed = true;
            }
        }

        if (_isOfPlayer == true && bulletDestroyed == true)
        {
            if (objectDestroyed == true)
            {
                EventBus.Invoke(new SoundEvent(AudioController.BulletDestroyedWithObstacleKey));
            }
            else
            {
                EventBus.Invoke(new SoundEvent(AudioController.BulletDestroyedKey));
            }
        }

        return bulletDestroyed;
    }

    public bool TakeDamage(Quaternion rotation, bool isPowered)
    {
        Instantiate(_littleExplosionSample, _transform.position, Quaternion.identity);
        Destroy(gameObject);

        return true;
    }

    protected virtual void OnDestroy()
    {
        EventBus.Unsubscribe<GamePausedEvent>(GamePausedHandle);
    }
}
