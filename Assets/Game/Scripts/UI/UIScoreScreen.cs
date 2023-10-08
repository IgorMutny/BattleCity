using UnityEngine;

public class UIScoreScreen : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeState();
        }
    }

    private void ChangeState()
    {
        bool lastBattleWon = PlayerStats.Instance.LastBattleWon;

        if (lastBattleWon == true)
        {
            int newLevel = PlayerStats.Instance.Level + 1;
            PlayerStats.Instance.SetLevel(PlayerStats.Instance.Level + 1, true);
            if (newLevel <= Map.MaxLevel * 2)
            {
                EventBus.Invoke(new GameStateChangedEvent(new ContinueGameState()));
            }
            else
            {
                EventBus.Invoke(new GameStateChangedEvent(new CongratulationsScreenState()));
            }
        }
        else
        {
            EventBus.Invoke(new GameStateChangedEvent(new GameOverScreenState()));
        }
    }
}
