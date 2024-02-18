using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyChaseStateLogicBaseSO : ScriptableObject
{
    protected EnemyStateMachine stateMachine;
    protected Transform trans;
    protected NavMeshAgent agent;
    protected Transform player;

    protected Weapon enemyWeapon;
    protected IAttack chaseStateAttackLogic;
    #nullable enable
    protected IReload? chaseStateReloadLogic;

    public EnemyChaseStateLogicBaseSO Initialize(EnemyStateMachine _stateMachine, Transform _transform,
    NavMeshAgent _agent, Transform _player, Weapon _enemyWeapon, IAttack _chaseStateAttackLogic, IReload? _chaseStateReloadLogic)
    #nullable disable
    {
        stateMachine = _stateMachine;
        trans = _transform;
        agent = _agent;
        player = _player;

        enemyWeapon = _enemyWeapon;
        chaseStateAttackLogic = _chaseStateAttackLogic;
        chaseStateReloadLogic = _chaseStateReloadLogic;

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

    protected bool ShouldBeInAttackState()
    {
        if (enemyWeapon is RangedWeapon)
        {
            RangedWeapon _rangedWeapon = enemyWeapon as RangedWeapon;

            if (player != null)
                return Vector2.Distance(player.position, _rangedWeapon.transform.position) <= _rangedWeapon.Range;
        }

        else if (enemyWeapon is MeleeWeapon)
        {
            MeleeWeapon _meleeWeapon = enemyWeapon as MeleeWeapon;

            if (player != null)
                return _meleeWeapon.GetGameObjectsInAttackAOE().Contains(player.gameObject);
        }

        else Debug.LogError("An unidentified weapon has been detected.");

        return false;
    }
}