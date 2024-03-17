using UnityEngine;
using System;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyKill : MonoBehaviour, IKillable
{
    public event Action<GameObject> OnKill;

    private bool hasEnemyBeenKilled;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        hasEnemyBeenKilled = false;
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if (enemyHealth.Health <= 0f && !hasEnemyBeenKilled)
        {
            Kill();
        }
    }

    public void Kill()
    {
        OnKill?.Invoke(gameObject);
        hasEnemyBeenKilled = true;
    }
}