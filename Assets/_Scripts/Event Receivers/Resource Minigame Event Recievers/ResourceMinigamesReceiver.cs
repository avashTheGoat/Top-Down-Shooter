using UnityEngine;
using System.Collections.Generic;
using System;

public class ResourceMinigamesReceiver : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private DayNightManager dayNightManager;
    [Space(15)]

    [Header("Day Time Values")]
    [SerializeField] private float timeSlowMultiplier = 0.25f;
    [Range(0f, 1f)]
    [SerializeField] private float percentOfDayLeft = 0.15f;
    
    [SerializeField] private List<ResourceGame> games;
    [SerializeField] private List<GameObject> mainUI;

    private void Start()
    {
        foreach (ResourceGame _game in games)
        {
            _game.OnSuccessfulStart += () => dayNightManager.ActivateMinigameMode
            (
                timeSlowMultiplier, dayNightManager.DayTimeSecondsLength * percentOfDayLeft
            );
            _game.OnSuccessfulStart += () =>
            {
                foreach (GameObject _ui in mainUI)
                    _ui.SetActive(false);
            };

            _game.OnGameSuccessfullyComplete += AddResourcesToPlayerInventory;
            _game.OnGameSuccessfullyComplete += _ => dayNightManager.DeactivateMinigameMode();
            _game.OnGameSuccessfullyComplete += _ => _game.Game.SetActive(false);
            _game.OnGameSuccessfullyComplete += _ =>
            {
                foreach (GameObject _ui in mainUI)
                    _ui.SetActive(true);
            };

            _game.OnGameUnsuccessfullyComplete += (_inventory, _) => AddResourcesToPlayerInventory(_inventory);
            _game.OnGameUnsuccessfullyComplete += (_, _) => dayNightManager.DeactivateMinigameMode();
            _game.OnGameUnsuccessfullyComplete += (_, _) => _game.Game.SetActive(false);
            _game.OnGameUnsuccessfullyComplete += (_, _) =>
            {
                foreach (GameObject _ui in mainUI)
                    _ui.SetActive(true);
            };
        }
    }

    private void AddResourcesToPlayerInventory(Inventory<Resource> _inventory)
    {
        foreach (Resource _resource in _inventory.GetDictionary().Keys)
            playerInventory.ResourceInventory.Add(_resource, _inventory.Get(_resource));
    }
}