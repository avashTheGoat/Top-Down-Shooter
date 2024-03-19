using UnityEngine;

public class PlayerInventoryOpener : MonoBehaviour
{
    [SerializeField] private KeyCode openInventoryKey;
    [SerializeField] private GameObject inventory;

    private void Update()
    {
        if (Input.GetKeyDown(openInventoryKey))
            inventory.SetActive(!inventory.activeInHierarchy);
    }
}