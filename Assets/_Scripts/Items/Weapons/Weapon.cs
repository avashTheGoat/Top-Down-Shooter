using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Weapon : Item
{
    public event Action<GameObject> OnAttack;


    public Transform Wielder { get; private set; }

    public List<string> TagsToIgnore { get; protected set; } = new();

    [Header("Attacking")]
    public float Damage;
    public float AttacksPerSecond;
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

    protected float GetResetAttackTimer() => attackCooldownTimer = 1 / AttacksPerSecond;
    protected void InvokeOnWeaponAttack() => OnAttack?.Invoke(gameObject);
}