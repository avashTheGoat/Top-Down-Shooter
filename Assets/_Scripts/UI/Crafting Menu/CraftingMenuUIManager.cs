using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class CraftingMenuUIManager : MonoBehaviour
{
    public event Action<CraftingRecipeSO> OnCraft;

    [Header("Dependencies")]
    [SerializeField] private DayNightManager dayNightManager;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TabsUI tabsManager;
    [SerializeField] private Component statsProviderComponent;
    [Space(15)]

    [Header("Prefabs")]
    [SerializeField] private ItemUI craftingThumbnailPrefab;
    [SerializeField] private ItemCraftingUI itemInfoUiPrefab;
    [SerializeField] private TextMeshProUGUI statTextPrefab;
    [Space(15)]

    [Header("Instantiated Prefab Locations")]
    [SerializeField] private RectTransform itemThumbnailsParent;
    [SerializeField] private RectTransform itemInfosParent;
    [Space(15)]
    
    [Header("Settings")]
    [SerializeField] private List<CraftingRecipeSO> craftingRecipes;
    [SerializeField] private bool doCraftsRefreshDaily = false;
    [SerializeField] private int numRecipesDaily;
    [SerializeField] private bool areSameRecipesAllowed = true;

    private List<ItemCraftingUI> spawnedItemInfos = new();

    private IItemStatsProvider statsProvider;

    private void Awake()
    {
        if (statsProviderComponent is not IItemStatsProvider)
            throw new ArgumentException("Must provide a component that is an IProvider<string>.");

        statsProvider = (IItemStatsProvider)statsProviderComponent;
    }

    private void Start()
    {
        SetCraftingRecipes();

        if (doCraftsRefreshDaily)
            dayNightManager.OnNightEnd += SetCraftingRecipes;
    }

    private void SetCraftingRecipes()
    {
        itemThumbnailsParent.DestroyChildren();
        itemInfosParent.DestroyChildren();

        tabsManager.Clear();
        spawnedItemInfos = new();

        List<CraftingRecipeSO> _chosenRecipes = craftingRecipes;

        if (doCraftsRefreshDaily)
            _chosenRecipes = RandomHelper.Choices(craftingRecipes, numRecipesDaily, areSameRecipesAllowed);

        foreach (CraftingRecipeSO _craftingRecipe in _chosenRecipes)
        {
            ItemUI _itemThumbnail = Instantiate(craftingThumbnailPrefab, itemThumbnailsParent);
            _itemThumbnail.SetItem(_craftingRecipe.Result);

            ItemCraftingUI _itemInfo = Instantiate(itemInfoUiPrefab, itemInfosParent);
            _itemInfo.gameObject.SetActive(false);
            _itemInfo.ItemUI.SetItem(_craftingRecipe.Result);

            _itemInfo.RequirementsManager.SetCraftingRecipe(_craftingRecipe);
            _itemInfo.RequirementsManager.PopulateWithCraftingRequirements(_itemInfo.ItemRequirementsUI);
            _itemInfo.RequirementsManager.SetButtonInteractionBasedOnCraftability
            (
                playerInventory.ResourceInventory, _itemInfo.CraftButton
            );

            List<string> _stats = statsProvider.ProvideStats(_craftingRecipe.Result);
            foreach (string _stat in _stats)
            {
                TextMeshProUGUI _text = Instantiate(statTextPrefab, _itemInfo.ItemStatsUI);
                _text.text = _stat;
            }

            tabsManager.AddTab(_itemInfo.gameObject, _itemThumbnail.GetComponent<Button>());

            _itemInfo.CraftButton.onClick.AddListener(() =>
            {
                tabsManager.RemoveTab(_itemThumbnail.GetComponent<Button>());

                spawnedItemInfos.Remove(_itemInfo);
                Destroy(_itemThumbnail.gameObject);
                Destroy(_itemInfo.gameObject);

                OnCraft?.Invoke(_craftingRecipe);

                UpdateAllCraftingButtons();
            });

            spawnedItemInfos.Add(_itemInfo);
        }
    }

    private void OnEnable() => UpdateAllCraftingButtons();

    private void UpdateAllCraftingButtons()
    {
        foreach (ItemCraftingUI _spawnedItemInfo in spawnedItemInfos)
            _spawnedItemInfo.RequirementsManager.SetButtonInteractionBasedOnCraftability(playerInventory.ResourceInventory, _spawnedItemInfo.CraftButton);
    }
}