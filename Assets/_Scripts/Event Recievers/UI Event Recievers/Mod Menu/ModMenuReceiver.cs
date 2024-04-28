using UnityEngine;

public class ModMenuReceiver : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private ModMenuUIManager modMenuManager;

    private void Start()
    {
        modMenuManager.OnModAttach += (_mod, _weapon) =>
        {
            _mod.ApplyMod(_weapon);
        };

        modMenuManager.OnModDeattach += (_mod, _weapon) =>
        {
            _mod.UnapplyMod(_weapon);
        };
    }
}