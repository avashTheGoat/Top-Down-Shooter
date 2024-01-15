using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float Health { get; private set; }

    [Min(0f)]
    [SerializeField] private float startHealth;

    public void Damage(float _damage)
    {
        Health -= _damage;
    }

    public void Heal(float _healAmount)
    {
        Health += _healAmount;
    }
}