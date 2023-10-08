using UnityEngine;

public class LevelHeader : MonoBehaviour
{
    private Vector3 _uiNumber2Position = new Vector3(16, 12.5f, 0);
    private Vector3 _uiNumber1Position = new Vector3(17, 12.5f, 0);
    private float _changeStateDelay = 2f;

    private void Start()
    {
        GameObject _uiNumberSample = Resources.Load<GameObject>("UINumber");
        UINumber uiNumber2 = Instantiate(
            _uiNumberSample, _uiNumber2Position, Quaternion.identity, transform)
            .GetComponent<UINumber>();
        UINumber uiNumber1 = Instantiate(
            _uiNumberSample, _uiNumber1Position, Quaternion.identity, transform)
            .GetComponent<UINumber>();

        int level = PlayerStats.Instance.Level;
        uiNumber2.SetValue(level / 10, Color.black);
        uiNumber1.SetValue(level % 10, Color.black);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EventBus.Invoke(new SoundEvent(AudioController.MainThemeKey));
            Invoke(nameof(ChangeState), _changeStateDelay);
        }
    }

    private void ChangeState()
    {
        EventBus.Invoke(new GameStateChangedEvent(new BattleState()));
    }
}
