using UnityEngine;

public class RangedWeaponReloadSlow : MonoBehaviour
{
    [SerializeField] private Component iMoveable;

    private IMoveable moveable;

    private void Awake() => moveable = (IMoveable)iMoveable;

    private void Start()
    {
        
    }
}