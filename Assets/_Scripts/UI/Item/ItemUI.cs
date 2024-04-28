using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    [field: SerializeField] public Item Item { get; private set; }

    [field: SerializeField] public Image ItemImage { get; private set; }
    [field: SerializeField] public TextMeshProUGUI ItemName { get; private set; }
    [field: SerializeField] public TextMeshProUGUI ItemDescription { get; private set; }

    public void SetItem(Item _item)
    {
        Item = _item;

        if (ItemImage != null)
            ItemImage.sprite = _item.Image;
        if (ItemName != null)
            ItemName.text = _item.Name;
        if (ItemDescription != null)
            ItemDescription.text = _item.Description;
    }
}