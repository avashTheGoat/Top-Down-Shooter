using UnityEngine;
using System.Collections.Generic;

public class WeaponModsInfoUI : MonoBehaviour
{
    [field: SerializeField] public Weapon Weapon { get; private set; }
    [field: SerializeField] public ItemUI ItemUI { get; private set; }
    [field: SerializeField] public List<ItemSlotUI> AttachedModUIs { get; private set; }

    public void SetWeapon(Weapon _weapon)
    {
        ItemUI.ItemImage.sprite = _weapon.Image;
        ItemUI.ItemName.text = _weapon.Name;
        Weapon = _weapon;
    }
}