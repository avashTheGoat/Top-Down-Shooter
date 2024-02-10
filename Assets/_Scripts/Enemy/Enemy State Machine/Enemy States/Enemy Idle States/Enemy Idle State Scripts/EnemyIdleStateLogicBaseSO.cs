using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyIdleStateLogicBaseSO : ScriptableObject
{
    protected EnemyStateMachine stateMachine;
    protected Transform trans;
    protected NavMeshAgent agent;
    protected Transform player;
    protected Collider2D detectionCollider;

    protected Weapon enemyWeapon;
    protected IAttack idleStateAttackLogic;
    protected IReload? idleStateReloadLogic;

    public EnemyIdleStateLogicBaseSO Initialize(EnemyStateMachine _stateMachine, Transform _transform, NavMeshAgent _agent,
        Transform _player, Collider2D _detectionCollider, Weapon _enemyWeapon, IAttack _idleStateAttackLogic,
        IReload? _idleStateReloadLogic)
    {
        stateMachine = _stateMachine;
        trans = _transform;
        agent = _agent;
        player = _player;
        detectionCollider = _detectionCollider;

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

    protected bool IsPointInCollider(Collider2D _collider, Vector2 _point)
    {
        Collider2D[] _hits = Physics2D.OverlapPointAll(_point);

        foreach (Collider2D _hit in _hits)
        {
            if (_hit == _collider)
            {
                return true;
            }
        }

        return false;
    }
}