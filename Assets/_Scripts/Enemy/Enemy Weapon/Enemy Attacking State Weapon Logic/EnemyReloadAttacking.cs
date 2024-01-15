using UnityEngine;

public class EnemyReloadAttacking : MonoBehaviour, IReload
{
    public bool ShouldReload(RangedWeapon _weapon) => _weapon.Ammo <= 0;
}