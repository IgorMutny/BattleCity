using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Sprite _pauseOn;
    [SerializeField] private Sprite _pauseOff;

    private Camera _camera;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private bool _isPaused = false;

    private void Start()
    {
        _camera = Camera.main;
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 origin = new Vector2(mousePosition.x, mousePosition.y);

            RaycastHit2D[] results = Physics2D.CircleCastAll(origin, 1f, Vector2.zero);
            foreach (RaycastHit2D result in results)
            {
                if (result.collider == _collider)
                {
                    _isPaused = !_isPaused;

                    if (_isPaused == true)
                    { 
                        _spriteRenderer.sprite = _pauseOn;
                    }
                    else
                    {
                        _spriteRenderer.sprite = _pauseOff;
                    }
                    EventBus.Invoke(new StopSoundEvent(AudioController.PlayerMovingKey));
                    EventBus.Invoke(new StopSoundEvent(AudioController.EnemyMovingKey));
                    EventBus.Invoke(new SoundEvent(AudioController.PauseKey));
                    EventBus.Invoke(new GamePausedEvent(_isPaused));
                    break;
                }
            }
        }
    }
}

