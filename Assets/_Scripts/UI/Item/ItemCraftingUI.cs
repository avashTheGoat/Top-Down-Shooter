using UnityEngine;
using UnityEngine.UI;

public class ItemCraftingUI : MonoBehaviour
{
    [field: SerializeField] public ItemUI ItemUI { get; private set; }
    [field: SerializeField] public RectTransform ItemStatsUI { get; private set; }
    [field: SerializeField] public RectTransform ItemRequirementsUI { get; private set; }
    [field: SerializeField] public CraftingRequirementsManager RequirementsManager { get; private set; }
    [field: SerializeField] public Button CraftButton { get; private set; }
}