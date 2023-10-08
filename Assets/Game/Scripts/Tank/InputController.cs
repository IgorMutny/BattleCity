using UnityEngine;

public class InputController : MonoBehaviour
{
    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();

        EventBus.Subscribe<InputButtonPressedEvent>(InputButtonPressedHandle);
    }

    private void InputButtonPressedHandle(InputButtonPressedEvent e)
    {
        if (_player.IsPaused == true)
        {
            return;
        }

        float direction = 0;
        int pressedKeys = 0;

        if (e.Value == 1)
        {
            direction = 0;
            pressedKeys++;
        }
        if (e.Value == 2)
        {
            direction = 180;
            pressedKeys++;
        }
        if (e.Value == 3)
        {
            direction = 90;
            pressedKeys++;
        }
        if (e.Value == 4)
        {
            direction = 270;
            pressedKeys++;
        }

        if (pressedKeys == 1)
        {
            _player.SetMovement(direction);
        }

        if (e.Value == 5)
        {
            _player.TryShoot();
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<InputButtonPressedEvent>(InputButtonPressedHandle);
    }
}
