using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyChaseStateLogicBaseSO : ScriptableObject, IAttack, IReload
{
    [Header("Raycast Fields")]
    [SerializeField] private int numRaycasts;
    [SerializeField] private float maxDistanceFromPlayerToChase;

    protected EnemyStateMachine stateMachine;
    protected NavMeshAgent agent;

    protected Transform trans;
    protected Transform player;

    protected Weapon weapon;

    protected SpriteRenderer spriteRenderer;
    protected Sprite leftMovingSprite;
    protected Sprite rightMovingSprite;

    private LayerMask ignoreLayers;

    protected Weapon enemyWeapon;

    public EnemyChaseStateLogicBaseSO Initialize(EnemyStateMachine _stateMachine, Transform _transform,
        NavMeshAgent _agent, Transform _player, LayerMask _ignoreLayers, Weapon _enemyWeapon,
        SpriteRenderer _spriteRenderer, Sprite _leftMovingSprite, Sprite _rightMovingSprite)
    {
        stateMachine = _stateMachine;
        agent = _agent;
        
        trans = _transform;
        player = _player;
        
        enemyWeapon = _enemyWeapon;

        spriteRenderer = _spriteRenderer;
        leftMovingSprite = _leftMovingSprite;
        rightMovingSprite = _rightMovingSprite;
        spriteRenderer.sprite = Random.Range(0, 2) == 0 ? leftMovingSprite : rightMovingSprite;

        ignoreLayers = _ignoreLayers;

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

    public virtual void DoUpdateLogic()
    {
        if (agent.velocity.x > 0)
            spriteRenderer.sprite = rightMovingSprite;

        else if (agent.velocity.x < 0)
            spriteRenderer.sprite = leftMovingSprite;
    }

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

    protected bool IsPlayerVisibleAndCloseEnough()
    {
        Physics2D.queriesHitTriggers = false;
        Vector2 _raycastDirection = (Vector2)agent.velocity == Vector2.zero ? Vector2.right : agent.velocity;

        for (int i = 0; i < numRaycasts; i++)
        {
            RaycastHit2D[] _raycastHits = Physics2D.RaycastAll(trans.position, _raycastDirection, maxDistanceFromPlayerToChase + Mathf.Epsilon, ~ignoreLayers);
            _raycastDirection = Quaternion.Euler(0, 0, 360 / numRaycasts) * _raycastDirection;

            if (_raycastHits.Length == 0)
                continue;

            if (_raycastHits[0].transform == player && Vector2.Distance(_raycastHits[0].point, trans.position) <= maxDistanceFromPlayerToChase)
            {
                Physics2D.queriesHitTriggers = true;
                return true;
            }
        }

        Physics2D.queriesHitTriggers = true;
        return false;
    }

    // IAttack and IReload
    public abstract bool ShouldAttack(Weapon _weapon);

    public abstract float GetWeaponRotationChange(Transform _weapon);

    public abstract bool ShouldReload(RangedWeapon _weapon);
}