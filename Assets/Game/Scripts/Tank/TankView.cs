using System.Collections.Generic;
using UnityEngine;

public class TankView
{
    private SpriteRenderer _spriteRenderer;
    private List<Sprite> _sprites;
    private float _animationTime;
    private float _animationTimeCurrent;
    private int _currentSprite;
    private bool _isMoving;

    private bool _isBlinking;
    private float _blinkTime = 0.1f;
    private float _blinkTimeCurrent;
    private Color _blinkColor = Color.red;
    private Color _normalColor;

    public TankView(
        SpriteRenderer spriteRenderer, Color defaultColor, List<Sprite> sprites, float animationTime, bool isBlinking)
    {
        _spriteRenderer = spriteRenderer;
        _sprites = sprites;
        _animationTime = animationTime;
        _currentSprite = 0;

        _spriteRenderer.sprite = _sprites[_currentSprite];
        SetColor(defaultColor);
        _isBlinking = isBlinking;
        _blinkTimeCurrent = _blinkTime;
    }

    public void Update()
    {
        TryAnimateMoving();
        TryAnimateBlinking();
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void SetMovement(bool value)
    {
        _isMoving = value;
    }

    private void TryAnimateMoving()
    {
        if (_isMoving == true)
        {
            _animationTimeCurrent -= Time.deltaTime;
        }

        if (_animationTimeCurrent <= 0)
        {
            ChangeSprite();
            _animationTimeCurrent = _animationTime;
        }
    }

    private void TryAnimateBlinking()
    {
        if (_isBlinking == true)
        {
            _blinkTimeCurrent -= Time.deltaTime;
        }

        if (_blinkTimeCurrent <= 0)
        {
            Blink();
            _blinkTimeCurrent = _blinkTime;
        }
    }

    private void ChangeSprite()
    {
        _currentSprite += 1;
        if (_currentSprite >= _sprites.Count)
        {
            _currentSprite = 0;
        }

        _spriteRenderer.sprite = _sprites[_currentSprite];
    }

    private void Blink()
    {
        if (_spriteRenderer.color == _blinkColor)
        {
            _spriteRenderer.color = _normalColor;
        }
        else
        {
            _normalColor = _spriteRenderer.color;
            _spriteRenderer.color = _blinkColor;
        }
    }
}
