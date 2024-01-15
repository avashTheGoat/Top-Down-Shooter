using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerSprintStamina : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float maxStamina;
    [Min(0)]
    [SerializeField] private float staminaIncreasePerSec;
    [Min(0)]
    [SerializeField] private float staminaDecreasePerSec;

    private PlayerMovement playerMovement;
    private float stamina;
    private bool hasRunOutOfStaminaWhileSprinting;

    private void Awake()
    {
        stamina = maxStamina;
        playerMovement = GetComponent<PlayerMovement>();
        hasRunOutOfStaminaWhileSprinting = false;
    }

    private void Update()
    {
        //hasRunOutOfStaminaWhileSprinting is to prevent player from sprinting (and losing stamina) if they have ran out of stamina
        //while holding down shift until either they reached max stamina (hasRunOutOfStaminaWhileSprinting = stamina != maxStamina)
        //or the player stopped pressing shift

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!hasRunOutOfStaminaWhileSprinting)
            {
                stamina -= staminaDecreasePerSec * Time.deltaTime;

                if (stamina <= 0f)
                {
                    hasRunOutOfStaminaWhileSprinting = true;
                }
            }
        }

        if (!Input.GetKey(KeyCode.LeftShift) || hasRunOutOfStaminaWhileSprinting)
        {
            stamina += staminaIncreasePerSec * Time.deltaTime;
            
            if (hasRunOutOfStaminaWhileSprinting)
            {
                hasRunOutOfStaminaWhileSprinting = stamina != maxStamina;
            }

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                hasRunOutOfStaminaWhileSprinting = false;
            }
        }

        stamina = Mathf.Clamp(stamina, 0f, maxStamina);

        playerMovement.CanSprint = stamina > 0f && !hasRunOutOfStaminaWhileSprinting;
    }
}