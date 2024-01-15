public class EnemyIdleState : BaseState
{
    private EnemyIdleStateLogicBaseSO idleStateLogic;

    public EnemyIdleState(EnemyIdleStateLogicBaseSO _idleStateLogic)
    {
        idleStateLogic = _idleStateLogic;
    }

    public override void EnterState()
    {
        idleStateLogic.DoEnterStateLogic();
    }

    public override void ExitState()
    {
        idleStateLogic.DoExitStateLogic();
    }

    public override void PhysicsUpdateState()
    {
        idleStateLogic.DoPhysicsUpdateStateLogic();
    }

    public override void UpdateState()
    {
        idleStateLogic.DoUpdateLogic();
    }
}