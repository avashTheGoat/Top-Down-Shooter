using UnityEngine;

public class EnemyReloadChasing : MonoBehaviour, IReload
{
    public bool ShouldReload(RangedWeapon _weapon) => _weapon.Ammo <= _weapon.MaxAmmo / 2;
}