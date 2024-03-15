using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyIdleStateLogicBaseSO : ScriptableObject
{
    protected EnemyStateMachine stateMachine;
    protected Transform trans;
    protected NavMeshAgent agent;
    protected Transform player;

    protected Weapon enemyWeapon;
    protected IAttack idleStateAttackLogic;
    #nullable enable
    protected IReload? idleStateReloadLogic;

    public EnemyIdleStateLogicBaseSO Initialize(EnemyStateMachine _stateMachine, Transform _transform, NavMeshAgent _agent,
    Transform _player, Weapon _enemyWeapon, IAttack _idleStateAttackLogic, IReload? _idleStateReloadLogic)
    #nullable disable
    {
        stateMachine = _stateMachine;
        trans = _transform;
        agent = _agent;
        player = _player;

        enemyWeapon = _enemyWeapon;
        idleStateAttackLogic = _idleStateAttackLogic;
        idleStateReloadLogic = _idleStateReloadLogic;

        return this;
    }

    public virtual void DoEnterStateLogic()
    {
        ResetValues();
    }

    public virtual void DoExitStateLogic()
    {
        ResetValues();
    }

    public abstract void DoUpdateLogic();
    public abstract void DoPhysicsUpdateStateLogic();
    protected abstract void ResetValues();
}