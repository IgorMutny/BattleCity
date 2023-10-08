using UnityEngine;

public abstract class FortressState
{
    public abstract void FixedUpdate();
    public abstract TileType GetTileState();
}

public class BrickFortressState : FortressState
{
    public override void FixedUpdate() { }

    public override TileType GetTileState()
    {
        return TileType.Brick;
    }

}

public class SteelFortressState : FortressState
{
    private float _stateTime = 16;

    public override void FixedUpdate()
    {
        _stateTime -= Time.fixedDeltaTime;
        if (_stateTime <= 0)
        {
            EventBus.Invoke(new FortressStateChangedEvent(new BlinkingFortressState()));
        }
    }

    public override TileType GetTileState()
    {
        return TileType.Steel;
    }
}

public class BlinkingFortressState : FortressState
{
    private float _stateTime = 4;
    private float _blinkTime = 0.2f;
    private float _blinkTimeCurrent;
    private TileType _currentTileType;

    public BlinkingFortressState()
    {
        _blinkTimeCurrent = _blinkTime;
        _currentTileType = TileType.Steel;
    }

    public override void FixedUpdate()
    {
        _stateTime -= Time.fixedDeltaTime;
        if (_stateTime <= 0)
        {
            EventBus.Invoke(new FortressStateChangedEvent(new BrickFortressState()));
        }

        _blinkTimeCurrent -= Time.fixedDeltaTime;
        if (_blinkTimeCurrent <= 0)
        {
            _blinkTimeCurrent = _blinkTime;
            if (_currentTileType == TileType.Steel)
            {
                _currentTileType = TileType.Brick;
            }
            else
            {
                _currentTileType = TileType.Steel;
            }
        }
    }

    public override TileType GetTileState()
    {
        return _currentTileType;
    }
}
