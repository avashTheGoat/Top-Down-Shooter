using UnityEngine;

public class HammerAttackReciever : MonoBehaviour
{
    [SerializeField] private HammerWeapon hammer;
    [SerializeField] private AnimationCurve percentOfOriginalDamageWhenXAwayFromCenter;

    private void Awake()
    {
        percentOfOriginalDamageWhenXAwayFromCenter.postWrapMode = WrapMode.Clamp;
        percentOfOriginalDamageWhenXAwayFromCenter.preWrapMode = WrapMode.Clamp;
    }

    private void Start()
    {
        hammer.OnWeaponAttack += DamageObjectsInCollider;
    }

    private void DamageObjectsInCollider()
    {
        foreach (GameObject _hitObject in hammer.GetGameObjectsInAttackAOE())
        {
            if (_hitObject.Equals(hammer.Wielder)) continue;

            if (_hitObject.TryGetComponent(out IDamageable _damageable))
            {
                float _distanceFromAttack = Vector2.Distance(hammer.GetAttackAOECenter(), _hitObject.transform.position);
                float _damage = TransformDamage(hammer.WeaponDamage, _distanceFromAttack);
                _damageable.Damage(_damage);

                print($"(Damage, DistanceFromCenter): ({_damage}, {_distanceFromAttack})");
            }
        }
    }

    private float TransformDamage(float _originalDamage, float _distanceFromCenter)
    {
        return _originalDamage * percentOfOriginalDamageWhenXAwayFromCenter.Evaluate(_distanceFromCenter);
    }
}