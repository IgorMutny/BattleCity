using UnityEngine;

public class UIFlag : MonoBehaviour
{
    private GameObject _uiNumberSample;
    private UINumber _uiLevelNumberDigit1;
    private UINumber _uiLevelNumberDigit2;
    private Vector3 _uiLevelNumberDigit1Position = new Vector3(28, 2, 0);
    private Vector3 _uiLevelNumberDigit2Position = new Vector3(27, 2, 0);

    public void Init()
    {
        _uiNumberSample = Resources.Load<GameObject>("UINumber");

        _uiLevelNumberDigit1 = Instantiate(
            _uiNumberSample, _uiLevelNumberDigit1Position, Quaternion.identity)
            .GetComponent<UINumber>();
        _uiLevelNumberDigit2 = Instantiate(
            _uiNumberSample, _uiLevelNumberDigit2Position, Quaternion.identity)
            .GetComponent<UINumber>();
    }

    public void SetLevelNumber(int level)
    {
        int digit2 = level / 10;
        int digit1 = level % 10;
        _uiLevelNumberDigit2.SetValue(digit2, Color.black);
        _uiLevelNumberDigit1.SetValue(digit1, Color.black);
    }

    public void BattleFinishedHandle()
    {
        Destroy(_uiLevelNumberDigit1.gameObject);
        Destroy(_uiLevelNumberDigit2.gameObject);
    }
}
