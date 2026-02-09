using System;
using UnityEngine;

[Serializable]
public class FishInstance
{
    [SerializeField] FishDefinition definition;

    // Jose made data (rolled per-attempt)
    [SerializeField] float size;
    [SerializeField] int quality;
    [SerializeField] int value;
    [SerializeField] float timeCaught;
    [SerializeField] float spoilTimeSeconds;

    // Hardcoded per fishDefinition
    [SerializeField] Vector2Int gridSize;

    // Inventory placement state (Ben / Inventory)
    [SerializeField] Vector2Int gridPosition;
    [SerializeField] bool rotated;
    [SerializeField] bool flipped;

    // Read-only accessors (definition-driven identity)
    public FishDefinition Definition => definition;

    public string FishId => definition != null ? definition.FishId : string.Empty;
    public string Name => definition != null ? definition.DisplayName : "Unknown Fish";
    public FishType Type => definition != null ? definition.Type : FishType.None;

    public Sprite Sprite => definition != null ? definition.RevealedSprite : null;
    public Sprite SilhouetteSprite => definition != null ? definition.SilhouetteSprite : null;

    // Instance data accessors
    public float Size => size;
    public int Quality => quality;
    public int Value => value;

    public float TimeCaught => timeCaught;
    public float SpoilTimeSeconds => spoilTimeSeconds;

    // Inventory state accessors
    public Vector2Int GridSize => gridSize;
    public Vector2Int GridPosition => gridPosition;

    public bool Rotated => rotated;
    public bool Flipped => flipped;

    public FishInstance() { }

    // Jose: constructor used when the pool generates a fish attempt
    public FishInstance(
        FishDefinition fishDefinition,
        float rolledSize,
        int rolledQuality,
        int rolledValue,
        float rolledSpoilTimeSeconds
    )
    {
        if (fishDefinition == null)
        {
            throw new ArgumentNullException(nameof(fishDefinition));
        }

        definition = fishDefinition;

        size = Mathf.Max(0f, rolledSize);
        quality = Mathf.Clamp(rolledQuality, 0, 100);
        value = Mathf.Max(0, rolledValue);

        spoilTimeSeconds = Mathf.Max(0f, rolledSpoilTimeSeconds);
        timeCaught = Time.time;

        // Grid size is hardcoded in FishDefinition
        gridSize = fishDefinition.GridSize;
        if (gridSize.x < 1) gridSize.x = 1;
        if (gridSize.y < 1) gridSize.y = 1;

        // Default placement state (not placed)
        gridPosition = new Vector2Int(-1, -1);
        rotated = false;
        flipped = false;
    }

    // Inventory helpers
    public Vector2Int GetFootprint()
    {
        return rotated
            ? new Vector2Int(gridSize.y, gridSize.x)
            : gridSize;
    }

    public void SetGridPosition(Vector2Int newGridPosition)
    {
        gridPosition = newGridPosition;
    }

    public void ToggleRotated()
    {
        rotated = !rotated;
    }

    public void ToggleFlipped()
    {
        flipped = !flipped;
    }
    public void SetRotated(bool set)
    {
        rotated = set;
    }




    // Spoilage helpers (optional feature)
    public float GetAgeSeconds()
    {
        return Mathf.Max(0f, Time.time - timeCaught);
    }

    public float GetFreshness01()
    {
        if (spoilTimeSeconds <= 0f)
        {
            return 0f;
        }

        float t = GetAgeSeconds() / spoilTimeSeconds;
        return Mathf.Clamp01(1f - t);
    }

    public bool IsSpoiled()
    {
        return GetAgeSeconds() >= spoilTimeSeconds;
    }
}