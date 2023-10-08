using System.Collections.Generic;
using UnityEngine;

public class ScorePopUp : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites = new List<Sprite>();

    private float _lifeTime = 2;
    private bool _isPaused = false;

    public void Init(int score)
    {
        EventBus.Subscribe<GamePausedEvent>(GamePausedHandle);
        
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        int spriteIndex = score / 100 - 1;
        spriteRenderer.sprite = _sprites[spriteIndex];
    }

    private void GamePausedHandle(GamePausedEvent e)
    {
        _isPaused = e.Paused;
    }

    private void FixedUpdate()
    {
        if (_isPaused == false)
        {
            _lifeTime -= Time.fixedDeltaTime;
            if (_lifeTime <= 0 )
            {
                EventBus.Invoke(new PopUpDestroyedEvent(gameObject));
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<GamePausedEvent>(GamePausedHandle);
    }
}
