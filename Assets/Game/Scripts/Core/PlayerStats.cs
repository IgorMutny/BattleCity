using UnityEngine;

public class PlayerStats
{
    public static PlayerStats Instance;

    private readonly int _scoreDefault = 0;
    private readonly int _hiScoreDefault = 20000;
    private readonly int _levelDefault = 1;
    private readonly int _livesDefault = 3;
    private readonly int _tankTypeDefault = 0;

    private readonly string _scoreKey = "Score";
    private readonly string _hiScoreKey = "HiScore";
    private readonly string _levelKey = "Level";
    private readonly string _livesKey = "Lives";
    private readonly string _tankTypeKey = "TankType";

    private int[] _tanksDestroyedInBattle = new int[4];

    public int Score { get; private set; }
    public int HiScore { get; private set; }
    public int Level { get; private set; }
    public int Lives { get; private set; }
    public int TankType { get; private set; }
    public bool LastBattleWon { get; private set; }

    public PlayerStats()
    {
        Instance = this;

        EventBus.Subscribe<BattleStartedEvent>(BattleStartedHandle);
    }

    public void Load()
    {
        Score = LoadStat(_scoreKey, _scoreDefault);
        HiScore = LoadStat(_hiScoreKey, _hiScoreDefault);
        Level = LoadStat(_levelKey, _levelDefault);
        Lives = LoadStat(_livesKey, _livesDefault);
        TankType = LoadStat(_tankTypeKey, _tankTypeDefault);
    }

    public void SetLastBattleWon (bool value)
    {
        LastBattleWon = value;
    }

    public void SetScore(int score, bool mustBeSaved)
    {
        Score = score;

        if (mustBeSaved == true)
        {
            PlayerPrefs.SetInt(_scoreKey, Score);
        }

        if (Score > HiScore)
        {
            HiScore = Score;
            PlayerPrefs.SetInt(_hiScoreKey, HiScore);
        }
    }

    public void SetLevel(int level, bool mustBeSaved)
    {
        Level = level;
        if (mustBeSaved == true)
        {
            PlayerPrefs.SetInt(_levelKey, Level);
        }
    }

    public void SetLives(int lives, bool mustBeSaved)
    {
        Lives = lives;
        if (mustBeSaved == true)
        {
            PlayerPrefs.SetInt(_livesKey, Lives);
        }
    }

    public void SetTankType(int tankType, bool mustBeSaved)
    {
        TankType = tankType;
        if (mustBeSaved == true)
        {
            PlayerPrefs.SetInt(_tankTypeKey, TankType);
        }
    }

    private int LoadStat(string key, int defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key);
        }
        else
        {
            return defaultValue;
        }
    }

    public void Clear()
    {
        SetScore(_scoreDefault, true);
        SetLevel(_levelDefault, true);
        SetLives(_livesDefault, true);
        SetTankType(_tankTypeDefault, true);
    }

    private void BattleStartedHandle(BattleStartedEvent e)
    {
        ClearDestroyedEnemiesArray();
    }

    private void ClearDestroyedEnemiesArray()
    {
        for(int i = 0; i < 4; i++)
        {
            _tanksDestroyedInBattle[i] = 0;
        }
    }

    public void AddDestroyedEnemy(int type)
    {
        _tanksDestroyedInBattle[type] += 1;
    }

    public int GetDestroyedEnemiesCount(int i)
    {
        return _tanksDestroyedInBattle[i];
    }
}
