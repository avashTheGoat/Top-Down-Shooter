using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileInfo : MonoBehaviour
{
    public float Damage { get; private set; }
    public float Range { get; private set; }
    public List<string> TagsToIgnore { get; private set; }

    private bool hasBeenInitialized = false;

    public void Init(float _damage, float _projectileSpeed, Vector2 _startPosition, float _zRotation, float _range, List<string> _tagsToIgnore)
    {
        if (hasBeenInitialized)
        {
            Debug.LogError("Bullet cannot be initalized twice.");
            return;
        }

        Transform _transform = GetComponent<Transform>();

        _transform.position = _startPosition;
        _transform.Rotate(new Vector3(0, 0, _zRotation));

        GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0f, 0f, _zRotation) * new Vector2(1, 0) * _projectileSpeed;
        
        Damage = _damage;
        Range = _range;
        TagsToIgnore = _tagsToIgnore;

        hasBeenInitialized = true;
    }
}