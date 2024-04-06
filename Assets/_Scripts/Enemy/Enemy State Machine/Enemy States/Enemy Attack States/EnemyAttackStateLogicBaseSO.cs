using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public abstract class EnemyAttackStateLogicBaseSO : ScriptableObject, IAttack, IReload
{
    [Tooltip("A float from 0 to 1 representing the percentage of the normal speed that the enemy will move at while attacking.")]
    [Range(0f, 1f)]
    [SerializeField] protected float attackingSpeedPercent;

    [Header("Player Visibility Raycast Fields")]
    [Tooltip("The number of raycasts that should be created to check for player visibility")]
    [SerializeField] private int numRaycasts;
    [SerializeField] private float maxRaycastDistance;

    protected EnemyStateMachine stateMachine;
    protected Transform trans;
    protected NavMeshAgent agent;
    protected Transform player;
    protected Weapon enemyWeapon;
    private LayerMask ignoreLayers;

    public EnemyAttackStateLogicBaseSO Initialize(EnemyStateMachine _stateMachine, Transform _transform, NavMeshAgent _agent,
    LayerMask _ignoreLayers, Transform _player, Weapon _enemyWeapon)
    #nullable disable
    {
        stateMachine = _stateMachine;
        trans = _transform;
        agent = _agent;
        player = _player;
        enemyWeapon = _enemyWeapon;;
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

    public abstract void DoUpdateLogic();

    public abstract void DoPhysicsUpdateStateLogic();

    protected abstract void ResetValues();

    protected bool ShouldBeInAttackState()
    {
        if (enemyWeapon is RangedWeapon)
        {
            RangedWeapon _rangedWeapon = enemyWeapon as RangedWeapon;

            if (player != null)
                return Vector2.Distance(player.position, _rangedWeapon.transform.position) <= _rangedWeapon.Range
                    && IsPlayerVisible();
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
            RangedWeapon _enemyRangedWeapon = (RangedWeapon) enemyWeapon;
            _enemyRangedWeapon.SetWeaponLogic(this, this);
        }

        else if (enemyWeapon is MeleeWeapon)
            enemyWeapon.SetWeaponLogic(this);

        else
            throw new System.Exception("Unrecognized weapon type.");
    }

    protected bool IsPlayerVisible()
    {
        Physics2D.queriesHitTriggers = false;
        Vector2 _raycastDirection = (Vector2)agent.velocity == Vector2.zero ? Vector2.right : agent.velocity;

        for (int i = 0; i < numRaycasts; i++)
        {
            RaycastHit2D[] _raycastHits = Physics2D.RaycastAll(trans.position, _raycastDirection, maxRaycastDistance + Mathf.Epsilon, ~ignoreLayers);
            
            _raycastDirection = Quaternion.Euler(0, 0, 360 / numRaycasts) * _raycastDirection;

            if (_raycastHits.Length == 0)
                continue;

            if (_raycastHits[0].transform == player)
            {
                Physics2D.queriesHitTriggers = true;
                return true;
            }
        }

        Physics2D.queriesHitTriggers = true;
        // MonoBehaviour.print("Cannot see player");
        return false;
    }

    // IAttack and IReload
    public abstract bool ShouldAttack(Weapon _weapon);

    public abstract float GetWeaponRotationChange(Transform _weapon);

    public abstract bool ShouldReload(RangedWeapon _weapon);
}