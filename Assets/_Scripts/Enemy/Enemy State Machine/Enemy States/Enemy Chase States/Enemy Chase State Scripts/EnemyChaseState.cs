public class EnemyChaseState : BaseState
{
    private EnemyChaseStateLogicBaseSO chaseStateLogic;

    public EnemyChaseState(EnemyChaseStateLogicBaseSO _idleStateLogic)
    {
        chaseStateLogic = _idleStateLogic;
    }

    public override void EnterState()
    {
        chaseStateLogic.DoEnterStateLogic();
    }

    public override void ExitState()
    {
        chaseStateLogic.DoExitStateLogic();
    }

    public override void PhysicsUpdateState()
    {
        chaseStateLogic.DoPhysicsUpdateStateLogic();
    }

    public override void UpdateState()
    {
        chaseStateLogic.DoUpdateLogic();
    }
}