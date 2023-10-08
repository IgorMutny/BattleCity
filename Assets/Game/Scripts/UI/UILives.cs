using UnityEngine;

public class UILives : MonoBehaviour
{
    private GameObject _uiNumberSample;
    private UINumber _uiNumber1;
    private UINumber _uiNumber2;
    private Vector3 _uiNumber1Position = new Vector3(29, 9, 0);
    private Vector3 _uiNumber2Position = new Vector3(28, 9, 0);

    public void Init()
    {
        _uiNumberSample = Resources.Load<GameObject>("UINumber");

        _uiNumber1 = Instantiate(
            _uiNumberSample, _uiNumber1Position, Quaternion.identity)
            .GetComponent<UINumber>();

        _uiNumber2 = Instantiate(
            _uiNumberSample, _uiNumber2Position, Quaternion.identity)
            .GetComponent<UINumber>();
    }

    public void SetLives(int lives)
    {
        _uiNumber2.SetValue(lives / 10, Color.black);
        _uiNumber1.SetValue(lives % 10, Color.black);
    }

    public void BattleFinishedHandle()
    {
        Destroy(_uiNumber1.gameObject);
        Destroy(_uiNumber2.gameObject);
    }
}
