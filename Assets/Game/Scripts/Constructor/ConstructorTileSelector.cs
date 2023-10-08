using UnityEngine;

public class ConstructorTileSelector : MonoBehaviour
{
    [SerializeField]
    private Constructor _constructor;

    public void SetCurrentTile(int value)
    {
        _constructor.SetCurrentTile(value + 1);
    }
}
