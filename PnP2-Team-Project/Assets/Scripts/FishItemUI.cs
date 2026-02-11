using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// This script is for dragging and dropping logic

// IMPORTANT::
// I had errors about the namespaces, but I've found this to be a Visual Studio Error,
// but it compiles in Unity

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
        // gets all the necessary components
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
        // the fish to bind
        fish = bind;
        // the inventory to bind to
        gridView = view;

        // makes sure there's a sprite to display, this should come from the fishInstance
        if (image != null)
        {
            image.sprite = fish != null ? fish.Sprite : null;
            // if we want to stretch the image, set this to false
            image.preserveAspect = true;
        }

        // if it has a position, it's a stored fish
        // if not it's a new fish, so use fishCaught Presenter
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

        // if we're at a slot, set it
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            slot = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<InventorySlotUI>();
        }

        // if we're not at a slot, put the fish back at the starting position
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

        // if we couldn't place or move, set the fish back to where it was
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