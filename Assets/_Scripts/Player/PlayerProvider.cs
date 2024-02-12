using UnityEngine;

public class PlayerProvider : MonoBehaviour
{
    [SerializeField] private string playerTag;

    private static Transform playerTrans;

    private void Start() => playerTrans = GameObject.FindGameObjectWithTag(playerTag).transform;

    public static bool TryGetPlayer(out Transform _player)
    {
        _player = null;

        if (playerTrans == null) return false;

        _player = playerTrans;
        return true;
    }

    public static Transform GetPlayer() => playerTrans;
}