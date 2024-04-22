using UnityEngine;

public class ForceSetPositionComponent : MonoBehaviour
{
    [Tooltip("Any components that are -301 are ignored.")]
    [SerializeField] private Vector3 positionComponents = new(-301f, -301f, -301f);

    private Transform trans;

    private void Awake() => trans = transform;

    private void Update()
    {
        if (positionComponents.x != -301f)
            trans.position = new(positionComponents.x, trans.position.y, trans.position.z);

        if (positionComponents.y != -301f)
            trans.position = new(trans.position.x, positionComponents.y, trans.position.z);

        if (positionComponents.z != -301f)
            trans.position = new(trans.position.x, trans.position.y, positionComponents.z);
    }
}