using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float Health { get; private set; }

    [SerializeField] private float startHealth;

    private void Awake()
    {
        Health = startHealth;
    }

    public void Damage(float _damage)
    {
        Health -= _damage;
    }
}