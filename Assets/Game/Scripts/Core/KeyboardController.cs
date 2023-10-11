using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) EventBus.Invoke(new InputButtonPressedEvent(1));
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) EventBus.Invoke(new InputButtonPressedEvent(2));
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) EventBus.Invoke(new InputButtonPressedEvent(3));
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) EventBus.Invoke(new InputButtonPressedEvent(4));
        if (Input.GetKey(KeyCode.Space)) EventBus.Invoke(new InputButtonPressedEvent(5));
    }
}
