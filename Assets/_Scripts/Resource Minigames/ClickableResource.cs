using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))]
public class ClickableResource : MonoBehaviour, IDamageable, IKillable
{
    public event Action<ClickableResource> OnClick;
    public event Action<ClickableResource> OnResourceDestroy;

    public ResourceSO Resource;

    public int MinResourceAmount { get; private set; }
    public int MaxResourceAmount { get; private set; }

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

    public void Kill() => OnResourceDestroy.Invoke(this);

    public void SetMinResourceAmount(int newMin)
    {
        if (newMin < 0)
            throw new ArgumentException("MinResourceAmount cannot be less than 0.");

        MinResourceAmount = newMin;
    }

    public void SetMaxResourceAmount(int newMax)
    {
        if (newMax < 0)
            throw new ArgumentException("MaxResourceAmount cannot be less than 0.");

        MaxResourceAmount = newMax;
    }
}