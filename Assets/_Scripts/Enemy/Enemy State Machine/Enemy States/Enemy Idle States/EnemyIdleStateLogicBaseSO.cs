using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public abstract class EnemyIdleStateLogicBaseSO : ScriptableObject, IAttack, IReload
{
    protected EnemyStateMachine stateMachine;
    protected Transform trans;
    protected NavMeshAgent agent;
    protected Transform player;
    protected List<Transform> enemies;

    protected Weapon enemyWeapon;

    public EnemyIdleStateLogicBaseSO Initialize(EnemyStateMachine _stateMachine, Transform _transform, NavMeshAgent _agent,
    Transform _player, List<Transform> _enemies, Weapon _enemyWeapon)
    {
        stateMachine = _stateMachine;
        trans = _transform;
        agent = _agent;
        player = _player;
        enemies = _enemies;

        enemyWeapon = _enemyWeapon;

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
            _enemyRangedWeapon.SetWeaponLogic(this, this);
        }

        else if (enemyWeapon is MeleeWeapon)
            enemyWeapon.SetWeaponLogic(this);

        else
            throw new System.Exception("Unrecognized weapon type.");
    }

    // IAttack and IReload
    public abstract bool ShouldAttack(Weapon _weapon);

    public abstract float GetWeaponRotationChange(Transform _weapon);

    public abstract bool ShouldReload(RangedWeapon _weapon);
}