using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private BarFillUI barFill;
    [SerializeField] private Component damageableComponent;

    private IDamageable damageable;

    private void Awake() => damageable = (IDamageable)damageableComponent;

    private void Start()
    {
        damageable.OnDamage += (_health, _) => barFill.FillAmount = Mathf.Clamp(_health / damageable.GetMaxHealth(), 0f, 1f);
        damageable.OnHeal += (_health, _) => barFill.FillAmount = Mathf.Clamp(_health / damageable.GetMaxHealth(), 0f, 1f);
    }
}