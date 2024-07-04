using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageUI : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI MessageText { get; private set; }
    [field: SerializeField] public Button CloseButton { get; private set; }
    [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
    [field: SerializeField] public Fadeable Fadeable { get; private set; }
}