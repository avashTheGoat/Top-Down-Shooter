using UnityEngine;

public interface IAttack
{
    bool ShouldAttack(Weapon _weapon);
    float GetWeaponRotationChange(Transform _weapon);
}