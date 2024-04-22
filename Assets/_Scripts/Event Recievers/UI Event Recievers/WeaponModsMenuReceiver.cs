using UnityEngine;

// listens for mod attachment. Updates a weapon's array of mods.
// also needs to update player's weapon mods inventory
public class WeaponModsMenuReceiver : MonoBehaviour
{
    [SerializeField] private ModMenuUIManager modMenuManager;

    private void Start()
    {
        modMenuManager.OnModAttach += (_, _) =>
        {
            
        };
    }
}