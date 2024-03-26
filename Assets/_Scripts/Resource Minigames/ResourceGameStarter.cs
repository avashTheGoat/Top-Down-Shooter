using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ResourceGameStarter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResourceGame game;
    [SerializeField] private DayNightManager dayNightManager;
    [Space(15)]

    private bool canPlayGame = false;

    private void Start() => dayNightManager.OnNightEnd += () => canPlayGame = true;

    private void OnCollisionEnter2D(Collision2D _col)
    {
        if (PlayerProvider.TryGetPlayer(out Transform _player))
        {
            if (_col.transform != _player)
                return;

            if (!dayNightManager.IsNight() && canPlayGame)
            {
                canPlayGame = false;
                game.StartGame();
            }
        }
    }
}