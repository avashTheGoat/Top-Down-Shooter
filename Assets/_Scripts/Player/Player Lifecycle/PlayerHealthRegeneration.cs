using UnityEngine;

public class PlayerHealthRegeneration : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float percentOfHealthRecoveredPerSec;
    [SerializeField] private PlayerHealth playerHealth;

    private void Update() => playerHealth.Heal(percentOfHealthRecoveredPerSec * playerHealth.GetMaxHealth() * Time.deltaTime);
}