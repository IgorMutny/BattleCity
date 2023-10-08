using System.Collections.Generic;
using UnityEngine;

public class HeadQuartersController : MonoBehaviour
{
    private Map _map;

    private FortressState _currentState;
    private TileType _currentTileType;

    private GameObject _headQuartersSample;
    private GameObject _headQuarters;
    private Vector3 _headQuartersPosition = new Vector3(12.5f, 0.5f, 0);

    private List<Vector2Int> _fortressPositions = new List<Vector2Int>()
        {
            new Vector2Int(11, 0),
            new Vector2Int(11, 1),
            new Vector2Int(11, 2),
            new Vector2Int(12, 2),
            new Vector2Int(13, 2),
            new Vector2Int(14, 2),
            new Vector2Int(14, 1),
            new Vector2Int(14, 0),
        };

    private bool _isPaused = false;

    public void Init()
    {
        EventBus.Subscribe<GamePausedEvent>(GamePausedHandle);
        EventBus.Subscribe<FortressStateChangedEvent>(FortressStateChangedHandle);
        EventBus.Subscribe<BonusCollectedEvent>(BonusCollectedHandle);
        EventBus.Subscribe<BattleFinishedEvent>(BattleFinishedHandle);

        _headQuartersSample = Resources.Load<GameObject>("HeadQuarters");
        _map = GetComponent<Map>();

        AddHeadQuarters();
        SetFortressState(TileType.Brick);

        _currentState = new BrickFortressState();
        _currentTileType = TileType.Brick;
    }

    private void GamePausedHandle(GamePausedEvent e)
    {
        _isPaused = e.Paused;
    }

    private void FortressStateChangedHandle(FortressStateChangedEvent e)
    {
        _currentState = e.State;
    }

    private void BonusCollectedHandle(BonusCollectedEvent e)
    {
        if (e.Type == BonusType.Shovel)
        {
            _currentState = new SteelFortressState();
        }
    }

    private void BattleFinishedHandle(BattleFinishedEvent e)
    {
        Destroy(_headQuarters);
    }

    private void FixedUpdate()
    {
        if (_isPaused == false)
        {
            _currentState.FixedUpdate();

            TileType newTileType = _currentState.GetTileState();
            if (newTileType != _currentTileType)
            {
                SetFortressState(newTileType);
                _currentTileType = newTileType;
            }
        }
    }

    private void AddHeadQuarters()
    {
        _headQuarters = Instantiate(_headQuartersSample, _headQuartersPosition, Quaternion.identity);
    }

    private void SetFortressState(TileType tile)
    {
        foreach (Vector2Int position in _fortressPositions)
        {
            _map.SetTileType(position.x, position.y, tile);
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<GamePausedEvent>(GamePausedHandle);
        EventBus.Unsubscribe<FortressStateChangedEvent>(FortressStateChangedHandle);
        EventBus.Unsubscribe<BattleFinishedEvent>(BattleFinishedHandle);
        EventBus.Unsubscribe<BonusCollectedEvent>(BonusCollectedHandle);
    }
}
