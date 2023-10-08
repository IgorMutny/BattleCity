using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    private void Start()
    {
        EventBus.Invoke(new SoundEvent(AudioController.VictoryKey));
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

