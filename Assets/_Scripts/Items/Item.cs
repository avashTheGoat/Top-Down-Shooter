using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Info")]
    [field: SerializeField] public string Name;
    [field: SerializeField] public string Description;
    [field: SerializeField] public Sprite Image;
}