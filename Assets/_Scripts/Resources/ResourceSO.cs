using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Scriptable Objects/Resources/New Resource")]
public class ResourceSO : ScriptableObject
{
    [field:SerializeField] public Sprite ResourceImage { get; private set; }
    [field: SerializeField] public string ResourceName { get; private set; }

    public override int GetHashCode() => 31 * ResourceName.GetHashCode();

    public override bool Equals(object other)
    {
        if (other.GetType() != typeof(ResourceSO))
            return false;

        ResourceSO _casted = other as ResourceSO;
        return ResourceName == _casted.ResourceName;
    }

    public override string ToString() => ResourceName;
}