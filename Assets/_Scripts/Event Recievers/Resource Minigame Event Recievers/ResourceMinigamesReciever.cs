using UnityEngine;
using System.Collections.Generic;
using System;

public class ResourceMinigamesReciever : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<ResourceGame> games;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private DayNightManager dayNightManager;
    [Space(15)]

    [Header("Day Time Values")]
    [SerializeField] private float timeSlowMultiplier = 0.25f;
    [Range(0f, 1f)]
    [SerializeField] private float percentOfDayLeft = 0.15f;

    private void Start()
    {
        foreach (ResourceGame _game in games)
        {
            _game.OnSuccessfulStart += () => dayNightManager.ActivateMinigameMode
            (
                timeSlowMultiplier, dayNightManager.DayTimeSecondsLength * percentOfDayLeft
            );
            _game.OnSuccessfulStart += () => PlayerInteractionManager.EnableUiMode();

            _game.OnGameSuccessfullyComplete += AddResourcesToPlayerInventory;
            _game.OnGameSuccessfullyComplete += _ => dayNightManager.DeactivateMinigameMode();
            _game.OnGameSuccessfullyComplete += _ => PlayerInteractionManager.DisableUiMode();
            _game.OnGameSuccessfullyComplete += _ => _game.GameUI.enabled = false;

            _game.OnGameUnsuccessfullyComplete += (_inventory, _) => AddResourcesToPlayerInventory(_inventory);
            _game.OnGameUnsuccessfullyComplete += (_, __) => dayNightManager.DeactivateMinigameMode();
            _game.OnGameUnsuccessfullyComplete += (_, __) => PlayerInteractionManager.DisableUiMode();
            _game.OnGameUnsuccessfullyComplete += (_, __) => _game.GameUI.enabled = false;
        }
    }

    private void AddResourcesToPlayerInventory(Inventory _inventory)
    {
        foreach (ResourceSO _resource in _inventory.GetInventory().Keys)
            playerInventory.Inventory.Add(_resource, _inventory.Get(_resource));
    }
}