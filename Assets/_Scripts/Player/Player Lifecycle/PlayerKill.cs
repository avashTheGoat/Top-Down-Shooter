using UnityEngine;
using System;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerKill : MonoBehaviour, IKillable
{
    public event Action<GameObject> OnKill;
    
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
            Kill();
    }

    public void Kill()
    {
        OnKill?.Invoke(gameObject);
        hasPlayerDied = true;

        // should remove this later, just have it for testing
        Destroy(gameObject);
    }
}