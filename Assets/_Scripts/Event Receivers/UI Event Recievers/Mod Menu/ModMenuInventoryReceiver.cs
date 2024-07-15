using UnityEngine;
using UnityEngine.UI;
using System.Linq;
    
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

    [Header("Prefabs")]
    [SerializeField] private MessageUI messagePrefab;
    [SerializeField] private ItemInventoryUI itemSlotPrefab;

    private PlayerInventoryUI<WeaponMod> inventoryUIManager;

    private void Awake()
    {
        if (!itemSlotPrefab.TryGetComponent(out DragDropUI _))
            Debug.LogError("itemSlotPrefab should have a DragDropUI component on it.");

        inventoryUIManager = new(itemSlotPrefab, playerInventory, true);
    }

    private void Start() => openButton.onClick.AddListener(() => inventoryUIManager.UpdateInventory(modInventoryUI, UpdateNewUI));

    private void SubscribeToDragDrop(DragDropUI _dragDrop)
    {
        _dragDrop.OnDragStart += _ =>
        {
            _dragDrop.Trans.SetParent(draggingItemsParent);
        };
        _dragDrop.OnDragStart += RemoveFromInventory;

        _dragDrop.OnDrop += _ =>
        {
            WeaponModsInfoUI _modsUI = modMenuManager.GetActiveWeaponModUI();
            if (_modsUI == null)
                return;

            foreach (ItemSlotUI _itemSlot in _modsUI.AttachedModUIs.Where(_slot => _slot.SlotUI.CurrentItem != null && ReferenceEquals(_dragDrop, _slot.SlotUI.CurrentItem)))
            {
                if (_dragDrop == null)
                    break;

                WeaponMod _mod = (WeaponMod)_itemSlot.Item;
                if (_mod.IsWeaponCorrectType(_modsUI.Weapon))
                {
                    _dragDrop.OnDragStart -= RemoveFromInventory;
                    Vector2 _slotPos = _itemSlot.SlotUI.Trans.position;
                    _slotPos = new(_slotPos.x + _dragDrop.Trans.rect.width + 5, _slotPos.y);
                    _dragDrop.Trans.position = _slotPos;
                    _dragDrop.Trans.SetParent(_itemSlot.SlotUI.Trans);
                    return;
                }

                else
                {
                    float _initialAlpha = messagePrefab.CanvasGroup.alpha;
                    messagePrefab.CanvasGroup.alpha = 0f;

                    MessageUI _messageUI = Instantiate(messagePrefab, canvas.transform);
                    _messageUI.transform.SetAsLastSibling();

                    _messageUI.MessageText.text = $"This weapon mod cannot be attached to a {_modsUI.Weapon.Name.ToLower()}.";
                    _messageUI.CloseButton.onClick.AddListener(() => _messageUI.Fadeable.FadeOut(0));

                    _messageUI.Fadeable.FadeIn(0);

                    _messageUI.Fadeable.OnFadeComplete += () =>
                    {
                        if (_messageUI.CanvasGroup.alpha != 0f)
                            return;

                        Destroy(_messageUI.gameObject);
                    };

                    _itemSlot.SlotUI.RemoveItem();

                    messagePrefab.CanvasGroup.alpha = _initialAlpha;

                    break;
                }
            }

            playerInventory.WeaponModInventory.Add((WeaponMod)_dragDrop.GetComponent<ItemInventoryUI>().ItemUI.Item);
            inventoryUIManager.UpdateInventory(modInventoryUI, _newItemAction: UpdateNewUI);

            Destroy(_dragDrop.gameObject);
        };
    }

    private void RemoveFromInventory(DragDropUI _dragDrop)
    {
        playerInventory.WeaponModInventory.Remove((WeaponMod)_dragDrop.GetComponent<ItemInventoryUI>().ItemUI.Item);
        inventoryUIManager.UpdateInventory(modInventoryUI, _newItemAction: UpdateNewUI);
    }

    private void UpdateNewUI(ItemInventoryUI _ui, Item _item, int _)
    {
        _ui.GetComponent<DragDropUI>().SetCanvas(canvas);
        _ui.GetComponent<ItemInventoryUI>().ItemUI.SetItem(_item);

        SubscribeToDragDrop(_ui.GetComponent<DragDropUI>());
    }
}