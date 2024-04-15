using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Collider2D))]
public class MenuManager : MonoBehaviour
{
    public event Action WhileMenuOpen;

    public event Action OnMenuOpen;
    public event Action OnMenuClose;

    [Header("References")]
    [SerializeField] private GameObject menu;
    [SerializeField] private Button menuCloseButton;
    [Space(15)]

    [Header("Settings")]
    [SerializeField] private bool shouldOpenDuringNight = false;
    [SerializeField] private DayNightManager dayNightManager;

    private void Awake() => menuCloseButton.onClick.AddListener(() => OnMenuClose?.Invoke());

    private void Update()
    {
        if (menu.activeInHierarchy)
            WhileMenuOpen?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D _col)
    {
        if (PlayerProvider.TryGetPlayer(out Transform _player))
        {
            if (_col.transform != _player)
                return;

            if (menu.activeInHierarchy)
                return;

            if (!shouldOpenDuringNight && dayNightManager.IsNight())
                return;

            menu.SetActive(true);
            OnMenuOpen?.Invoke();
        }
    }
}