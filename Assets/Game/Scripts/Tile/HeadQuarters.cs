using UnityEngine;

public class HeadQuarters : MonoBehaviour
{
    [SerializeField] private Sprite _destroyedSprite;

    private SpriteRenderer _spriteRenderer;
    private bool _isDestroyed = false;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Destroy()
    {
        if (_isDestroyed == false)
        {
            _isDestroyed = true;
            EventBus.Invoke(new DefeatEvent());
            EventBus.Invoke(new SoundEvent(AudioController.PlayerDestroyedKey));
            _spriteRenderer.sprite = _destroyedSprite;
        }
    }
}
