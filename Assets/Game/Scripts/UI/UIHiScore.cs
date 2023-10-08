using UnityEngine;

public class UIHiScore : MonoBehaviour
{
    private GameObject uiNumberSample;
    private int _digitsCount = 6;
    private Vector3 _startPosition = new Vector3(5f, 0, 0);

    private void Start()
    {
        uiNumberSample = Resources.Load<GameObject>("UINumber");
        UINumber[] digits = new UINumber[_digitsCount];
        int hiScore = PlayerStats.Instance.HiScore;

        for (int i = 0; i < _digitsCount; i++)
        {
            digits[i] = Instantiate(
                uiNumberSample, transform.position + _startPosition, Quaternion.identity, transform)
                .GetComponent<UINumber>();
            _startPosition.x += 1;
        }

        int divider = (int)Mathf.Pow(10, _digitsCount - 1);
        int hiScoreDivided = hiScore;
        for (int i = 0; i < _digitsCount; i++)
        {
            int value = hiScoreDivided / divider;
            hiScoreDivided -= value * divider;
            digits[i].SetValue(value, Color.yellow);
            divider /= 10;       
        }
    }
}
