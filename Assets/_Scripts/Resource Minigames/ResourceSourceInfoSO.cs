using UnityEngine;

[CreateAssetMenu(fileName = "Resource Source Info", menuName = "Scriptable Objects/Resources/New Resource Source Info")]
public class ResourceSourceInfoSO : ScriptableObject
{
    [field: SerializeField] public GameObject ResourceObject { get; private set; }
    [field: SerializeField] public Resource ResourceDropped { get; private set; }
    [field: SerializeField] public int MinAmountDropped { get; private set; }
    [field: SerializeField] public int MaxAmountDropped { get; private set; }
    [HideInInspector] public int SpawnChance => spawnChance;

    [Range(1, 100)]
    [SerializeField] private int spawnChance;

    #nullable enable
    public void Init(GameObject? _resourceObject, Resource? _resourceDropped, int? _minAmountDropped, int? _maxAmountDropped)
    #nullable disable
    {
        if (_resourceObject != null)
            ResourceObject = _resourceObject;

        if (_resourceDropped != null)
            ResourceDropped = _resourceDropped;

        if (_minAmountDropped != null)
            MinAmountDropped = (int)_minAmountDropped;

        if (_maxAmountDropped != null)
            MaxAmountDropped = (int)_maxAmountDropped;
    }
}