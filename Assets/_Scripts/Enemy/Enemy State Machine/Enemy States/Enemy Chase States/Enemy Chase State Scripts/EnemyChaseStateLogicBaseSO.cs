using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyChaseStateLogicBaseSO : ScriptableObject
{
    protected EnemyStateMachine stateMachine;
    protected Transform trans;
    protected NavMeshAgent agent;
    protected Transform player;
    protected Collider2D detectionCollider;
    protected Collider2D attackCollider;

    public EnemyChaseStateLogicBaseSO Initialize(EnemyStateMachine _stateMachine, Transform _transform, NavMeshAgent _agent,
        Transform _player, Collider2D _detectionCollider, Collider2D _attackCollider)
    {
        stateMachine = _stateMachine;
        trans = _transform;
        agent = _agent;
        player = _player;
        detectionCollider = _detectionCollider;
        attackCollider = _attackCollider;

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