using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class PlayerHealthPickup : MonoBehaviour
{
    public event Action<float> OnHealthPickup;

    [Range(0f, 1f)]
    [SerializeField] private float percentOfMaxHealthHealed;

    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (PlayerProvider.TryGetPlayer(out Transform _player))
        {
            if (_col.transform != _player)
                return;

            PlayerHealth _health = _player.GetComponent<PlayerHealth>();

            float _healthHealed = percentOfMaxHealthHealed * _health.GetMaxHealth();
            _health.Heal(_healthHealed);

            // remove later and destroy in event reciever instead
            Destroy(gameObject);

            OnHealthPickup?.Invoke(_healthHealed);
        }
    }
}