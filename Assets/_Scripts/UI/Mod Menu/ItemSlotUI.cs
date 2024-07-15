using UnityEngine;

public class ItemSlotUI : MonoBehaviour
{
    [field: SerializeField] public Item Item { get; private set; }
    [field: SerializeField] public ItemUI ItemUI { get; private set; }
    [field: SerializeField] public DragDropSlotUI SlotUI { get; private set; }

    public void SetItem(Item _mod)
    {
        ItemUI.ItemImage.sprite = _mod != null ? _mod.Image : null;
        Item = _mod;
    }
}