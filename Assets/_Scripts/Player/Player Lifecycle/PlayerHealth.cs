using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action<float, GameObject> OnDamage;
    public event Action<float, GameObject> OnHeal;
    
    public float Health { get; private set; }
    
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float initialHealth = 100f;

    private void Awake() => Health = initialHealth;

    public void Damage(float _damage)
    {
        if (_damage < 0)
            throw new ArgumentException("The damage cannot be less than 0.", nameof(_damage));

        Health = Mathf.Clamp(Health - _damage, 0f, 100f);
        OnDamage?.Invoke(Health, gameObject);
    }

    public void Heal(float _healAmount)
    {
        if (_healAmount < 0)
            throw new ArgumentException("The heal amount cannot be less than 0.", nameof(_healAmount));

        Health = Mathf.Clamp(Health + _healAmount, 0f, maxHealth);
        OnHeal?.Invoke(Health, gameObject);
    }

    public float GetMaxHealth() => maxHealth;
}