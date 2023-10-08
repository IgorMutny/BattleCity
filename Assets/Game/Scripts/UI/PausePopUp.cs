using UnityEngine;

public class PausePopUp : MonoBehaviour
{
    private void Start()
    {
        EventBus.Subscribe<GamePausedEvent>(GamePausedHandle);
    }

    private void GamePausedHandle(GamePausedEvent e)
    {
        if (e.Paused == false)
        {
            EventBus.Invoke(new PopUpDestroyedEvent(gameObject));
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<GamePausedEvent>(GamePausedHandle);
    }
}
