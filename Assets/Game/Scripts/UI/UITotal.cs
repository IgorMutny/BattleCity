using UnityEngine;

public class UITotal : MonoBehaviour
{
    private void Start()
    {
        GameObject uiNumberSample = Resources.Load<GameObject>("UINumber");

        Vector3 uiCountDigit1Position = new Vector3(transform.position.x + 5f, transform.position.y, 0);
        Vector3 uiCountDigit2Position = new Vector3(transform.position.x + 4f, transform.position.y, 0);

        UINumber uiCountDigit1 = Instantiate(
            uiNumberSample, uiCountDigit1Position, Quaternion.identity, transform)
            .GetComponent<UINumber>();
        UINumber uiCountDigit2 = Instantiate(
            uiNumberSample, uiCountDigit2Position, Quaternion.identity, transform)
            .GetComponent<UINumber>();

        int value = 0;
        for (int i = 0; i < 4; i++)
        {
            value += PlayerStats.Instance.GetDestroyedEnemiesCount(i);
        }
        uiCountDigit2.SetValue(value / 10, Color.white);
        uiCountDigit1.SetValue(value % 10, Color.white);
    }
}
