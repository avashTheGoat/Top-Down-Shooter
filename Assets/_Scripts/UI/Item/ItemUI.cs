using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    [field: SerializeField] public Image ItemImage { get; private set; }
    [field: SerializeField] public TextMeshProUGUI ItemName { get; private set; }
    [field: SerializeField] public TextMeshProUGUI ItemDescription { get; private set; }
}