using UnityEngine;

public class GameObjectMouseFollow : MonoBehaviour
{
    [SerializeField] private Camera mainCam;

    private Transform trans;

    private void Awake() => trans = transform;

    private void Update() => trans.position = mainCam.ScreenToWorldPoint(Input.mousePosition);
}