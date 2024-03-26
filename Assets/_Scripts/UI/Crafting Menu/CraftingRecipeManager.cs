using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CraftingRecipeManager : MonoBehaviour
{
    public event Action<CraftingRecipeSO> OnRecipeCraft;
    public CraftingRecipeSO CraftingRecipe { get; private set; }
    [field: SerializeField] public Image ResultImage { get; private set; }
    [field: SerializeField] public TextMeshProUGUI ResultName { get; private set; }
    [field: SerializeField] public TextMeshProUGUI ResultDescription { get; private set; }
    [field: SerializeField] public RectTransform CraftingRequirementsArea { get; private set; }
    
    [SerializeField] private Button craftButton;

    [Header("Prefabs")]
    [SerializeField] private CraftingRequirementUI requirementsPrefab;

    private void Start()
    {
        craftButton.onClick.AddListener(() => Destroy(gameObject));
        craftButton.onClick.AddListener(() => OnRecipeCraft?.Invoke(CraftingRecipe));
    }

    public void SetCraftingRecipe(CraftingRecipeSO _recipe)
    {
        CraftingRecipe = _recipe;

        foreach (ResourceAmount _resourceAmount in CraftingRecipe.CraftingRequirements)
        {
            CraftingRequirementUI _requirement = Instantiate(requirementsPrefab, CraftingRequirementsArea);
            _requirement.RequirementImage.sprite = _resourceAmount.Resource.ResourceImage;
            _requirement.RequirementCount.SetCount(_resourceAmount.Amount);
        }
    }

    public bool SetButtonInteractionBasedOnCraftability(Inventory _inventory)
    {
        craftButton.interactable = true;
        foreach (ResourceAmount _resourceAmount in CraftingRecipe.CraftingRequirements)
        {
            if (!_inventory.Contains(_resourceAmount.Resource))
            {
                craftButton.interactable = false;
                break;
            }

            if (_inventory.Get(_resourceAmount.Resource) < _resourceAmount.Amount)
            {
                craftButton.interactable = false;
                break;
            }
        }

        return craftButton.interactable;
    }
}