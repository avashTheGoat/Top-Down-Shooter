using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsProvider : MonoBehaviour, IWeaponProvider
{
    public List<T> GetWeapons<T>() where T : Weapon
    {
        List<T> _weapons = new();
        GetComponentsInChildren(true, _weapons);

        return _weapons;
    }
}