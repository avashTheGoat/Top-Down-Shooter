using UnityEngine;
using UnityEngine.UI;
    
public class ModMenuInventoryReceiver : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform draggingItemsParent;
    [SerializeField] private Button openButton;
    [SerializeField] private ModMenuUIManager modMenuManager;
    [SerializeField] private RectTransform modInventoryUI;
    [SerializeField] private PlayerInventory playerInventory;
    [Space(15)]

    [Header("Wrong Weapon UI")]
    [SerializeField] private MessageUI messagePrefab;
    [Min(0f)]
    [SerializeField] private float fadeInDuration;
    [Min(0f)]
    [SerializeField] private float fadeOutDuration;
    [Space(15)]

    [Header("Prefabs")]
    [SerializeField] private ItemInventoryUI itemSlotPrefab;

    private PlayerInventoryUI<WeaponMod> inventoryUIManager;

    private UIEffects uiEffects;

    private void Awake()
    {
        if (!itemSlotPrefab.TryGetComponent(out DragDropUI _dragDrop))
            Debug.LogError("itemSlotPrefab should have a DragDropUI component on it.");

        inventoryUIManager = new(itemSlotPrefab, playerInventory, true);

        uiEffects = new(this);
        uiEffects.OnEffectComplete += _messageInstance =>
        {
            if (_messageInstance.GetComponent<CanvasGroup>().alpha != 0f)
                return;

            Destroy(_messageInstance.gameObject);
        };
    }

    private void Start()
    {
        openButton.onClick.AddListener(() =>
        {
            inventoryUIManager.UpdateInventory(modInventoryUI, UpdateNewUI);

            int _childCount = modInventoryUI.childCount;
            for (int i = 0; i < _childCount; i++)
            {
                DragDropUI _dragDrop = modInventoryUI.GetChild(i).GetComponent<DragDropUI>();
                SubscribeToDragDrop(_dragDrop);
            }
        });
    }

    private void SubscribeToDragDrop(DragDropUI _dragDrop, bool _isNew = false)
    {
        _dragDrop.OnDragStart += _ =>
        {
            playerInventory.WeaponModInventory.Remove((WeaponMod)_dragDrop.GetComponent<ItemInventoryUI>().ItemUI.Item);

            _dragDrop.Trans.SetParent(draggingItemsParent);
            inventoryUIManager.UpdateInventory(modInventoryUI, _newItemAction: UpdateNewUI);
        };

        _dragDrop.OnDrop += _ =>
        {
            WeaponModsInfoUI _modsUI = modMenuManager.GetActiveWeaponModUI();
            if (_modsUI == null)
                return;

            foreach (ItemSlotUI _itemSlot in _modsUI.AttachedModUIs)
            {
                if (_itemSlot.SlotUI.CurrentItem != null && _dragDrop != null && ReferenceEquals(_dragDrop, _itemSlot.SlotUI.CurrentItem))
                {
                    WeaponMod _mod = (WeaponMod)_itemSlot.Item;
                    if (_mod.IsWeaponCorrectType(_modsUI.Weapon))
                        return;

                    else
                    {
                        print("Slot " + _itemSlot.name + " has wrong type weapon mod.");

                        float _initialAlpha = messagePrefab.CanvasGroup.alpha;
                        messagePrefab.CanvasGroup.alpha = 0f;

                        MessageUI _messageUI = Instantiate(messagePrefab, canvas.transform);
                        _messageUI.transform.SetAsLastSibling();

                        _messageUI.MessageText.text = $"This weapon mod cannot be attached to a {_modsUI.Weapon.Name.ToLower()}.";
                        _messageUI.CloseButton.onClick.AddListener(() => uiEffects.FadeOut(_messageUI.CanvasGroup, fadeOutDuration));

                        uiEffects.FadeIn(_messageUI.CanvasGroup, fadeInDuration);

                        _itemSlot.SlotUI.RemoveItem();

                        messagePrefab.CanvasGroup.alpha = _initialAlpha;

                        break;
                    }
                }
            }

            playerInventory.WeaponModInventory.Add((WeaponMod)_dragDrop.GetComponent<ItemInventoryUI>().ItemUI.Item);
            inventoryUIManager.UpdateInventory(modInventoryUI, _newItemAction: UpdateNewUI);

            Destroy(_dragDrop.gameObject);
        };
    }

    private void UpdateNewUI(ItemInventoryUI _ui, Item _item, int _)
    {
        _ui.GetComponent<DragDropUI>().SetCanvas(canvas);
        _ui.GetComponent<ItemInventoryUI>().ItemUI.SetItem(_item);

        SubscribeToDragDrop(_ui.GetComponent<DragDropUI>(), true);
    }
}