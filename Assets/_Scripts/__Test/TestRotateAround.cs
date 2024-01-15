using UnityEngine;

public class TestRotateAround : MonoBehaviour
{
    [SerializeField] private float rotateAngle;
    [SerializeField] private Transform objectToRotate;
    [SerializeField] private Vector2 rotateAroundPoint;

    private void Update() => objectToRotate.RotateAround(rotateAroundPoint, Vector3.forward, rotateAngle);
}