using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;
    public event Action OnInventoryChanged;

    [SerializeField] int gridWidth = 4;
    [SerializeField] int gridHeight = 5;

    FishInstance[,] occupied;

    readonly List<FishInstance> storedFish = new List<FishInstance>();

    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);

        RebuildGrid();
    }

    public void SetGridSize(int newWidth, int newHeight)
    {
        gridWidth = Mathf.Max(1, newWidth);
        gridHeight = Mathf.Max(1, newHeight);

        ClearAll();
        RebuildGrid();
        RaiseChanged();
    }

    void RebuildGrid()
    {
        occupied = new FishInstance[gridWidth, gridHeight];
    }
    public void ClearAll()
    {
        storedFish.Clear();

        if (occupied != null)
        {
            Array.Clear(occupied, 0, occupied.Length);
        }
        RaiseChanged();
    }

    public bool TryToPlace(FishInstance fish, Vector2Int gridPosition)
    {
        // check for a fish
        if(fish == null) return false;
        // check if gridPosition is in the inventory area
        if(!IsInGrid(gridPosition)) return false;

        // check if the fish can fit at that spot
        Vector2Int size = fish.GetFootprint();
        if(!CanFitAt(fish, gridPosition, size)) return false;

        // place the fish
        WriteOccupancy(fish, gridPosition, size, true);

        // set the fish's gridPosition
        fish.SetGridPosition(gridPosition);

        // if we're just moving the fish, don't re-add it to the stored fish list
        if (!storedFish.Contains(fish))
        {
            storedFish.Add(fish);
        }

        RaiseChanged();
        return true;
    }

    public bool TryToMove(FishInstance fish, Vector2Int newPosition)
    {
        // check for a fish
        if(fish == null) return false;
        // make sure we are moving a stored fish (and not a newly caught fish)
        if(!storedFish.Contains(fish)) return false;

        // store our fishes old info in case something goes wrong
        Vector2Int oldPosition = fish.GridPosition;
        Vector2Int size = fish.GetFootprint();

        // clear the fish's old spot
        WriteOccupancy(fish, oldPosition, size, false);

        // check that where we want to move is in the inventory area and it can fit there
        bool canFitAtNew = IsInGrid(newPosition) && CanFitAt(fish, newPosition, size);

        // if it can't, then cancel the move and re-occupy the old positions
        if (!canFitAtNew)
        {
            WriteOccupancy(fish, oldPosition, size, true);
            return false;
        }

        // if it can fit, set the new positons
        WriteOccupancy(fish, newPosition, size, true);
        fish.SetGridPosition(newPosition);

        RaiseChanged();
        return true;
    }

    public void RemoveFish(FishInstance fish)
    {
        // check for a fish
        if (fish == null) return;
        // check if the fish to remove is even a stored fish
        if (!storedFish.Contains(fish)) return;
        
        // get the info of the fish
        Vector2Int position = fish.GridPosition;
        Vector2Int size = fish.GetFootprint();

        // clear the fish's spot
        WriteOccupancy(fish, position, size, false);

        // remove the fish from the stored fish list
        storedFish.Remove(fish);

        // unset the fish position
        fish.SetGridPosition(new Vector2Int(-1, -1));

        RaiseChanged();
    }

    public bool TryAutoPlaceFish(FishInstance fish)
    {
        // check for a fish
        if(fish == null) return false;

        // for loop going through the whole inventory
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                // check each position if the fish can fit there
                // and place the fish if it can
                if (TryToPlace(fish, new Vector2Int(i, j)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public IReadOnlyList<FishInstance> GetAllFish()
    {
        return storedFish;
    }

    public FishInstance GetFishAtCell(Vector2Int cell)
    {
        // returns what fish, if any, is at that position
        if(!IsInGrid(cell)) return null;
        return occupied[cell.x, cell.y];
    }

    bool CanFitAt(FishInstance fish, Vector2Int position, Vector2Int size)
    {
        // for loop going through the every idex of the fish index
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                // temp spot for the part of the fish we're checking
                Vector2Int cell = new Vector2Int(position.x + x, position.y + y);
                // if that part of the fish is outside of the inventory, stop checking
                if (!IsInGrid(cell)) return false;

                // returns if that cell is occupied
                FishInstance existing = occupied[cell.x, cell.y];
                // if it is occupied and that occupied is not the fish we're checking
                if (existing != null && existing != fish)
                {
                    // stop the check and report "can't fit"
                    return false;
                }
            }
        }
        // after all parts of the fish are checked and everything can fit, return that fact
        return true;
    }

    void WriteOccupancy(FishInstance fish, Vector2Int position, Vector2Int size, bool occupy)
    {
        // if position isn't valid, return
        if (position.x < 0 || position.y < 0) return;

        // for loop for the fish footprint
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                // get the cell that we are writing at
                Vector2Int cell = new Vector2Int(position.x + x, position.y + y);
                // safety check that we're not writing outside the inventory
                // other functions should be checking this anyways
                if (!IsInGrid(cell)) continue;

                // at that cell in occupied array, if occupy is true, provide it the fish information, if not, set it to null
                occupied[cell.x, cell.y] = occupy ? fish : null;
            }
        }
    }


    bool IsInGrid(Vector2Int cell)
    {
        // in grid? return true. not in grid? return false
        return cell.x >= 0 && cell.y >= 0 && cell.x < gridWidth && cell.y < gridHeight;
    }
    private void RaiseChanged()
    {
        OnInventoryChanged?.Invoke();
    }

    internal int GetTotalFishValue()
    {
        int total = 0;
        foreach (FishInstance fish in storedFish)
        {
            total += fish.Value;
        }
        return total;
    }
}
