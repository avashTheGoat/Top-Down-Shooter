using UnityEngine;
using System;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerKill : MonoBehaviour, IKillable
{
    public static event Action OnPlayerDeath;

    private bool hasPlayerDied;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        hasPlayerDied = false;
    }

    private void Update()
    {
        if (playerHealth.Health <= 0f && !hasPlayerDied)
        {
            Kill();
        }
    }

    public void Kill()
    {
        OnPlayerDeath?.Invoke();
        hasPlayerDied = true;
    }
}