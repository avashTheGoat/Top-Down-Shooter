using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerSprintStamina : MonoBehaviour
{
    [field: SerializeField] public float MaxStamina { get; private set; }
    public float Stamina { get; private set; }

    [Min(0)]
    [SerializeField] private float staminaIncreasePerSec;
    [Min(0)]
    [SerializeField] private float staminaDecreasePerSec;

    private PlayerMovement playerMovement;
    private bool hasRunOutOfStaminaWhileSprinting;

    private void Awake()
    {
        Stamina = MaxStamina;
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
                Stamina -= staminaDecreasePerSec * Time.deltaTime;

                if (Stamina <= 0f)
                {
                    hasRunOutOfStaminaWhileSprinting = true;
                }
            }
        }

        if (!Input.GetKey(KeyCode.LeftShift) || hasRunOutOfStaminaWhileSprinting)
        {
            Stamina += staminaIncreasePerSec * Time.deltaTime;
            
            if (hasRunOutOfStaminaWhileSprinting)
            {
                hasRunOutOfStaminaWhileSprinting = Stamina != MaxStamina;
            }

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                hasRunOutOfStaminaWhileSprinting = false;
            }
        }

        Stamina = Mathf.Clamp(Stamina, 0f, MaxStamina);

        playerMovement.CanSprint = Stamina > 0f && !hasRunOutOfStaminaWhileSprinting;
    }
}