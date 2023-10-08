using System.Collections.Generic;
using UnityEngine;

public class UIEnemies : MonoBehaviour
{
    private GameObject _uiEnemySample;
    private List<GameObject> _uiEnemies = new List<GameObject>();

    public void Init()
    {
        _uiEnemySample = Resources.Load<GameObject>("UIEnemy");

        int firstX = (int)transform.position.x;
        int firstY = (int)transform.position.y;

        int x = firstX;
        int y = firstY;

        for (int i = 0; i < EnemiesController.TotalEnemiesInLevel; i++)
        {
            y = firstY - i / 2;
            x = firstX + i % 2;
            GameObject newEnemy = Instantiate(_uiEnemySample, new Vector3(x, y, 0), Quaternion.identity);
            _uiEnemies.Add(newEnemy);
        }
    }

    public void SetEnemies(int count)
    {
        foreach (GameObject enemy in _uiEnemies)
        {
            if (_uiEnemies.IndexOf(enemy) >= count)
            {
                Destroy(_uiEnemies[count]);
            }
        }
    }

    public void BattleFinishedHandle()
    {
        foreach (GameObject enemy in _uiEnemies)
        {
            Destroy(enemy);
        }
    }
}
