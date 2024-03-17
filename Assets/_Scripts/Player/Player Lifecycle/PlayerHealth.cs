using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float Health { get; private set; }

    [Min(0f)]
    [SerializeField] private float startHealth;

    private void Awake() => Health = startHealth;

    public void Damage(float _damage)
    {
        if (_damage < 0)
            throw new System.ArgumentException("The damage cannot be less than 0.", nameof(_damage));

        Health -= _damage;
        print($"Player got damaged to {Health}");
    }

    public void Heal(float _healAmount)
    {
        if (_healAmount < 0)
            throw new System.ArgumentException("The heal amount cannot be less than 0.", nameof(_healAmount));

        Health += _healAmount;
        print($"Player got healed to {Health}");
    }
}