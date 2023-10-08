using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : MonoBehaviour
{
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    private Camera _camera;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private bool _isActive;

    private void Start()
    {
        _camera = Camera.main;
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _isActive = FindObjectOfType<AudioController>().GetVolume() > 1;
        SetSprite();
    }

    private void SetSprite()
    {
        if (_isActive == true)
        {
            _spriteRenderer.sprite = _soundOn;
        }
        else
        {
            _spriteRenderer.sprite = _soundOff;
        }
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
                    _isActive = !_isActive;

                    SetSprite();

                    EventBus.Invoke(new VolumeChangedEvent(_isActive));
                    break;
                }
            }
        }
    }
}
