using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Weapon : Item
{
    public event Action<GameObject> OnAttack;

    public List<string> TagsToIgnore { get; protected set; } = new();
    
    public Transform Wielder { get; private set; }
    
    public float Damage => damage;
    public float AttacksPerSecond => attacksPerSecond;

    [Header("Attacking")]
    [SerializeField] protected float damage;
    [SerializeField] protected float attacksPerSecond;
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

    public void SetTagsToIgnore(List<string> _tagsToIgnore)
    {
        if (_tagsToIgnore == null)
            throw new ArgumentNullException(nameof(_tagsToIgnore));

        TagsToIgnore = _tagsToIgnore;
    }

    public abstract void ResetWeapon();

    protected virtual void Attack()
    {
        
    }

    public virtual void SetDamage(float _newDamage)
    {
        if (_newDamage < 0)
            throw new ArgumentException("Damage cannot be negative.");

        damage = _newDamage;
    }

    public virtual void SetAttacksPerSecond(float _newAttacksPerSecond)
    {
        if (_newAttacksPerSecond < 0)
            throw new ArgumentException("Attacks per second cannot be negative.");

        attacksPerSecond = _newAttacksPerSecond;
    }

    protected float GetResetAttackTimer() => attackCooldownTimer = 1 / attacksPerSecond;
    protected void InvokeOnWeaponAttack() => OnAttack?.Invoke(gameObject);
}