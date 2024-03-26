using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Collider2D))]
public class CraftingMenuManager : MonoBehaviour
{
    public event Action WhileCraftingOpen;

    public event Action OnCraftingOpen;
    public event Action OnCraftingClose;

    [Header("References")]
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private DayNightManager dayNightManager;
    [SerializeField] private Button craftingCloseButton;

    private void Awake() => craftingCloseButton.onClick.AddListener(() => OnCraftingClose?.Invoke());

    private void Update()
    {
        if (craftingUI.activeInHierarchy)
            WhileCraftingOpen?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D _col)
    {
        if (PlayerProvider.TryGetPlayer(out Transform _player))
        {
            if (_col.transform != _player)
                return;

            if (dayNightManager.IsNight() || craftingUI.activeInHierarchy)
                return;

            craftingUI.SetActive(true);
            OnCraftingOpen?.Invoke();
        }
    }
}