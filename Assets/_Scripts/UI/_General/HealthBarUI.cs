using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Component damageableComponent;

    private IDamageable damageable;

    private void Awake() => damageable = (IDamageable)damageableComponent;

    private void Start()
    {
        damageable.OnDamage += (_health, _) => fillImage.fillAmount = Mathf.Clamp(_health / damageable.GetMaxHealth(), 0f, 1f);
        damageable.OnHeal += (_health, _) => fillImage.fillAmount = Mathf.Clamp(_health / damageable.GetMaxHealth(), 0f, 1f);
    }
}