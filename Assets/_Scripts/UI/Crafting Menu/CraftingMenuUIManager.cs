using UnityEngine;
using System;
using System.Collections.Generic;

public class CraftingMenuUIManager : MonoBehaviour
{
    public event Action<Item> OnCraft;

    [SerializeField] private PlayerInventory playerInventory;

    [Header("Prefabs")]
    [SerializeField] private CraftingRecipeUIManager craftingRecipePrefab;
    [Space(15)]

    [Header("Content Info")]
    [SerializeField] private List<CraftingRecipeSO> craftingRecipes;
    [SerializeField] private RectTransform craftingMenuItems;

    private List<CraftingRecipeUIManager> craftingRecipeUIs = new();

    private void Start()
    {
        foreach (CraftingRecipeSO _craftingRecipe in craftingRecipes)
        {
            CraftingRecipeUIManager _craftingUI = Instantiate(craftingRecipePrefab, craftingMenuItems);
            _craftingUI.ResultImage.sprite = _craftingRecipe.Result.Image;
            _craftingUI.ResultName.text = _craftingRecipe.Result.Name;
            _craftingUI.ResultDescription.text = _craftingRecipe.Result.Description;

            _craftingUI.SetCraftingRecipe(_craftingRecipe);
            _craftingUI.SetButtonInteractionBasedOnCraftability(playerInventory.ResourceInventory);

            // removes resources from inventory & updates all buttons if an item is crafted
            _craftingUI.OnRecipeCraft += _recipe =>
            {
                craftingRecipeUIs.RemoveAll(_craftingRecipe => _craftingRecipe == null);

                foreach (ResourceAmount _resourceAmount in _recipe.CraftingRequirements)
                    playerInventory.ResourceInventory.Remove(_resourceAmount.Resource, _resourceAmount.Amount);

                foreach (CraftingRecipeUIManager _craftingRecipeUI in craftingRecipeUIs)
                    _craftingRecipeUI.SetButtonInteractionBasedOnCraftability(playerInventory.ResourceInventory);
            };

            _craftingUI.OnRecipeCraft += _ => OnCraft?.Invoke(_craftingUI.CraftingRecipe.Result);

            craftingRecipeUIs.Add(_craftingUI);
        }
    }

    private void OnEnable()
    {
        craftingRecipeUIs.RemoveAll(_craftingRecipe => _craftingRecipe == null);

        foreach (CraftingRecipeUIManager _craftingRecipeUI in craftingRecipeUIs)
            _craftingRecipeUI.SetButtonInteractionBasedOnCraftability(playerInventory.ResourceInventory);
    }
}