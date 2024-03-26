[System.Serializable]
public struct ResourceAmount
{
    public ResourceSO Resource;
    public int Amount;

    public ResourceAmount(ResourceSO _resource, int _amount)
    {
        Resource = _resource;
        Amount = _amount;
    }
}