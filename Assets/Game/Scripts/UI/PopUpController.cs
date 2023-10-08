using System.Collections.Generic;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    private GameObject _scorePopUpSample;

    private GameObject _pausePopUpSample;
    private Vector3 _pausePopUpPosition = new Vector3(12.5f, 12.5f, 0);

    private GameObject _gameOverPopUpSample;
    private Vector3 _gameOverPopUpPosition = new Vector3(12.5f, 0, 0);

    private List<GameObject> _popUps = new List<GameObject>();

    public void Init()
    {
        _scorePopUpSample = Resources.Load<GameObject>("ScorePopUp");
        _pausePopUpSample = Resources.Load<GameObject>("PausePopUp");
        _gameOverPopUpSample = Resources.Load<GameObject>("GameOverPopUp");

        EventBus.Subscribe<ScoreAddedEvent>(ScoreAddedHandle);
        EventBus.Subscribe<GamePausedEvent>(GamePausedHandle);
        EventBus.Subscribe<DefeatEvent>(DefeatHandle);
        EventBus.Subscribe<PopUpDestroyedEvent>(PopUpDestroyedHandle);
        EventBus.Subscribe<BattleFinishedEvent>(BattleFinishedHandle);
    }

    private void ScoreAddedHandle(ScoreAddedEvent e)
    {
        if (e.Score != 0)
        {
            GameObject newPopUp = Instantiate(_scorePopUpSample, e.Position, Quaternion.identity);
            newPopUp.GetComponent<ScorePopUp>().Init(e.Score);
            _popUps.Add(newPopUp);
        }
    }

    private void GamePausedHandle(GamePausedEvent e)
    { 
        if (e.Paused == true)
        {
            GameObject newPopUp = Instantiate(_pausePopUpSample, _pausePopUpPosition, Quaternion.identity);
            _popUps.Add(newPopUp);
        }
    }

    private void DefeatHandle(DefeatEvent e)
    {
        GameObject newPopUp = Instantiate(_gameOverPopUpSample, _gameOverPopUpPosition, Quaternion.identity);
        _popUps.Add(newPopUp);
    }

    private void PopUpDestroyedHandle(PopUpDestroyedEvent e)
    { 
        foreach (GameObject popUp in _popUps)
        {
            if (popUp == e.PopUp)
            {
                _popUps.Remove(popUp);
                break;
            }
        }
    }

    private void BattleFinishedHandle(BattleFinishedEvent e)
    { 
        foreach (GameObject popUp in _popUps)
        { 
            Destroy(popUp);
        }

        _popUps.Clear();
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<ScoreAddedEvent>(ScoreAddedHandle);
        EventBus.Unsubscribe<GamePausedEvent>(GamePausedHandle);
        EventBus.Unsubscribe<DefeatEvent>(DefeatHandle);
        EventBus.Unsubscribe<PopUpDestroyedEvent>(PopUpDestroyedHandle);
        EventBus.Unsubscribe<BattleFinishedEvent>(BattleFinishedHandle);
    }
}
