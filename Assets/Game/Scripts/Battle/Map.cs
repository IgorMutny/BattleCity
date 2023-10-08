using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Map : MonoBehaviour
{
    public static readonly int Width = 26;
    public static readonly int Height = 26;
    public static readonly string FilePath = Application.streamingAssetsPath + "/Levels/";
    public static readonly int MaxLevel = 35;

    private GameObject _bedrockSample;
    private GameObject _backgroundSample;

    private Tile[,] _tileMap = new Tile[Width, Height];

    private GameObject _background;
    private List<GameObject> _bedrocks = new List<GameObject>();

    public void Init(int level)
    {
        EventBus.Subscribe<BattleFinishedEvent>(BattleFinishedHandle);

        _bedrockSample = Resources.Load<GameObject>("Bedrock");
        _backgroundSample = Resources.Load<GameObject>("Background");

        Load(level);
    }

    public TileType GetTileType(int x, int y)
    {
        return _tileMap[x, y].GetTileType();
    }

    public void SetTileType(int x, int y, TileType tileType)
    {
        _tileMap[x, y].SetTileType(tileType);
    }

    private void Load(int level)
    {
        if (level > MaxLevel)
        {
            level -= MaxLevel;
        }

        AddBackground();
        AddBedrock();
        LoadTileMap(FilePath + level.ToString());
    }


    private void AddBackground()
    {
        _background = Instantiate(_backgroundSample);
    }

    private void AddBedrock()
    {
        for (int x = 0; x < Width; x++)
        {
            _bedrocks.Add(Instantiate(_bedrockSample, new Vector3(x, -1, 0), Quaternion.identity));
            _bedrocks.Add(Instantiate(_bedrockSample, new Vector3(x, Height, 0), Quaternion.identity));
        }

        for (int y = 0; y < Height; y++)
        {
            _bedrocks.Add(Instantiate(_bedrockSample, new Vector3(-1, y, 0), Quaternion.identity));
            _bedrocks.Add(Instantiate(_bedrockSample, new Vector3(Width, y, 0), Quaternion.identity));
        }
    }

    private void LoadTileMap(string fileName)
    {
        UnityWebRequest www = UnityWebRequest.Get(fileName);
        www.SendWebRequest();

        while (www.isDone == false) { }

        string stream = www.downloadHandler.text;

        char[] bytes = stream.ToCharArray();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                TileType tile = (TileType)bytes[x * Map.Width + y];
                _tileMap[x, y] = new Tile(x, y, tile);
            }
        }
    }

    private void BattleFinishedHandle(BattleFinishedEvent e)
    {
        foreach (Tile tile in _tileMap)
        {
            tile.SetTileType(TileType.Empty);
        }

        foreach (GameObject bedrock in _bedrocks)
        {
            Destroy(bedrock);
        }

        Destroy(_background);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<BattleFinishedEvent>(BattleFinishedHandle);
    }
}

