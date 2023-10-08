using System.Collections.Generic;
using UnityEngine;

public class BonusesController : MonoBehaviour
{
    private List<GameObject> _bonuses = new List<GameObject>();

    private Bonus _currentBonus;

    public void Init()
    {
        EventBus.Subscribe<BlinkingEnemyDamagedEvent>(CreateBonus);
        EventBus.Subscribe<BattleFinishedEvent>(BattleFinishedHandle);

        _bonuses.Add(Resources.Load<GameObject>("BonusStar"));
        _bonuses.Add(Resources.Load<GameObject>("BonusGrenade"));
        _bonuses.Add(Resources.Load<GameObject>("BonusTank"));
        _bonuses.Add(Resources.Load<GameObject>("BonusHelmet"));
        _bonuses.Add(Resources.Load<GameObject>("BonusShovel"));
        _bonuses.Add(Resources.Load<GameObject>("BonusClock"));
    }

    private void BattleFinishedHandle(BattleFinishedEvent e)
    {
        if (_currentBonus != null)
        {
            Destroy(_currentBonus.gameObject);
        }
    }

    private void CreateBonus(BlinkingEnemyDamagedEvent e)
    {
        if (_currentBonus != null)
        {
            Destroy(_currentBonus.gameObject);
            _currentBonus = null;
        }

        GameObject sample = _bonuses[Random.Range(0, _bonuses.Count)];

        float x = Random.Range(0, Map.Width - 1) + 0.5f;
        float y = Random.Range(0, Map.Height - 1) + 0.5f;
        Vector3 position = new Vector3(x, y, 0);

        _currentBonus = Instantiate(sample, position, Quaternion.identity).GetComponent<Bonus>();
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<BlinkingEnemyDamagedEvent>(CreateBonus);
        EventBus.Unsubscribe<BattleFinishedEvent>(BattleFinishedHandle);
    }
}
