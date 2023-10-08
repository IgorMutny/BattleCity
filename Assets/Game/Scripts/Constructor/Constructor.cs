using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Constructor : MonoBehaviour
{
    [SerializeField] private GameObject _brickSample;
    [SerializeField] private GameObject _steelSample;
    [SerializeField] private GameObject _bushSample;
    [SerializeField] private GameObject _iceSample;
    [SerializeField] private GameObject _waterSample;

    private string _fileName;
    
    private TileType _currentTile;
    private TileType[,] _map = new TileType[Map.Width, Map.Height];
    private GameObject[,] _mapView = new GameObject[Map.Width, Map.Height];

    private Camera _camera;

    private Dictionary<TileType, GameObject> _tileDictionary = new Dictionary<TileType, GameObject>();

    public void SetCurrentTile(int tileId)
    {
        _currentTile = (TileType)tileId;
    }

    public void Save()
    {
        FileStream fileStream = new FileStream(Map.FilePath + _fileName, FileMode.Create);

        foreach (TileType tile in _map)
        {
            fileStream.WriteByte((byte)tile);
        }

        fileStream.Close();
    }

    public void Load()
    {
        Clear();

        FileStream fileStream = new FileStream(Map.FilePath + _fileName, FileMode.Open);

        for (int x = 0; x < Map.Width; x++)
        {
            for (int y = 0; y < Map.Height; y++)
            {
                TileType tile = (TileType)fileStream.ReadByte();
                AddTile(x, y, tile);
            }
        }

        fileStream.Close();
    }

    public void Clear()
    {
        for (int x = 0; x < Map.Width; x++)
        {
            for (int y = 0; y < Map.Height; y++)
            {
                RemoveTile(x, y);
            }
        }
    }

    public void SetFileName(string fileName)
    {
        _fileName = fileName;
    }

    private void Start()
    {
        _camera = Camera.main;
        _currentTile = TileType.Brick;

        _tileDictionary.Add(TileType.Brick, _brickSample);
        _tileDictionary.Add(TileType.Steel, _steelSample);
        _tileDictionary.Add(TileType.Bush, _bushSample);
        _tileDictionary.Add(TileType.Ice, _iceSample);
        _tileDictionary.Add(TileType.Water, _waterSample);
    }

    private void Update()
    {
        Vector2 position = GetMousePosition();
        bool inBounds = CheckBounds(position);

        int x = (int)position.x;
        int y = (int)position.y;

        if (inBounds == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_map[x, y] == TileType.Empty)
                {
                    AddTile(x, y, _currentTile);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (_map[x, y] != TileType.Empty)
                {
                    RemoveTile(x, y);
                }
            }
        }
    }

    private Vector2 GetMousePosition()
    {
        Vector2 position = _camera.ScreenToWorldPoint(Input.mousePosition);
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }

    private bool CheckBounds(Vector2 position)
    {
        bool xAvailable = position.x >= 0 && position.x < Map.Width;
        bool yAvailable = position.y >= 0 && position.y < Map.Height;
        return xAvailable && yAvailable;
    }

    private void AddTile(int x, int y, TileType tile)
    {
        _map[x, y] = tile;
        if (tile != TileType.Empty)
        {
            GameObject tileSample = _tileDictionary[tile];
            _mapView[x, y] = Instantiate(tileSample, new Vector2(x, y), Quaternion.identity);
        }
    }

    private void RemoveTile(int x, int y)
    {

        _map[x, y] = TileType.Empty;
        Destroy(_mapView[x, y]);
    }
}
