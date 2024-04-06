using UnityEngine;

public class PlayerCrosshairUI : MonoBehaviour
{
    [SerializeField] private RectTransform crosshairUI;
    [SerializeField] private bool useParentDisplacement;

    // determined by crosshair's parent
    private Vector3 displacement;

    private void Awake() => Cursor.visible = false;

    private void Update()
    {
        crosshairUI.position = Input.mousePosition;

        if (useParentDisplacement)
        {
            displacement = crosshairUI.parent.localPosition;
            crosshairUI.position = new Vector2(crosshairUI.position.x + displacement.x, crosshairUI.position.y + displacement.y);
        }
    }
}