using UnityEngine;

[CreateAssetMenu(fileName = "Attack And Chase Player", menuName = "Scriptable Objects/Enemy States/Attack States/Attack and Chase")]
public class EnemyAttackChasePlayer : EnemyAttackStateLogicBaseSO
{
    private bool isFirstFrame = true;

    private float initialSpeed;

    public override void DoEnterStateLogic()
    {
        base.DoEnterStateLogic();

        initialSpeed = agent.speed;
    }

    public override void DoPhysicsUpdateStateLogic()
    {

    }

    public override void DoUpdateLogic()
    {
        if (!ShouldBeInAttackState())
        {
            stateMachine.TransitionToState(stateMachine.ChaseState);
            return;
        }

        if (isFirstFrame)
        {
            enemyWeapon.ChangeWeaponLogic(attackingStateAttackLogic, attackingStateReloadLogic);
            agent.speed = initialSpeed * attackingSpeedPercent;

            isFirstFrame = false;
        }

        agent.SetDestination(player.position);
    }

    protected override void ResetValues()
    {
        isFirstFrame = true;
    }

    public override void DoExitStateLogic()
    {
        base.DoExitStateLogic();

        agent.speed = initialSpeed;
    }
}