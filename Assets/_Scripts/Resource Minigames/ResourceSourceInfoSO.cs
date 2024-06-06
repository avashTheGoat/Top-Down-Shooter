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
}