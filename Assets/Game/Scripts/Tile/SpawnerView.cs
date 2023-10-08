using System.Collections.Generic;
using UnityEngine;

public class SpawnerView
{
    private List<Sprite> _sprites;
    private float _animationTime = 0.05f;
    private float _animationTimeCurrent;
    private SpriteRenderer _spriteRenderer;
    private Color _colorNormal = Color.white;
    private Color _colorInvisible = new Color(1f, 1f, 1f, 0f);
    private int _currentSprite;

    public SpawnerView(SpriteRenderer spriteRenderer, List<Sprite> sprites)
    {
        _spriteRenderer = spriteRenderer;
        _sprites = sprites;

        Disable();
    }

    public void Update()
    {
        Animate();
    }

    public void Enable()
    {
        _currentSprite = 0;
        _spriteRenderer.sprite = _sprites[_currentSprite];
        _spriteRenderer.color = _colorNormal;
        _animationTimeCurrent = _animationTime;
    }

    public void Disable()
    {
        _spriteRenderer.color = _colorInvisible;
    }

    private void Animate()
    {
        _animationTimeCurrent -= Time.deltaTime;

        if (_animationTimeCurrent <= 0)
        {
            ChangeSprite();
            _animationTimeCurrent = _animationTime;
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


}
