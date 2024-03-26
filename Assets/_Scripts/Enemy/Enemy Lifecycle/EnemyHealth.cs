using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public event Action<float, GameObject> OnDamage;
    public event Action<float, GameObject> OnHeal;

    public float Health { get; private set; }

    [SerializeField] private float maxHealth = 100f;

    private void Awake() => Health = maxHealth;

    public void Damage(float _damage)
    {
        Health -= _damage;
        OnDamage?.Invoke(Health, gameObject);
    }

    public void Heal(float _heal)
    {
        throw new NotImplementedException();
    }

    public float GetMaxHealth() => maxHealth;
}