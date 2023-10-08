using UnityEngine;

public class UILevel : MonoBehaviour
{
    private Vector3 _uiNumber2Position = new Vector3(14.5f, 22.5f, 0);
    private Vector3 _uiNumber1Position = new Vector3(15.5f, 22.5f, 0);

    private void Start()
    {
        GameObject uiNumberSample = Resources.Load<GameObject>("UINumber");

        UINumber uiNumber2 = Instantiate(
            uiNumberSample, _uiNumber2Position, Quaternion.identity, transform)
            .GetComponent<UINumber>();
        UINumber uiNumber1 = Instantiate(
            uiNumberSample, _uiNumber1Position, Quaternion.identity, transform)
            .GetComponent<UINumber>();

        int level = PlayerStats.Instance.Level;
        uiNumber2.SetValue(level / 10, Color.white);
        uiNumber1.SetValue(level % 10, Color.white);
    }
}
