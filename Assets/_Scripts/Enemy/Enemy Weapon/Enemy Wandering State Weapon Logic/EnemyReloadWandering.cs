using UnityEngine;

public class EnemyReloadWandering : MonoBehaviour, IReload
{
    public bool ShouldReload(RangedWeapon _weapon) => _weapon.Ammo < _weapon.MaxAmmo;
}