using UnityEngine;

public class SwordAttackReciever : MonoBehaviour
{
    [SerializeField] private SwordWeapon sword;

    private void Start()
    {
        sword.OnWeaponAttack += DamageObjectsInCollider;
    }

    private void DamageObjectsInCollider()
    {
        foreach (GameObject _hitObject in sword.GetGameObjectsInAttackAOE())
        {
            if (_hitObject.Equals(sword.Wielder)) continue;

            if (_hitObject.TryGetComponent(out IDamageable _damageable))
            {
                _damageable.Damage(sword.WeaponDamage);
            }
        }
    }
}