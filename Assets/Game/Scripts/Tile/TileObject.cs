using UnityEngine;

public abstract class TileObject : MonoBehaviour
{
    private Tile _tile;

    public void Init(Tile tile)
    {
        _tile = tile;
    }

    public virtual void Destroy()
    {
        _tile.SetTileType(TileType.Empty);
    }
}
