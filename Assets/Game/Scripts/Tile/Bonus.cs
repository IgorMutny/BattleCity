using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private BonusType _type;

    private SpriteRenderer _spriteRenderer;
    private float _blinkTime = 0.1f;
    private float _blinkTimeCurrent;
    private int _score = 500;

    private Color _normalColor = Color.white;
    private Color _blinkColor = new Color(1f, 1f, 1f, 0f);

    public bool IsPaused = false;

    private void Start()
    {
        EventBus.Subscribe<GamePausedEvent>(GamePausedHandle);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _blinkTimeCurrent = _blinkTime;

        EventBus.Invoke(new SoundEvent(AudioController.BonusSpawnedKey));
    }

    private void GamePausedHandle(GamePausedEvent e)
    {
        IsPaused = e.Paused;
    }

    private void Update()
    {
        if (IsPaused == false)
        {
            _blinkTimeCurrent -= Time.deltaTime;

            if (_blinkTimeCurrent <= 0)
            {
                Blink();
                _blinkTimeCurrent = _blinkTime;
            }
        }
    }

    private void Blink()
    {
        if (_spriteRenderer.color == _normalColor)
        {
            _spriteRenderer.color = _blinkColor;
        }
        else
        {
            _spriteRenderer.color = _normalColor;
        }
    }

    public void Collect()
    {
        EventBus.Invoke(new ScoreAddedEvent(_score, transform.position));
        EventBus.Invoke(new BonusCollectedEvent(_type));
        EventBus.Invoke(new SoundEvent(AudioController.BonusCollectedKey));
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<GamePausedEvent>(GamePausedHandle);
    }
}

public enum BonusType
{
    Star,
    Grenade,
    Shovel,
    Clock,
    Helmet,
    Tank
}
