using UnityEngine;

public enum FishType
{
    None,
    Trout,
    Salmon,
    Tuna,
    Eel,
    Catfish,
    Bass,
    Shark,
    Boot,
    Treasure
}

public enum BaitType
{
    None,
    Worm,
    Minnow,
    Insect,
    Shrimp,
    Bread,
    ShinyLure
}

[CreateAssetMenu(fileName = "FishDefinition", menuName = "Scriptable Objects/FishDefinition")]
public class FishDefinition : ScriptableObject
{
    [Header("Identity")]
    [Tooltip("Unique string key for this fish (ex: trout_common). If empty, auto-fills from asset name.")]
    [SerializeField] string fishID;

    [Tooltip("Name shown to the player (ex: 'Trout'). If empty, auto-fills from asset name.")]
    [SerializeField] string displayName;

    [Tooltip("Optional grouping. Use None if you rely entirely on fishID.")]
    [SerializeField] FishType type = FishType.None;


    [Header("Visuals")]
    [SerializeField] Sprite revealed;
    [SerializeField] Sprite silhouette;

    [Header("Tuning")]
    [Tooltip("For setting higher chances of appearing in pools")]
    [Min(0f)]
    [SerializeField] float spawnWeight = 1f;

    [Tooltip("size range for FishInstance.Size")]
    [Min(0f)]
    [SerializeField] float sizeMin = 0.5f;

    [Min(0f)]
    [SerializeField] float sizeMax = 2f;

    [Tooltip("For base value (money) before any size/quality/etc modifiers")]
    [Min(0)]
    [SerializeField] int baseValue = 10;

    [Tooltip("Seconds before Spoiling")]
    [Min(0f)]
    [SerializeField] float baseSpoilTime = 180f; // 3 minutes


    [Header("Bait Preferences")] // for if we decide to do bait
    [Tooltip("if empty, fish won't have a preference")]
    [SerializeField] BaitType[] preferredBaits;

    [Tooltip("multiplier for when bait is preferred.")]
    [Range(0f, 5f)]
    [SerializeField] float preferredBaitAttractionMultiplier = 1.5f;

    [Tooltip("multiplier for when bait is NOT preferred.")]
    [Range(0f, 5f)]
    [SerializeField] float nonPreferredBaitAttractionMultiplier = 0.75f;


    [Header("Inventory Footprint")]
    [SerializeField] Vector2Int gridSize;
    public Vector2Int GridSize => gridSize;


    [Header("Optional Info")]
    [TextArea(2, 6)]
    [SerializeField] string description;


    public string FishId => fishID;
    public string DisplayName => displayName;
    public FishType Type => type;

    public Sprite RevealedSprite => revealed;
    public Sprite SilhouetteSprite => silhouette;

    public float SpawnWeight => spawnWeight;

    public float SizeMin => sizeMin;
    public float SizeMax => sizeMax;

    public int BaseValue => baseValue;
    public float BaseSpoilTime => baseSpoilTime;

    public BaitType[] PreferredBaits => preferredBaits;
    public float PreferredBaitAttractionMultiplier => preferredBaitAttractionMultiplier;
    public float NonPreferredBaitAttractionMultiplier => nonPreferredBaitAttractionMultiplier;

    public string Description => description;


    public bool IsBaitPreferred(BaitType bait)
    {
        if (preferredBaits == null || preferredBaits.Length == 0)
        {
            return false;
        }

        for (int i = 0; i < preferredBaits.Length; i++)
        {
            if (preferredBaits[i] == bait)
            {
                return true;
            }
        }

        return false;
    }

    public float GetAttractionMultiplier(BaitType bait)
    {
        if (IsBaitPreferred(bait))
        {
            return preferredBaitAttractionMultiplier;
        }

        return nonPreferredBaitAttractionMultiplier;
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(fishID))
        {
            fishID = name.ToLowerInvariant().Replace(" ", "_");
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            displayName = name;
        }

        if (sizeMax < sizeMin)
        {
            sizeMax = sizeMin;
        }

        // Grid size must always be at least 1x1
        if (gridSize.x < 1) gridSize.x = 1;
        if (gridSize.y < 1) gridSize.y = 1;
    }
#endif
}
