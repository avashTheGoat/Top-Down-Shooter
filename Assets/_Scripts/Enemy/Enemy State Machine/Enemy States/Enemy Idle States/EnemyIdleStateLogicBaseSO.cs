using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public abstract class EnemyIdleStateLogicBaseSO : ScriptableObject
{
    protected EnemyStateMachine stateMachine;
    protected Transform trans;
    protected NavMeshAgent agent;
    protected Transform player;
    protected List<Transform> enemies;

    protected Weapon enemyWeapon;
    protected IAttack idleStateAttackLogic;
    #nullable enable
    protected IReload? idleStateReloadLogic;

    public EnemyIdleStateLogicBaseSO Initialize(EnemyStateMachine _stateMachine, Transform _transform, NavMeshAgent _agent,
    Transform _player, List<Transform> _enemies, Weapon _enemyWeapon, IAttack _idleStateAttackLogic,
    IReload? _idleStateReloadLogic)
    #nullable disable
    {
        stateMachine = _stateMachine;
        trans = _transform;
        agent = _agent;
        player = _player;
        enemies = _enemies;

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

    protected void SetWeaponLogic()
    {
        if (enemyWeapon is RangedWeapon)
        {
            RangedWeapon _enemyRangedWeapon = (RangedWeapon)enemyWeapon;
            _enemyRangedWeapon.SetWeaponLogic(idleStateAttackLogic, idleStateReloadLogic);
        }

        else if (enemyWeapon is MeleeWeapon)
            enemyWeapon.SetWeaponLogic(idleStateAttackLogic);

        else
            throw new System.Exception("Unrecognized weapon type.");
    }
}