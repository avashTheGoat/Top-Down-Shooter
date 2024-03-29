using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponReloadUI : MonoBehaviour
{
    [SerializeField] private Image reloadImage;

    private RangedWeapon curRangedWeapon;

    private void Update()
    {
        if (curRangedWeapon == null)
            return;

        float _reloadProgress = (curRangedWeapon.ReloadTime - curRangedWeapon.CurReloadTimeLeft) / curRangedWeapon.ReloadTime;
        reloadImage.fillAmount = Mathf.Clamp(_reloadProgress, 0f, 1f);
    }

    public void SetFillAmount(float _fillAmount)
    {
        if (_fillAmount < 0f || _fillAmount > 1f)
            throw new System.ArgumentException($"The {nameof(_fillAmount)} cannot be less than 0 " +
                $"or greater than 1. It is {_fillAmount}.", nameof(_fillAmount));

        reloadImage.fillAmount = _fillAmount;
    }

    public void SetRangedWeapon(RangedWeapon _rangedWeapon) => curRangedWeapon = _rangedWeapon;
}