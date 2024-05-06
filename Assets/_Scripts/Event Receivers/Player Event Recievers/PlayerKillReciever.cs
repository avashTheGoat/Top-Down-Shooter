using UnityEngine;

public class PlayerKillReciever : MonoBehaviour
{
    [SerializeField] private PlayerKill playerKill;

    [SerializeField] private GameObject deathUI;

    private void Start() => playerKill.OnKill += _ => deathUI.SetActive(true);
}