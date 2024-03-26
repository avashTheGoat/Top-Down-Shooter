using UnityEngine;
using UnityEngine.UI;

public class CraftingRequirementUI : MonoBehaviour
{
    [field: SerializeField] public Image RequirementImage { get; private set; }
    [field: SerializeField] public CounterUI RequirementCount { get; private set; }
}