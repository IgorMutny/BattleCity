using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private Map _map;
    private List<NPoint> _researched = new List<NPoint>();
    private List<NPoint> _onResearching = new List<NPoint>();
    private NPoint[,] _nPoints = new NPoint[Map.Width - 1, Map.Height - 1];
    private NPoint _target;
    private Vector2Int _targetPosition;

    public PathFinder(Map map)
    {
        EventBus.Subscribe<TileMapChangedEvent>(TileMapChangedHandle);

        _map = map;

        for (int x = 0; x < Map.Width - 1; x++)
        {
            for (int y = 0; y < Map.Height - 1; y++)
            {
                _nPoints[x, y] = new NPoint(x, y, GetNPointState(x, y));
            }
        }
    }

    public List<Vector2> FindPath(Vector2 begin, Vector2 target)
    {

        InitializeNewSearch();
        ExecuteSearch(begin, target);
        return ConstructRoute();
    }

    private void InitializeNewSearch()
    {
        foreach (NPoint point in _nPoints)
        {
            point.Path.Clear();
        }

        _target = null;
        _researched.Clear();
        _onResearching.Clear();
    }

    private void ExecuteSearch(Vector2 begin, Vector2 target)
    {
        NPoint initialNPoint = _nPoints[(int)begin.x, (int)begin.y];
        _onResearching.Add(initialNPoint);

        _targetPosition = new Vector2Int((int)target.x, (int)target.y);

        do
        {
            MakeStep(false);
        }
        while (_onResearching.Count > 0 && _target == null);

        if (_target == null)
        {
            _researched.Clear();
            _onResearching.Clear();
            _onResearching.Add(initialNPoint);

            do
            {
                MakeStep(true);
            }
            while (_onResearching.Count > 0 && _target == null);
        }
    }

    private List<Vector2> ConstructRoute()
    {
        List<Vector2> route = new List<Vector2>();

        if (_target != null)
        {
            foreach (NPoint point in _target.Path)
            {
                Vector2 position = new Vector2(point.Position.x + 0.5f, point.Position.y + 0.5f);
                route.Add(position);
            }
        }
        else
        {
            route.Add(AIController.NoTargetPosition);
        }

        return route;
    }

    private NPointState GetNPointState(int x, int y)
    {
        if (IsPassableTile(_map.GetTileType(x, y)) == true
            && IsPassableTile(_map.GetTileType(x + 1, y)) == true
            && IsPassableTile(_map.GetTileType(x, y + 1)) == true
            && IsPassableTile(_map.GetTileType(x + 1, y + 1)) == true)
        {
            return NPointState.Free;
        }
        else
        {
            if (IsPassableOrBreakableTile(_map.GetTileType(x, y)) == true
            && IsPassableOrBreakableTile(_map.GetTileType(x + 1, y)) == true
            && IsPassableOrBreakableTile(_map.GetTileType(x, y + 1)) == true
            && IsPassableOrBreakableTile(_map.GetTileType(x + 1, y + 1)) == true)
            {
                return NPointState.Breakable;
            }
            else
            {
                return NPointState.Unbreakable;
            }
        }
    }

    private bool IsPassableTile(TileType tile)
    {
        return tile == TileType.Empty || tile == TileType.Ice || tile == TileType.Bush;
    }

    private bool IsPassableOrBreakableTile(TileType tile)
    {
        return tile == TileType.Empty || tile == TileType.Ice || tile == TileType.Bush || tile == TileType.Brick;
    }

    private void MakeStep(bool withBreakableTiles)
    {
        List<NPoint> onAdding = new List<NPoint>();
        List<NPoint> onDeleting = new List<NPoint>();

        foreach (NPoint point in _onResearching)
        {
            List<NPoint> neighbours = GetNeighbours(point);
            foreach (NPoint neighbour in neighbours)
            {
                if (NPointStateIsValid(neighbour.State, withBreakableTiles) == true)
                {
                    if (_researched.Contains(neighbour) == false
                        && _onResearching.Contains(neighbour) == false
                        && onAdding.Contains(neighbour) == false
                        && onDeleting.Contains(neighbour) == false)
                    {
                        neighbour.AddToList(point);
                        onAdding.Add(neighbour);

                        if (neighbour.Position.x == _targetPosition.x && neighbour.Position.y == _targetPosition.y)
                        {
                            _target = neighbour;
                            return;
                        }
                    }
                }
            }

            onDeleting.Add(point);
        }

        foreach (NPoint point in onAdding)
        {
            _onResearching.Add(point);
        }

        foreach (NPoint point in onDeleting)
        {
            _onResearching.Remove(point);
            _researched.Add(point);
            point.Path.Clear();
        }
    }

    private bool NPointStateIsValid(NPointState state, bool withBreakableTiles)
    {
        if (withBreakableTiles == true)
        {
            return state == NPointState.Free || state == NPointState.Breakable;
        }
        else
        {
            return state == NPointState.Free;
        }
    }

    private List<NPoint> GetNeighbours(NPoint origin)
    {
        int x = origin.Position.x;
        int y = origin.Position.y;

        List<NPoint> results = new List<NPoint>();

        if (IsInBounds(x, y - 1) == true) results.Add(_nPoints[x, y - 1]);
        if (IsInBounds(x - 1, y) == true) results.Add(_nPoints[x - 1, y]);
        if (IsInBounds(x + 1, y) == true) results.Add(_nPoints[x + 1, y]);
        if (IsInBounds(x, y + 1) == true) results.Add(_nPoints[x, y + 1]);

        return results;
    }

    private bool IsInBounds(int x, int y)
    {
        bool xAvailable = x >= 0 && x < _nPoints.GetLength(0);
        bool yAvailable = y >= 0 && y < _nPoints.GetLength(1);
        return xAvailable && yAvailable;
    }

    private void TileMapChangedHandle(TileMapChangedEvent e)
    {
        Vector2Int position = e.Tile.GetPosition();
        int x = position.x;
        int y = position.y;

        List<NPoint> nPointsToUpdateState = new List<NPoint>();

        if (IsInBounds(x - 1, y - 1) == true) nPointsToUpdateState.Add(_nPoints[x - 1, y - 1]);
        if (IsInBounds(x, y - 1) == true) nPointsToUpdateState.Add(_nPoints[x, y - 1]);
        if (IsInBounds(x - 1, y) == true) nPointsToUpdateState.Add(_nPoints[x - 1, y]);
        if (IsInBounds(x, y) == true) nPointsToUpdateState.Add(_nPoints[x, y]);

        foreach (NPoint point in nPointsToUpdateState)
        {
            point.ChangeState(GetNPointState(point.Position.x, point.Position.y));
        }
    }

    ~PathFinder()
    {
        EventBus.Unsubscribe<TileMapChangedEvent>(TileMapChangedHandle);
    }

    public class NPoint
    {
        public readonly Vector2Int Position;
        public List<NPoint> Path = new List<NPoint>();
        public NPointState State;

        public NPoint(int x, int y, NPointState state)
        {
            Position = new Vector2Int(x, y);
            State = state;
        }

        public void AddToList(NPoint point)
        {
            foreach (NPoint p in point.Path)
            {
                Path.Add(p);
            }
            Path.Add(point);
        }

        public void ChangeState(NPointState state)
        {
            State = state;
        }
    }

    public enum NPointState
    {
        Free,
        Breakable,
        Unbreakable
    }
}

