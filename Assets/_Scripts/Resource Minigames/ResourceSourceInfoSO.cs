using UnityEngine;

[CreateAssetMenu(fileName = "Resource Source Info", menuName = "Scriptable Objects/Resources/New Resource Source Info")]
public class ResourceSourceInfoSO : ScriptableObject
{
    [field: SerializeField] public GameObject ResourceObject { get; private set; }
    [field: SerializeField] public Resource Resource { get; private set; }
    [field: SerializeField] public int MinAmount { get; private set; }
    [field: SerializeField] public int MaxAmount { get; private set; }
    [HideInInspector] public int SpawnChance => spawnChance;

    [Range(1, 100)]
    [SerializeField] private int spawnChance;
}