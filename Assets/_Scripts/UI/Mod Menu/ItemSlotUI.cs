using UnityEngine;

public class ItemSlotUI : MonoBehaviour
{
    [field: SerializeField] public Item Item { get; private set; }
    [field: SerializeField] public ItemUI ItemUI { get; private set; }
    [field: SerializeField] public DragDropSlotUI SlotUI { get; private set; }

    #nullable enable
    public void SetItem(Item? _mod)
    #nullable disable
    {
        if (_mod == null)
        {
            ItemUI.ItemImage.sprite = null;
            return;
        }

        ItemUI.ItemImage.sprite = _mod.Image;
        Item = _mod;
    }
}