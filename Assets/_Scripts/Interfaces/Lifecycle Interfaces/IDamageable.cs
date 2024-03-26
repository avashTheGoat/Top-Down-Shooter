using UnityEngine;
using System;

public interface IDamageable
{
    event Action<float, GameObject> OnDamage;
    event Action<float, GameObject> OnHeal;

    void Damage(float _damage);
    void Heal(float _heal);

    float GetMaxHealth();
}