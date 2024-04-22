using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class CraftingMenuUIManager : MonoBehaviour
{
    public event Action<CraftingRecipeSO> OnCraft;

    [Header("Dependencies")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TabsUI tabsManager;
    [SerializeField] private Component statsProviderComponent;
    [Space(15)]

    [Header("Prefabs")]
    [SerializeField] private ItemUI craftingThumbnailPrefab;
    [SerializeField] private ItemCraftingUI itemInfoUiPrefab;
    [SerializeField] private TextMeshProUGUI statTextPrefab;
    [Space(15)]

    [Header("Content Info")]
    [SerializeField] private List<CraftingRecipeSO> craftingRecipes;
    [SerializeField] private RectTransform itemThumbnails;
    [SerializeField] private RectTransform itemInfos;

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
        foreach (CraftingRecipeSO _craftingRecipe in craftingRecipes)
        {
            ItemUI _itemThumbnail = Instantiate(craftingThumbnailPrefab, itemThumbnails);
            _itemThumbnail.ItemImage.sprite = _craftingRecipe.Result.Image;
            _itemThumbnail.ItemName.text = _craftingRecipe.Result.Name;

            ItemCraftingUI _itemInfo = Instantiate(itemInfoUiPrefab, itemInfos);
            _itemInfo.ItemUI.ItemImage.sprite = _craftingRecipe.Result.Image;
            _itemInfo.ItemUI.ItemDescription.text = _craftingRecipe.Result.Description;
            _itemInfo.ItemUI.ItemName.text = _craftingRecipe.Result.Name;

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
                _itemThumbnail.gameObject.SetActive(false);
                _itemInfo.gameObject.SetActive(false);

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
        {
            _spawnedItemInfo.RequirementsManager.SetButtonInteractionBasedOnCraftability
            (
                playerInventory.ResourceInventory, _spawnedItemInfo.CraftButton
            );
        }
    }
}