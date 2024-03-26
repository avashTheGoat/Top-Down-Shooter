using UnityEngine;
using System;

public abstract class Weapon : Item
{
    public event Action<GameObject> OnWeaponAttack;
    public float WeaponDamage => weaponDamage;

    public Transform Wielder { get; private set; }

    [Header("Attacking")]
    [SerializeField] protected float weaponDamage;
    [SerializeField] protected float ATTACKS_PER_SECOND;
    [Space(15)]

    protected IAttack attackLogic;
    
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

        trans.RotateAround(Wielder.position, Vector3.forward, attackLogic.GetWeaponRotationChange(trans));
    }

    public void SetWeaponLogic(IAttack _attackLogic)
    {
        if (_attackLogic == null)
            throw new ArgumentNullException(nameof(_attackLogic), "The _attackLogic parameter cannot be null for a Weapon.");

        attackLogic = _attackLogic;
    }

    public void SetWielder(Transform _wielder)
    {
        if (_wielder == null)
            throw new ArgumentNullException(nameof(_wielder), $"{nameof(_wielder)} cannot be null.");

        Wielder = _wielder;
    }

    protected virtual void Attack()
    {
        
    }

    protected float GetResetAttackTimer() => attackCooldownTimer = 1 / ATTACKS_PER_SECOND;
    protected void InvokeOnWeaponAttack() => OnWeaponAttack?.Invoke(gameObject);
}