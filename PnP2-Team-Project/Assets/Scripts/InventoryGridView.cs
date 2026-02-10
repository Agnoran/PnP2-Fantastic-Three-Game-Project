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
    [SerializeField] float cellSize = 100f;
    [SerializeField] float cellSpacing = 2f;

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
        // make a fresh grid so nothing is displayed twice
        ClearGrid();

        if (InventorySystem.instance == null) return;

        // grab the height and width of the grid
        int w = InventorySystem.instance.GridWidth;
        int h = InventorySystem.instance.GridHeight;

        // for every cell of the grid
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                // create a slot
                InventorySlotUI slot = Instantiate(slotPrefab, slotParent);

                // set the information of the new slot
                slot.SetGridPosition(new Vector2Int(x, y));
                RectTransform rt = slot.GetComponent<RectTransform>();
                rt.anchoredPosition = GridToLocal(new Vector2Int(x, y));

                // add the new slot to the list of slots
                slots.Add(slot);
            }
        }
    }

    public void Refresh()
    {
        // Clear existing items so items are not displayed twice
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (spawnedItems[i] != null)
            {
                Destroy(spawnedItems[i].gameObject);
            }
        }
        // since nothing is spawned we want an empty list
        spawnedItems.Clear();

        if (InventorySystem.instance == null) return;

        // get all the fish that are in the inventory
        // this is set to readonly because we don't want the possibility of editing by accident
        IReadOnlyList<FishInstance> allFish = InventorySystem.instance.GetAllFish();

        // go through the list and look at all the fish
        for (int i = 0; i < allFish.Count; i++)
        {
            // make an instance of the fish
            FishInstance fish = allFish[i];
            // if there isn't a fish, we skip
            if (fish == null) continue;

            // Only render if placed
            if (fish.GridPosition.x < 0 || fish.GridPosition.y < 0) continue;

            // create a new item
            FishItemUI ui = Instantiate(itemPrefab, slotParent);
            // bind that item to the fish we're working with
            ui.BindFish(fish, this);

            // set information for the new item
            RectTransform rt = ui.GetComponent<RectTransform>();

            // IMPORTANT: make math stable regardless of prefab settings
            // this overwrites to what is needed, regardless of mistakes while making the prefab
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);

            // Scale item to footprint FIRST (size changes won't shift top-left pivot)
            // this does not change the LOOK of the item,
            // but will make sure other items can't later be placed on top
            Vector2Int footprint = fish.GetFootprint();
            rt.sizeDelta = new Vector2(
                (cellSize * footprint.x) + (cellSpacing * (footprint.x - 1)),
                (cellSize * footprint.y) + (cellSpacing * (footprint.y - 1))
            );

            // Now place it
            rt.anchoredPosition = GridToLocal(fish.GridPosition);

            // add the item to the spawned list
            spawnedItems.Add(ui);
        }
    }
    /// <summary>
    /// this adapts the cell to account for visual spaces in the grid
    /// otherwise things will spawn weird
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public Vector2 GridToLocal(Vector2Int cell)
    {
        float x = (cell.x * (cellSize + cellSpacing));
        float y = -(cell.y * (cellSize + cellSpacing));
        return new Vector2(x, y);
    }

    public bool TryGetSlotUnderPointer(out InventorySlotUI slot)
    {
        slot = null;

        // placeholder for getting a slot by pointing at it
        // maybe a select and point way of placing fish instead of drag?
        // the video I watched suggested to have this just in case
        return false;
    }

    void ClearGrid()
    {
        // Note:
        // this is for when inventories close, we don't want the slots staying
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