using UnityEngine;

public class UINumber : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    public void SetValue(int value, Color color)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[value];
        GetComponent<SpriteRenderer>().color = color;
    }
}
