using UnityEngine;

public class GameOverPopUp : MonoBehaviour
{
    private float _speed = 5f;
    private bool _isPaused = false;

    void Start()
    {
        EventBus.Subscribe<GamePausedEvent>(GamePausedHandle);
    }

    private void FixedUpdate()
    {
        if (_isPaused == false)
        {
            transform.Translate(Vector3.up * _speed * Time.fixedDeltaTime);
        }
    }

    private void GamePausedHandle(GamePausedEvent e)
    {
        _isPaused = e.Paused;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<GamePausedEvent>(GamePausedHandle);
    }
}
