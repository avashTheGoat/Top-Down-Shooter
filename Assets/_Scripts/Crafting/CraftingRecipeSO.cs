using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Scriptable Objects/Crafting Recipe")]
public class CraftingRecipeSO : ScriptableObject
{
    [field: SerializeField] public List<ResourceAmount> CraftingRequirements { get; private set; } = new();
    [field: SerializeField] public Item Result { get; private set; }
}