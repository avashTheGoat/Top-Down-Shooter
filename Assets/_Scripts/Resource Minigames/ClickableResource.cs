using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))]
public class ClickableResource : MonoBehaviour, IDamageable, IKillable
{
    // should prob remove OnClick later
    public event Action<ClickableResource> OnClick;
    public event Action<GameObject> OnKill;
    public event Action<float, GameObject> OnDamage;
    public event Action<float, GameObject> OnHeal;

    public float Health { get; private set; }

    [SerializeField] private float initialHealth;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => OnClick?.Invoke(this));
        Health = initialHealth;
    }

    public void Damage(float _damage)
    {
        Health -= _damage;
        if (Health <= 0f)
            Kill();
    }

    public void Kill() => OnKill.Invoke(gameObject);

    public void Heal(float _heal)
    {
        throw new NotImplementedException();
    }

    public float GetMaxHealth()
    {
        throw new NotImplementedException();
    }
}