using UnityEngine;

public class PlayerCrosshairUI : MonoBehaviour
{
    [SerializeField] private RectTransform crosshairUI;

    private void Awake() => Cursor.visible = false;

    private void Update() => crosshairUI.position = Input.mousePosition;
}