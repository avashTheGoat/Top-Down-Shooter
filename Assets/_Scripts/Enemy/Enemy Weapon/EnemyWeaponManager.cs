using UnityEngine;

public class EnemyWeaponManager : MonoBehaviour
{
    [HideInInspector] public Weapon Weapon;

    private WeaponInitializer weaponInitializer;

    private void Awake() => weaponInitializer = GetComponent<WeaponInitializer>();

    private void Start()
    {
        weaponInitializer.InitializeWeaponWielder(Weapon);
        weaponInitializer.ApplyWeaponTransformChange(Weapon);
        weaponInitializer.InitializeTagsToIgnore(Weapon);
    }
}