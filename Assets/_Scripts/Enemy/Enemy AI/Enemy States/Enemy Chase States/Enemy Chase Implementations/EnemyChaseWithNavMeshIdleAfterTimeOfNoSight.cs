using UnityEngine;

[CreateAssetMenu(fileName = "Chase Directly Using NavMesh", menuName = "Scriptable Objects/Enemy/Enemy States/Chase States/Chase Directly Using NavMesh")]
public class EnemyChaseWithNavMeshIdleAfterTimeOfNoSight : EnemyChaseStateLogicBaseSO
{
    #region Chase Modifiers
    [Header("Chase Modifiers")]
    [SerializeField] private float chaseSpeedMultiplier;
    [SerializeField] private float minTimeToWanderAfterDetectionExit;
    [SerializeField] private float maxTimeToWanderAfterDetectionExit;
    [Space(15)]
    #endregion

    private float timeToWanderAfterDetectionExit;
    private float timeAfterDetectionExit = 0f;
    private bool isFirstFrameAfterDetectionExit = true;

    public override void DoEnterStateLogic()
    {
        isFirstFrameAfterDetectionExit = true;
        timeAfterDetectionExit = 0f;
        agent.speed *= chaseSpeedMultiplier;

        SetWeaponLogic();
    }

    public override void DoPhysicsUpdateStateLogic()
    {
        throw new System.NotImplementedException();
    }

    public override void DoUpdateLogic()
    {
        if (ShouldBeInAttackState())
        {
            stateMachine.TransitionToState(stateMachine.AttackState);
            return;
        }

        Vector2 _playerPosition = Vector2.zero;
        if (player != null) _playerPosition = player.position;

        if (IsPlayerVisibleAndCloseEnough())
        {
            timeAfterDetectionExit = 0f;
            isFirstFrameAfterDetectionExit = true;
        }

        else
        {
            if (isFirstFrameAfterDetectionExit)
            {
                timeToWanderAfterDetectionExit = Random.Range(minTimeToWanderAfterDetectionExit, maxTimeToWanderAfterDetectionExit);
                timeAfterDetectionExit = 0f;
                isFirstFrameAfterDetectionExit = false;
            }

            timeAfterDetectionExit += Time.deltaTime;

            if (timeAfterDetectionExit >= timeToWanderAfterDetectionExit)
            {
                stateMachine.TransitionToState(stateMachine.IdleState);
                return;
            }
        }

        agent.SetDestination(_playerPosition);
    }

    protected override void ResetValues()
    {
        isFirstFrameAfterDetectionExit = true;
        timeAfterDetectionExit = 0f;
        agent.speed /= chaseSpeedMultiplier;
    }

    public override float GetWeaponRotationChange(Transform _weapon)
    {
        if (PlayerProvider.TryGetPlayer(out Transform _player))
        {
            Vector2 _playerDirection = (_player.position - trans.position).normalized;
            Vector2 _weaponDirection = (_weapon.position - trans.position).normalized;

            float _deltaAngle = Vector2.SignedAngle(_weaponDirection, _playerDirection);

            return _deltaAngle;
        }

        return 0f;
    }

    public override bool ShouldAttack(Weapon _weapon) => false;

    public override bool ShouldReload(RangedWeapon _weapon) => _weapon.Ammo <= _weapon.MaxAmmo / 2;
}