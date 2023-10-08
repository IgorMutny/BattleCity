using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController: MonoBehaviour
{
    public readonly static string BulletShotKey = "BulletShot";
    public readonly static string BulletDestroyedKey = "BulletDestroyed";
    public readonly static string BulletDestroyedWithObstacleKey = "BulletDestroyedWithObstacle";
    public readonly static string PlayerDestroyedKey = "PlayerDestroyed";
    public readonly static string EnemyDestroyedKey = "EnemyDestroyed";
    public readonly static string BonusCollectedKey = "BonusCollected";
    public readonly static string BonusSpawnedKey = "BonusSpawned";
    public readonly static string ExtraLifeReceivedKey = "ExtraLifeReceived";
    public readonly static string HeavyTankDamagedKey = "HeavyTankDamaged";
    public readonly static string GameOverKey = "GameOver";
    public readonly static string IceRotationKey = "IceRotation";
    public readonly static string MainThemeKey = "MainTheme";
    public readonly static string PlayerMovingKey = "PlayerMoving";
    public readonly static string EnemyMovingKey = "EnemyMoving";
    public readonly static string VictoryKey = "Victory";
    public readonly static string PauseKey = "Pause";

    [SerializeField] private AudioClip _bulletShotClip;
    [SerializeField] private AudioClip _bulletDestroyedClip;
    [SerializeField] private AudioClip _bulletDestroyedWithObstacleClip;
    [SerializeField] private AudioClip _enemyDestroyedClip;
    [SerializeField] private AudioClip _playerDestroyedClip;
    [SerializeField] private AudioClip _bonusCollectedClip;
    [SerializeField] private AudioClip _bonusSpawnedClip;
    [SerializeField] private AudioClip _extraLifeReceivedClip;
    [SerializeField] private AudioClip _heavyTankDamagedClip;
    [SerializeField] private AudioClip _gameOverClip;
    [SerializeField] private AudioClip _iceRotationClip;
    [SerializeField] private AudioClip _mainThemeClip;
    [SerializeField] private AudioClip _playerMovingClip;
    [SerializeField] private AudioClip _enemyMovingClip;
    [SerializeField] private AudioClip _victoryClip;
    [SerializeField] private AudioClip _pauseClip;

    private Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioSource> _sources = new Dictionary<string, AudioSource>();

    private int _volume;
    private string _volumeKey = "Sound";

    private void Start()
    {
        FillDictionaries();

        EventBus.Subscribe<SoundEvent>(Play);
        EventBus.Subscribe<StopSoundEvent>(Stop);
        EventBus.Subscribe<VolumeChangedEvent>(VolumeChangedHandle);

        if (PlayerPrefs.HasKey(_volumeKey))
        {
            _volume = PlayerPrefs.GetInt(_volumeKey);
        }
        else
        {
            _volume = 1;
            PlayerPrefs.SetInt(_volumeKey, _volume);
        }
    }

    private void Play(SoundEvent e)
    {
        AudioSource audioSource = _sources[e.Key];
        AudioClip clip = _clips[e.Key];
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void Stop(StopSoundEvent e)
    {
        AudioSource audioSource = _sources[e.Key];
        audioSource.Stop();
    }

    private void VolumeChangedHandle(VolumeChangedEvent e)
    { 
        if (e.IsActive == true)
        {
            _volume = 1;
        }
        else
        {
            _volume = 0;
        }

        foreach (AudioSource audioSource in _sources.Values)
        { 
            audioSource.volume = _volume;
        }

        PlayerPrefs.SetInt(_volumeKey, _volume);
    }

    private void FillDictionaries()
    {
        _clips.Add(BulletShotKey, _bulletShotClip);
        _sources.Add(BulletShotKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(BulletDestroyedKey, _bulletDestroyedClip);
        _sources.Add(BulletDestroyedKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(BulletDestroyedWithObstacleKey, _bulletDestroyedWithObstacleClip);
        _sources.Add(BulletDestroyedWithObstacleKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(PlayerDestroyedKey, _playerDestroyedClip);
        _sources.Add(PlayerDestroyedKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(EnemyDestroyedKey, _enemyDestroyedClip);
        _sources.Add(EnemyDestroyedKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(BonusCollectedKey, _bonusCollectedClip);
        _sources.Add(BonusCollectedKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(BonusSpawnedKey, _bonusSpawnedClip);
        _sources.Add(BonusSpawnedKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(ExtraLifeReceivedKey, _extraLifeReceivedClip);
        _sources.Add(ExtraLifeReceivedKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(HeavyTankDamagedKey, _heavyTankDamagedClip);
        _sources.Add(HeavyTankDamagedKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(GameOverKey, _gameOverClip);
        _sources.Add(GameOverKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(IceRotationKey, _iceRotationClip);
        _sources.Add(IceRotationKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(MainThemeKey, _mainThemeClip);
        _sources.Add(MainThemeKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(PlayerMovingKey, _playerMovingClip);
        _sources.Add(PlayerMovingKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(EnemyMovingKey, _enemyMovingClip);
        _sources.Add(EnemyMovingKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(VictoryKey, _victoryClip);
        _sources.Add(VictoryKey, gameObject.AddComponent<AudioSource>());

        _clips.Add(PauseKey, _pauseClip);
        _sources.Add(PauseKey, gameObject.AddComponent<AudioSource>());
    }

    public int GetVolume()
    {
        return _volume;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<SoundEvent>(Play);
        EventBus.Unsubscribe<StopSoundEvent>(Stop);
        EventBus.Unsubscribe<VolumeChangedEvent>(VolumeChangedHandle);
    }

}
