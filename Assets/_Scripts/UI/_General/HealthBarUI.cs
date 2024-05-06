using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Component damageableComponent;

    private IDamageable damageable;

    private void Awake()
    {
        if (damageableComponent == null)
            return;

        damageable = (IDamageable)damageableComponent;
    }

    private void Start()
    {
        damageable.OnDamage += UpdateHealth;
        damageable.OnHeal += UpdateHealth;
    }

    public void SetDamageable(IDamageable _damageable)
    {
        if (damageable != null)
        {
            damageable.OnDamage -= UpdateHealth;
            damageable.OnHeal -= UpdateHealth;
        }

        damageable = _damageable;
        damageable.OnDamage += UpdateHealth;
        damageable.OnHeal += UpdateHealth;
    }

    private void UpdateHealth(float _health, GameObject _)
    {
        fillImage.fillAmount = Mathf.Clamp(_health / damageable.GetMaxHealth(), 0f, 1f);
    }
}