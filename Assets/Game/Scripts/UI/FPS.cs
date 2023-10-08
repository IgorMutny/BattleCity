using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    private TextMeshPro _textMeshPro;
    private float _timer = 0.5f;
    private float _timerCurrent;

    private void Start()
    {
        _textMeshPro = gameObject.AddComponent<TextMeshPro>();
        _textMeshPro.fontSize = 288;
        _timerCurrent = _timer;
    }

    private void Update()
    {
        _timerCurrent -= Time.deltaTime;

        if (_timerCurrent <= 0)
        {
            float fps = 1;
            if (Time.deltaTime > 0)
            {
                fps = 1.0f / Time.deltaTime;
            }
            _textMeshPro.text = "FPS: " + fps.ToString();

            _timerCurrent = _timer;
        }
    }
}
