using UnityEngine;

public abstract class Bullet : MonoBehaviour
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
                Instantiate(_littleExplosionSample, _transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    protected abstract bool CheckTankHits();

    private bool CheckTileHits()
    {
        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(
        _transform.position, new Vector2(_size, _size), 0, Vector2.zero, 0);

        bool destroyed = false;

        foreach (RaycastHit2D raycastHit in raycastHits)
        {
            if (raycastHit.collider.GetComponent<HeadQuarters>() != null)
            {
                raycastHit.collider.GetComponent<HeadQuarters>().Destroy();
                destroyed = true;
            }

            if (raycastHit.collider.GetComponent<Brick>() != null)
            {
                raycastHit.collider.GetComponent<Brick>().TakeDamage(transform.rotation, _powered);
                if (_isOfPlayer == true)
                {
                    EventBus.Invoke(new SoundEvent(AudioController.BulletDestroyedWithObstacleKey));
                }
                destroyed = true;
            }

            if (raycastHit.collider.GetComponent<Steel>() != null)
            {
                raycastHit.collider.GetComponent<Steel>().TakeDamage(_powered);
                if (_isOfPlayer == true)
                {
                    EventBus.Invoke(new SoundEvent(AudioController.BulletDestroyedKey));
                }
                destroyed = true;
            }

            if (raycastHit.collider.GetComponent<Bullet>() != null && raycastHit.collider.GetComponent<Bullet>() != this)
            {
                if (_isOfPlayer == true)
                {
                    EventBus.Invoke(new SoundEvent(AudioController.BulletDestroyedWithObstacleKey));
                }
                destroyed = true;
            }

            if (raycastHit.collider.GetComponent<Bedrock>() != null)
            {
                if (_isOfPlayer == true)
                {
                    EventBus.Invoke(new SoundEvent(AudioController.BulletDestroyedKey));
                }
                destroyed = true;
            }
        }

        return destroyed;
    }

    protected virtual void OnDestroy()
    {
        EventBus.Unsubscribe<GamePausedEvent>(GamePausedHandle);
    }
}
