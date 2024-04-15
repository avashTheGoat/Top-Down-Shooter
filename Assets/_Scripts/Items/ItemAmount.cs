[System.Serializable]
public struct ItemAmount
{
    public Item Item;
    public int Amount;

    public ItemAmount(Item _resource, int _amount)
    {
        Item = _resource;
        Amount = _amount;
    }
}