using UnityEngine;

public class EnemyAttackWandering : MonoBehaviour, IAttack
{
    private Transform trans;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trans = transform;
    }

    public float GetWeaponRotationChange(Transform _weapon)
    {
        Vector2 _movementDirection = rb.velocity.normalized;
        Vector2 _weaponDirection = (_weapon.position - trans.position).normalized;

        float _deltaAngle = Vector2.SignedAngle(_weaponDirection, _movementDirection);

        return _deltaAngle;
    }

    public bool ShouldAttack(Weapon _weapon) => false;
}