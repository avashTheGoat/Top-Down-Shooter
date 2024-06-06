using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyIdleStateLogicBaseSO : ScriptableObject, IAttack, IReload
{
    protected EnemyStateMachine stateMachine;
    protected NavMeshAgent agent;

    protected Transform trans;
    protected Transform player;

    protected Weapon weapon;

    protected SpriteRenderer spriteRenderer;
    protected Sprite leftMovingSprite;
    protected Sprite rightMovingSprite;

    public EnemyIdleStateLogicBaseSO Initialize(EnemyStateMachine _stateMachine, Transform _transform,
        NavMeshAgent _agent, Transform _player, Weapon _enemyWeapon, SpriteRenderer _spriteRenderer,
        Sprite _leftMovingSprite, Sprite _rightMovingSprite)
    {
        stateMachine = _stateMachine;
        agent = _agent;

        trans = _transform;
        player = _player;

        weapon = _enemyWeapon;

        spriteRenderer = _spriteRenderer;
        leftMovingSprite = _leftMovingSprite;
        rightMovingSprite = _rightMovingSprite;
        spriteRenderer.sprite = Random.Range(0, 2) == 0 ? leftMovingSprite : rightMovingSprite;

        return this;
    }

    public virtual void DoEnterStateLogic() => ResetValues();

    public virtual void DoExitStateLogic() => ResetValues();

    public virtual void DoUpdateLogic()
    {
        if (agent.velocity.x > 0)
            spriteRenderer.sprite = rightMovingSprite;

        else if (agent.velocity.x < 0)
            spriteRenderer.sprite = leftMovingSprite;
    }

    public abstract void DoPhysicsUpdateStateLogic();

    protected abstract void ResetValues();

    protected void SetWeaponLogic()
    {
        if (weapon is RangedWeapon)
        {
            RangedWeapon _enemyRangedWeapon = (RangedWeapon)weapon;
            _enemyRangedWeapon.SetWeaponLogic(this, this);
        }

        else if (weapon is MeleeWeapon)
            weapon.SetWeaponLogic(this);

        else
            throw new System.Exception("Unrecognized weapon type.");
    }

    // IAttack and IReload
    public abstract bool ShouldAttack(Weapon _weapon);

    public abstract float GetWeaponRotationChange(Transform _weapon);

    public abstract bool ShouldReload(RangedWeapon _weapon);
}