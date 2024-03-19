using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillReciever : MonoBehaviour
{
    [SerializeField] private EnemyWavesSpawner enemyWaves;

    private List<EnemyKill> enemyKills = new();
    private List<EnemyKill> subscribedEnemyKills = new();

    private void Update()
    {
        subscribedEnemyKills.RemoveAll((_enemyKill) => { return _enemyKill == null; });

        List<int> _indexesToRemove = new();
        enemyKills = GetEnemyKills();

        for (int i = 0; i < enemyKills.Count; i++)
        {
            EnemyKill _enemyKill = enemyKills[i];

            if (_enemyKill == null)
            {
                _indexesToRemove.Add(i);
                continue;
            }

            if (subscribedEnemyKills.Contains(_enemyKill)) continue;

            _enemyKill.OnKill += DestroyEnemy;

            subscribedEnemyKills.Add(_enemyKill);
        }

        foreach (int _indexToRemove in _indexesToRemove)
        {
            enemyKills.RemoveAt(_indexToRemove);
        }
    }

    private List<EnemyKill> GetEnemyKills()
    {
        List<EnemyKill> _enemyKills = new();

        enemyWaves.SpawnedEnemies.ForEach((_enemy) => { _enemyKills.Add(_enemy.GetComponent<EnemyKill>()); });

        return _enemyKills;
    }

    private void DestroyEnemy(GameObject deadEnemy) => Destroy(deadEnemy);
}