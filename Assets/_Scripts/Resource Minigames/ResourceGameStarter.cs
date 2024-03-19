using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ResourceGameStarter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResourceGame game;
    [SerializeField] private DayNightManager dayNightManager;
    [Space(15)]

    private bool canStartGame = true;

    private Transform trans;

    private void Awake() => trans = transform;

    private void Start()
    {
        dayNightManager.OnDayEnd += () => canStartGame = false;
        dayNightManager.OnNightEnd += () => canStartGame = true;
    }

    private void OnCollisionEnter2D(Collision2D _col)
    {
        if (PlayerProvider.TryGetPlayer(out Transform _player))
        {
            if (_col.transform != _player)
                return;

            if (canStartGame)
            {
                canStartGame = false;
                game.StartGame();
            }
        }
    }
}