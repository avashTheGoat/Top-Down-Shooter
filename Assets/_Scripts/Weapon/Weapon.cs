using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour
{
    public event Action OnWeaponAttack;
    public float WeaponDamage => weaponDamage;
    public IAttack AttackLogic;

    public Transform Wielder { get; private set; }

    [Header("Attacking")]
    [SerializeField] protected float weaponDamage;
    [SerializeField] protected float ATTACKS_PER_SECOND;
    [Space(15)]

    protected Transform trans;

    protected float attackCooldownTimer;

    protected virtual void Awake()
    {
        trans = GetComponent<Transform>();
        attackCooldownTimer = GetResetAttackTimer();
    }

    protected virtual void Update()
    {
        attackCooldownTimer -= Time.deltaTime;
        attackCooldownTimer = Mathf.Clamp(attackCooldownTimer, 0, GetResetAttackTimer());
    }

    public virtual void Init(Transform _wielder, IAttack _attackLogic)
    {
        if (_wielder is null) throw new ArgumentNullException(nameof(_wielder), "The passed in Transform should not be null.");
        if (_attackLogic is null) throw new ArgumentNullException(nameof(_attackLogic), "The passed in IAttack should not be null.");

        Wielder = _wielder;
        AttackLogic = _attackLogic;
    }

    protected virtual void Attack()
    {
        
    }

    protected float GetResetAttackTimer() => attackCooldownTimer = 1 / ATTACKS_PER_SECOND;
    protected void InvokeOnWeaponAttack() => OnWeaponAttack?.Invoke();
}