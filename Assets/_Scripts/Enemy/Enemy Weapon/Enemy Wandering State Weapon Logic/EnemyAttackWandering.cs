using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAttackWandering : MonoBehaviour, IAttack
{
    private Transform trans;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        trans = transform;
    }

    public float GetWeaponRotationChange(Transform _weapon)
    {
        Vector2 _movementDirection = agent.velocity.normalized;
        Vector2 _weaponDirection = (_weapon.position - trans.position).normalized;

        float _deltaAngle = Vector2.SignedAngle(_weaponDirection, _movementDirection);

        return _deltaAngle;
    }

    public bool ShouldAttack(Weapon _weapon) => false;
}