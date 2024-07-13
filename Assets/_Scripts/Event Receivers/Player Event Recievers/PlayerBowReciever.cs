using UnityEngine;
using Cinemachine;

public class PlayerBowReciever : MonoBehaviour
{
    [SerializeField] private PlayerWeaponsManager playerWeapons;

    [Header("Zoom Effect")]
    [SerializeField] private CinemachineVirtualCamera mainCam;
    [SerializeField] private float maxZoomOrthoSize;
    [Space(15)]

    [Header("Player Speed Change")]
    [SerializeField] private PlayerMovement playerMovement;
    [Range(0f, 1f)]
    [SerializeField] private float chargingSpeedPercent;

    private bool hasBowBeenFound = false;

    private float startingCameraSize;

    private void Start() => startingCameraSize = mainCam.m_Lens.OrthographicSize;

    private void Update()
    {
        if (hasBowBeenFound)
            return;

        playerWeapons.GetRangedWeapons().ForEach(_rangedWeapon =>
        { 
            if (_rangedWeapon is BowWeapon)
            {
                hasBowBeenFound = true;

                BowWeapon _bowWeapon = (BowWeapon)_rangedWeapon;

                _bowWeapon.OnBowCharge += (_maxCharge, _curCharge) =>
                {
                    float _t = Mathf.Clamp(_curCharge / _maxCharge, 0f, 1f);
                    mainCam.m_Lens.OrthographicSize = Mathf.Lerp(startingCameraSize, maxZoomOrthoSize, _t);

                    playerMovement.SetMovementSpeed(playerMovement.GetOriginalMovementSpeed() * chargingSpeedPercent);
                };

                _bowWeapon.OnAttack += _ => DoOnAttack();
                _bowWeapon.OnAttackWithoutAmmo += _ => DoOnAttack();
            }
        });
    }

    private void DoOnAttack()
    {
        mainCam.m_Lens.OrthographicSize = startingCameraSize;
        playerMovement.SetMovementSpeed(playerMovement.GetOriginalMovementSpeed());
    }
}