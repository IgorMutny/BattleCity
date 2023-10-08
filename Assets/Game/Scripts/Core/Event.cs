using UnityEngine;

public class GameStateChangedEvent
{
    public readonly GameStateBase GameState;
    public GameStateChangedEvent(GameStateBase gameState)
    {
        GameState = gameState;
    }
}

public class VictoryEvent { }

public class DefeatEvent { }

public class BattleStartedEvent
{
    public readonly int Level;

    public BattleStartedEvent(int level)
    {
        Level = level;
    }
}

public class BattleFinishedEvent { }

public class PlayerDestroyedEvent { }

public class EnemyDestroyedEvent
{
    public readonly Enemy Tank;
    public readonly int Type;

    public EnemyDestroyedEvent(Enemy tank, int type)
    {
        Tank = tank;
        Type = type;
    }
}

public class PlayerLivesChangedEvent
{
    public readonly int Lives;

    public PlayerLivesChangedEvent(int lives)
    {
        Lives = lives;
    }
}

public class RemainingEnemiesChangedEvent
{
    public readonly int RemainingEnemies;

    public RemainingEnemiesChangedEvent(int remainingEnemies)
    {
        RemainingEnemies = remainingEnemies;
    }
}

public class ScoreAddedEvent
{
    public readonly int Score;
    public readonly UnityEngine.Vector3 Position;

    public ScoreAddedEvent(int score, UnityEngine.Vector3 position)
    {
        Score = score;
        Position = position;
    }
}

public class BlinkingEnemyDamagedEvent { }

public class BonusCollectedEvent
{
    public readonly BonusType Type;

    public BonusCollectedEvent(BonusType type)
    {
        Type = type;
    }
}

public class PlayerSpawnedEvent
{
    public readonly Player Tank;

    public PlayerSpawnedEvent(Player tank)
    {
        Tank = tank;
    }
}

public class EnemySpawnedEvent
{
    public readonly Enemy Tank;

    public EnemySpawnedEvent(Enemy tank)
    {
        Tank = tank;
    }
}

public class GamePausedEvent
{
    public readonly bool Paused;

    public GamePausedEvent(bool paused)
    {
        Paused = paused;
    }
}

public class FortressStateChangedEvent
{
    public readonly FortressState State;

    public FortressStateChangedEvent(FortressState state)
    {
        State = state;
    }
}

public class PopUpDestroyedEvent
{
    public readonly GameObject PopUp;

    public PopUpDestroyedEvent(GameObject popUp)
    {
        PopUp = popUp;
    }
}

public class TileMapChangedEvent
{
    public readonly Tile Tile;

    public TileMapChangedEvent(Tile tile)
    {
        Tile = tile;
    }
}

public class SoundEvent
{
    public readonly string Key;

    public SoundEvent(string key)
    {
        Key = key;
    }
}

public class StopSoundEvent
{
    public readonly string Key;

    public StopSoundEvent(string key)
    {
        Key = key;
    }
}

public class InputButtonPressedEvent
{
    public readonly int Value;

    public InputButtonPressedEvent(int value)
    { 
        Value = value;
    }
}

public class VolumeChangedEvent
{
    public readonly bool IsActive;

    public VolumeChangedEvent(bool isActive)
    {
        IsActive = isActive;
    }
}

