using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGridView : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] InventorySlotUI slotPrefab;
    [SerializeField] FishItemUI itemPrefab;

    [Header("Layout")]
    [SerializeField] RectTransform slotParent;
    [SerializeField] float cellSize = 64f;
    [SerializeField] float cellSpacing = 6f;

    readonly List<InventorySlotUI> slots = new List<InventorySlotUI>();
    readonly List<FishItemUI> spawnedItems = new List<FishItemUI>();

    void OnEnable()
    {
        if (InventorySystem.instance != null)
        {
            InventorySystem.instance.OnInventoryChanged += Refresh;
        }

        BuildGrid();
        Refresh();
    }

    void OnDisable()
    {
        if (InventorySystem.instance != null)
        {
            InventorySystem.instance.OnInventoryChanged -= Refresh;
        }
    }

    public void BuildGrid()
    {
        ClearGrid();

        if (InventorySystem.instance == null) return;

        int w = InventorySystem.instance.GridWidth;
        int h = InventorySystem.instance.GridHeight;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                InventorySlotUI slot = Instantiate(slotPrefab, slotParent);
                slot.SetGridPosition(new Vector2Int(x, y));

                RectTransform rt = slot.GetComponent<RectTransform>();
                rt.anchoredPosition = GridToLocal(new Vector2Int(x, y));

                slots.Add(slot);
            }
        }
    }

    public void Refresh()
    {
        // Clear existing item visuals
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (spawnedItems[i] != null)
            {
                Destroy(spawnedItems[i].gameObject);
            }
        }
        spawnedItems.Clear();

        if (InventorySystem.instance == null) return;

        IReadOnlyList<FishInstance> allFish = InventorySystem.instance.GetAllFish();

        for (int i = 0; i < allFish.Count; i++)
        {
            FishInstance fish = allFish[i];
            if (fish == null) continue;

            // Only render if placed
            if (fish.GridPosition.x < 0 || fish.GridPosition.y < 0) continue;

            FishItemUI ui = Instantiate(itemPrefab, slotParent);
            ui.BindFish(fish, this);

            RectTransform rt = ui.GetComponent<RectTransform>();

            // IMPORTANT: make math stable regardless of prefab settings
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);

            // Scale item to footprint FIRST (size changes won't shift top-left pivot)
            Vector2Int footprint = fish.GetFootprint();
            rt.sizeDelta = new Vector2(
                (cellSize * footprint.x) + (cellSpacing * (footprint.x - 1)),
                (cellSize * footprint.y) + (cellSpacing * (footprint.y - 1))
            );

            // Now place it
            rt.anchoredPosition = GridToLocal(fish.GridPosition);

            spawnedItems.Add(ui);
        }
    }

    public Vector2 GridToLocal(Vector2Int cell)
    {
        float x = (cell.x * (cellSize + cellSpacing));
        float y = -(cell.y * (cellSize + cellSpacing));
        return new Vector2(x, y);
    }

    public bool TryGetSlotUnderPointer(out InventorySlotUI slot)
    {
        slot = null;

        // Cheap approach: rely on Unity’s event system raycast via FishItemUI
        // So this is just a helper placeholder if you want later.
        return false;
    }

    void ClearGrid()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] != null)
            {
                Destroy(slots[i].gameObject);
            }
        }
        slots.Clear();
    }

    public float CellSize => cellSize;
    public float CellSpacing => cellSpacing;
}