using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;

    private SpriteRenderer _spriteRenderer;
    private float _animationTime = 0.03f;
    private float _animationTimeCurrent;
    private int _currentSprite = 0;

    private bool _isPaused = false;

    private void Start()
    {
        EventBus.Subscribe<GamePausedEvent>(GamePausedHandle);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _sprites[_currentSprite];
        _animationTimeCurrent = _animationTime;
    }

    private void GamePausedHandle(GamePausedEvent e)
    {
        _isPaused = e.Paused;
    }

    private void Update()
    {
        if (_isPaused == false)
        {
            _animationTimeCurrent -= Time.deltaTime;

            if (_animationTimeCurrent <= 0)
            {
                ChangeSprite();
                _animationTimeCurrent = _animationTime;
            }
        }
    }

    private void ChangeSprite()
    {
        _currentSprite += 1;
        if (_currentSprite < _sprites.Count)
        {
            _spriteRenderer.sprite = _sprites[_currentSprite];
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<GamePausedEvent>(GamePausedHandle);
    }
}
