using System.Collections.Generic;

public interface IWeaponProvider
{
    List<T> GetWeapons<T>() where T : Weapon;
}