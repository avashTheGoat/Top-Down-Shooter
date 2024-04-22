using UnityEngine;

public class ModUI : MonoBehaviour
{
    [field: SerializeField] public WeaponMod Mod { get; private set; }
    [field: SerializeField] public ItemUI WeaponModUI { get; private set; }

    #nullable enable
    public void SetMod(WeaponMod? _mod)
    #nullable disable
    {
        if (_mod == null)
        {
            WeaponModUI.ItemImage.sprite = null;
            return;
        }

        WeaponModUI.ItemImage.sprite = _mod.Image;
        Mod = _mod;
    }
}