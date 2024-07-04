using UnityEngine;

public class MetalGameReceiver : MonoBehaviour
{
    [SerializeField] private MetalGame metalGame;

    [Header("Enter/Exit Position")]
    [SerializeField] private Vector2 enterPosition;
    [SerializeField] private Vector2 exitPosition;

    private void Start()
    {
        metalGame.OnSuccessfulStart += () =>
        {
            if (PlayerProvider.TryGetPlayer(out Transform _player))
                _player.position = enterPosition;
        };

        metalGame.OnGameSuccessfullyComplete += _ => HandleExit();
        metalGame.OnGameUnsuccessfullyComplete += (_, _) => HandleExit();
    }

    private void HandleExit()
    {
        metalGame.GameUI.gameObject.SetActive(false);
        if (PlayerProvider.TryGetPlayer(out Transform _player))
            _player.position = exitPosition;
    }
}