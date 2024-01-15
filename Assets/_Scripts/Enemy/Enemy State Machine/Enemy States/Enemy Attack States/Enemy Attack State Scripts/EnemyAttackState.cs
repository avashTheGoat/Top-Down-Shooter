public class EnemyAttackState : BaseState
{
    private EnemyAttackStateLogicBaseSO attackStateLogic;

    public EnemyAttackState(EnemyAttackStateLogicBaseSO _idleStateLogic)
    {
        attackStateLogic = _idleStateLogic;
    }

    public override void EnterState()
    {
        attackStateLogic.DoEnterStateLogic();
    }

    public override void ExitState()
    {
        attackStateLogic.DoExitStateLogic();
    }

    public override void PhysicsUpdateState()
    {
        attackStateLogic.DoPhysicsUpdateStateLogic();
    }

    public override void UpdateState()
    {
        attackStateLogic.DoUpdateLogic();
    }
}