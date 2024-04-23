public class Resource : Item
{
    public override int GetHashCode() => 31 * Name.GetHashCode();

    public override bool Equals(object other)
    {
        if (other.GetType() != typeof(Resource))
            return false;

        Resource _casted = other as Resource;
        return Name == _casted.Name;
    }

    public override string ToString() => Name;
}