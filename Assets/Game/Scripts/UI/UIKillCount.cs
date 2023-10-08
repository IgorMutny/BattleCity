using UnityEngine;

public class UIKillCount : MonoBehaviour
{
    [SerializeField] private int _tankType;
    [SerializeField] private int _scoreForOneTank;

    private int _scoreDigitsCount = 4;
    private GameObject _uiNumberSample;
    private int _killCount;

    private void Start()
    {
        _uiNumberSample = Resources.Load<GameObject>("UINumber");

        CreateKillCounter();
        CreateScoreCounter();
    }

    private void CreateKillCounter()
    {
        Vector3 uiCountDigit1Position = new Vector3(transform.position.x - 2.5f, transform.position.y, 0);
        Vector3 uiCountDigit2Position = new Vector3(transform.position.x - 3.5f, transform.position.y, 0);

        UINumber uiCountDigit1 = Instantiate(
            _uiNumberSample, uiCountDigit1Position, Quaternion.identity, transform)
            .GetComponent<UINumber>();
        UINumber uiCountDigit2 = Instantiate(
            _uiNumberSample, uiCountDigit2Position, Quaternion.identity, transform)
            .GetComponent<UINumber>();

        _killCount = PlayerStats.Instance.GetDestroyedEnemiesCount(_tankType);
        uiCountDigit2.SetValue(_killCount / 10, Color.white);
        uiCountDigit1.SetValue(_killCount % 10, Color.white);
    }

    private void CreateScoreCounter()
    {
        UINumber[] scoreDigits = new UINumber[_scoreDigitsCount];
        Vector3 digitPosition = new Vector3(transform.position.x - 12.5f, transform.position.y, 0);
        int score = _killCount * _scoreForOneTank;

        for (int i = 0; i < _scoreDigitsCount; i++)
        {
            scoreDigits[i] = Instantiate(
                _uiNumberSample, digitPosition, Quaternion.identity, transform)
                .GetComponent<UINumber>();
            digitPosition.x += 1;
        }

        int divider = (int)Mathf.Pow(10, _scoreDigitsCount - 1);
        int scoreDivided = score;
        for (int i = 0; i < _scoreDigitsCount; i++)
        {
            int value = scoreDivided / divider;
            scoreDivided -= value * divider;
            scoreDigits[i].SetValue(value, Color.white);
            divider /= 10;
        }
    }
}
