using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

public class InventoryItemTooltip : MonoBehaviour
{
    [SerializeField] private RectTransform mouseFollowUI;

    [Header("Prefabs")]
    [SerializeField] private ItemUI rightLeaningPrefab;
    [SerializeField] private ItemUI leftLeaningPrefab;

    private CanvasRenderer curTooltipRenderer = null;
    private ItemUI curTooltip = null;

    private Direction curDirection = Direction.RIGHT;

    private void Update()
    {
        PointerEventData _pointerData = new(EventSystem.current);
        _pointerData.position = Input.mousePosition;

        List<RaycastResult> _results = new();
        EventSystem.current.RaycastAll(_pointerData, _results);

        if (_results.Count == 0)
            return;

        if (_results[0].gameObject.TryGetComponent(out ItemInventoryUI _itemUI))
        {
            if (curTooltip == null)
            {
                curTooltip = Instantiate(rightLeaningPrefab, mouseFollowUI);
                curTooltipRenderer = curTooltip.GetComponent<CanvasRenderer>();
            }

            curTooltip.SetItem(_itemUI.ItemUI.Item);

            if (curTooltipRenderer.cull)
            {
                ItemUI _newCur = Instantiate(curDirection == Direction.RIGHT ? leftLeaningPrefab : rightLeaningPrefab, mouseFollowUI);
                CanvasRenderer _newRenderer = _newCur.GetComponent<CanvasRenderer>();

                _newCur.SetItem(_itemUI.ItemUI.Item);
                Destroy(curTooltip.gameObject);

                curTooltip = _newCur;
                curTooltipRenderer = _newRenderer;
            }

            curTooltip.gameObject.SetActive(true);
        }

        else
        {
            if (curTooltip != null)
                curTooltip.gameObject.SetActive(false);
        }
    }

    private enum Direction { RIGHT, LEFT }
}