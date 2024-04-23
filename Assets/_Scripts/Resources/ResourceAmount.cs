[System.Serializable]
public struct ResourceAmount
{
    public Resource Resource;
    public int Amount;

    public ResourceAmount(Resource _resource, int _amount)
    {
        Resource = _resource;
        Amount = _amount;
    }
}