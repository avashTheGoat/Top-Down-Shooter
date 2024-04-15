using UnityEngine;
using UnityEngine.UI;

public class CraftingRequirementsManager : MonoBehaviour
{
    [field: SerializeField] public ItemUI ItemUI { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private CraftingRequirementUI requirementsPrefab;

    private CraftingRecipeSO craftingRecipe;

    public void PopulateWithCraftingRequirements(RectTransform _target)
    {
        foreach (ResourceAmount _resourceAmount in craftingRecipe.CraftingRequirements)
        {
            CraftingRequirementUI _requirement = Instantiate(requirementsPrefab, _target);
            _requirement.RequirementImage.sprite = _resourceAmount.Resource.ResourceImage;
            _requirement.RequirementCount.SetCount(_resourceAmount.Amount);
        }
    }

    public void SetCraftingRecipe(CraftingRecipeSO _recipe) => craftingRecipe = _recipe;

    public bool SetButtonInteractionBasedOnCraftability(Inventory<ResourceSO> _inventory, Button _button)
    {
        _button.interactable = true;
        foreach (ResourceAmount _resourceAmount in craftingRecipe.CraftingRequirements)
        {
            if (!_inventory.Contains(_resourceAmount.Resource))
            {
                _button.interactable = false;
                break;
            }

            if (_inventory.Get(_resourceAmount.Resource) < _resourceAmount.Amount)
            {
                _button.interactable = false;
                break;
            }
        }

        return _button.interactable;
    }
}