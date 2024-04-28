using UnityEngine;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class DragDropUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public event Action<DragDropUI> OnDragStart;
	public event Action<DragDropUI> OnDrop;
	
	public Canvas Canvas { get; private set; }

	public RectTransform Trans { get; private set; }

	private CanvasGroup canvasGroup;

    public void Awake()
    {
        Trans = GetComponent<RectTransform>();
		canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnDrag(PointerEventData _eventData) => Trans.anchoredPosition += _eventData.delta / Canvas.scaleFactor;

    public void OnBeginDrag(PointerEventData _eventData)
	{
		canvasGroup.blocksRaycasts = false;
		OnDragStart?.Invoke(this);
	}

	public void OnEndDrag(PointerEventData _eventData)
	{
		canvasGroup.blocksRaycasts = true;
		OnDrop?.Invoke(this);
	}

	public void OnEndDrag(PointerEventData _eventData, Action _preOnDrop)
	{
		canvasGroup.blocksRaycasts = true;
		_preOnDrop.Invoke();
		OnDrop?.Invoke(this);
	}

	public void SetCanvas(Canvas _canvas) => Canvas = _canvas;
}