using UnityEngine;

public class Item : MonoBehaviour
{
    public string Name => itemName;
    public string Description => description;
    public Sprite Image => image;

    [Header("Item Info")]
    [SerializeField] protected string itemName;
    [SerializeField] protected string description;
    [SerializeField] protected Sprite image;

    public override bool Equals(object other)
    {
        Item _item = (Item)other;
        return _item.Name == Name;
    }

    public override int GetHashCode() => Name.GetHashCode();
}