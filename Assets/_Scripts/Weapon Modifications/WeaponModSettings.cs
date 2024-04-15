using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Modification", menuName = "Scriptable Objects/Weapon Mod")]
public class WeaponModSettings : ScriptableObject
{
    public float DamageMulti => damageMulti;
    public float AttacksPerSecondMulti => attacksPerSecondMulti;

    public float AccuracyMulti => accuracyMulti;
    public float ReloadTimeMulti => reloadTimeMulti;
    public float ProjectileRangeMulti => projectileRangeMulit;
    public float ProjectileSpeedMulti => projectileSpeedMulti;
    public int NumBulletsIncrease => numBulletsIncrease;
    public float MaxBulletsMulti => maxBulletsMulti;

    [SerializeField] private float damageMulti = 1f;
    [SerializeField] private float attacksPerSecondMulti = 1f;


    [Header("Ranged Weapon Fields")]
    [Tooltip("Improves accuracy for ranged weapons.")]
    [SerializeField] private float accuracyMulti = 1f;
    [SerializeField] private float reloadTimeMulti = 1f;
    [SerializeField] private float projectileRangeMulit = 1f;
    [SerializeField] private float projectileSpeedMulti = 1f;
    [SerializeField] private int numBulletsIncrease = 0;
    [SerializeField] private float maxBulletsMulti = 1f;
}