using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FishItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Image image;

    FishInstance fish;
    InventoryGridView gridView;

    RectTransform rectTransform;
    Canvas rootCanvas;
    Vector2 originalAnchoredPos;

    CanvasGroup canvasGroup;

    bool isFromInventory;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rootCanvas = GetComponentInParent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void BindFish(FishInstance bind, InventoryGridView view)
    {
        fish = bind;
        gridView = view;

        if (image != null)
        {
            image.sprite = fish != null ? fish.Sprite : null;
            image.preserveAspect = true;
        }

        isFromInventory = fish != null && fish.GridPosition.x >= 0 && fish.GridPosition.y >= 0;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalAnchoredPos = rectTransform.anchoredPosition;

        // Bring to front so it draws over grid
        transform.SetAsLastSibling();

        // Let raycasts pass through while dragging so slots can be detected.
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rootCanvas == null) return;

        // Move in UI space
        rectTransform.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore raycast blocking after drag.
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        // Did we drop over a slot?
        InventorySlotUI slot = null;

        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            slot = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<InventorySlotUI>();
        }

        if (slot == null || InventorySystem.instance == null || fish == null)
        {
            rectTransform.anchoredPosition = originalAnchoredPos;
            return;
        }

        Vector2Int targetCell = slot.GridPosition;

        bool success;

        // If fish was already in inventory, try moving.
        // If it wasn't placed yet (caught fish), try placing.
        if (fish.GridPosition.x >= 0 && fish.GridPosition.y >= 0)
        {
            success = InventorySystem.instance.TryToMove(fish, targetCell);
        }
        else
        {
            success = InventorySystem.instance.TryToPlace(fish, targetCell);
        }

        if (!success)
        {
            rectTransform.anchoredPosition = originalAnchoredPos;
            return;
        }

        // SUCCESS:
        // InventoryGridView will rebuild and show the fish in the correct snapped position.
        // This dragged visual (often from FishCaughtSlot) must be removed to prevent duplicates.
        if (gridView != null)
        {
            gridView.Refresh();
        }

        Destroy(gameObject);
    }
}