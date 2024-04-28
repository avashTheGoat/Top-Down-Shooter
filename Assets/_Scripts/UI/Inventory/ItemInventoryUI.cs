using UnityEngine;

public class ItemInventoryUI : MonoBehaviour
{
    [field: SerializeField] public ItemUI ItemUI { get; private set; }
    [field: SerializeField] public CounterUI CounterUI { get; private set; }
}