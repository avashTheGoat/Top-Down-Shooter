using UnityEngine;

[CreateAssetMenu(fileName = "Attack & Chase Visible Player", menuName = "Scriptable Objects/Enemy/Enemy States/Attack States/Attack & Chase Visible Player")]
public class EnemyAttackChaseVisiblePlayer : EnemyAttackStateLogicBaseSO
{
    private float initialSpeed;

    public override void DoEnterStateLogic()
    {
        base.DoEnterStateLogic();

        initialSpeed = agent.speed;
        SetWeaponLogic();
    }

    public override void DoPhysicsUpdateStateLogic() { }

    public override void DoUpdateLogic()
    {
        if (!ShouldBeInAttackState() || !IsPlayerVisible())
        {
            stateMachine.TransitionToState(stateMachine.ChaseState);
            return;
        }

        agent.SetDestination(player.position);
    }

    public override void DoExitStateLogic()
    {
        base.DoExitStateLogic();

        agent.speed = initialSpeed;
    }

    protected override void ResetValues() { }

    public override bool ShouldAttack(Weapon _weapon) => IsPlayerVisible();

    public override float GetWeaponRotationChange(Transform _weapon)
    {
        if (PlayerProvider.TryGetPlayer(out Transform _player))
        {
            Vector2 _playerDirection = _player.position - trans.position;
            Vector2 _weaponDirection = _weapon.position - trans.position;

            float _deltaAngle = Vector2.SignedAngle(_weaponDirection.normalized, _playerDirection.normalized);

            return _deltaAngle;
        }

        return 0f;
    }

    public override bool ShouldReload(RangedWeapon _weapon) => _weapon.Ammo <= 0;
}