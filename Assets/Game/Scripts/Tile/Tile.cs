using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private Vector2Int _position;
    private GameObject _gameObject;
    private TileType _type;

    public Tile(int x, int y, TileType type)
    {
        _position = new Vector2Int(x, y);
        _type = type;

        CreateTileObject(_type);
    }

    public void SetTileType(TileType type)
    {
        if (_gameObject != null)
        {
            GameObject.Destroy(_gameObject);
        }

        _type = type;
        CreateTileObject(_type);

        EventBus.Invoke(new TileMapChangedEvent(this));
    }

    public TileType GetTileType()
    {
        return _type;
    }

    public Vector2Int GetPosition()
    {
        return _position;
    }

    private void CreateTileObject(TileType type)
    {
        if (_type != TileType.Empty)
        {
            Vector3 position = new Vector3(_position.x, _position.y, 0);
            _gameObject = GameObject.Instantiate(TileDictionary.GetSample(type), position, Quaternion.identity);
            _gameObject.GetComponent<TileObject>().Init(this);

        }
    }
}

public enum TileType
{
    Empty,
    Brick,
    Steel,
    Bush,
    Ice,
    Water,
    Spawner,
    Tank
}

public static class TileDictionary
{
    private static Dictionary<TileType, GameObject> _dictionary = new Dictionary<TileType, GameObject>();

    static TileDictionary()
    {
        GameObject brickSample = Resources.Load<GameObject>("Brick");
        GameObject steelSample = Resources.Load<GameObject>("Steel");
        GameObject bushSample = Resources.Load<GameObject>("Bush");
        GameObject iceSample = Resources.Load<GameObject>("Ice");
        GameObject waterSample = Resources.Load<GameObject>("Water");

        _dictionary.Add(TileType.Brick, brickSample);
        _dictionary.Add(TileType.Steel, steelSample);
        _dictionary.Add(TileType.Bush, bushSample);
        _dictionary.Add(TileType.Ice, iceSample);
        _dictionary.Add(TileType.Water, waterSample);
    }

    public static GameObject GetSample(TileType tile)
    {
        return _dictionary[tile];
    }
}

