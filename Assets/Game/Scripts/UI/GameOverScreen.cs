using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    private void Start()
    {
        EventBus.Invoke(new SoundEvent(AudioController.GameOverKey));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeState();
        }
    }

    private void ChangeState()
    {
        EventBus.Invoke(new GameStateChangedEvent(new MainMenuState()));
    }
}

