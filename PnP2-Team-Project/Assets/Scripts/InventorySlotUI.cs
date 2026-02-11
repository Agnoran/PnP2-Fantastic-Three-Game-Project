using UnityEngine;

// simple little slot. this just a place to seperate a slot from a fishItem
public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] Vector2Int gridPosition;
    public Vector2Int GridPosition => gridPosition;

    public void SetGridPosition(Vector2Int set)
    {
        gridPosition = set;
    }
}