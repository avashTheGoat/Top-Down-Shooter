using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DragDropSlotUI : MonoBehaviour, IDropHandler
{
    public event Action<DragDropUI> OnItemDropped;
    public event Action<DragDropUI> OnItemRemoved;

    public DragDropUI CurrentItem { get; private set; } = null;

    private DragDropUI prevCurItem = null;

    private RectTransform trans;

    private void Awake() => trans = GetComponent<RectTransform>();

    public void OnDrop(PointerEventData _eventData)
    {
        if (_eventData.pointerDrag == null)
            return;

        if (CurrentItem != null)
            return;

        if (_eventData.pointerDrag.TryGetComponent(out DragDropUI _dragDropUI))
        {
            _dragDropUI.Trans.SetParent(trans);
            _dragDropUI.Trans.localPosition = Vector3.zero;

            CurrentItem = _dragDropUI;
            prevCurItem = CurrentItem;

            CurrentItem.OnDragStart += _ => CurrentItem = null;

            CurrentItem.OnDrop += HandleItemMoveFromSlot;

            OnItemDropped?.Invoke(CurrentItem);
        }
    }

    public void RemoveItem()
    {
        if (CurrentItem == null)
            return;

        CurrentItem.OnDrop -= HandleItemMoveFromSlot;
        CurrentItem.OnEndDrag(null, () => CurrentItem = null);
    }

    private void HandleItemMoveFromSlot(DragDropUI _dragDropUI)
    {
        // UI gets dragged, so CurrentItem == null
        // If dropped onto slot, OnDrop on slot gets called before OnDrop on DragDropUI, so CurrentItem != null.
        // If not dropped onto slot, CurrentItem == null, so the if statement will succeed
        if (CurrentItem == null)
        {
            prevCurItem.OnDrop -= HandleItemMoveFromSlot;
            OnItemRemoved?.Invoke(prevCurItem);

            prevCurItem = null;
        }
    }
}