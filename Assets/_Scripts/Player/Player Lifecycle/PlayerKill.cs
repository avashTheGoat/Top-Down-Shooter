using UnityEngine;
using System;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerKill : MonoBehaviour, IKillable
{
    public event Action OnPlayerDeath;

    private bool hasPlayerDied;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        hasPlayerDied = false;
    }

    private void Update()
    {
        print("Player health: " + playerHealth.Health);
        if (playerHealth.Health <= 0f && !hasPlayerDied)
            Kill();
    }

    public void Kill()
    {
        OnPlayerDeath?.Invoke();
        hasPlayerDied = true;

        Destroy(gameObject);
    }
}