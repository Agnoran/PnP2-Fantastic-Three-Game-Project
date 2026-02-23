using UnityEngine;

public enum ShopCategory
{
    FishAndTackle,
    BoatUpgrades
}
public enum ShopItemType
{
    Consumable,
    Gear,
    Upgrade
}
[CreateAssetMenu(menuName = "Scriptable Objects/Shop Item", fileName = "NewShopItem")]

public class ShopItem : ScriptableObject
{
    [SerializeField] string displayName;
    [TextArea(2,6)] [SerializeField] string description;
    [SerializeField] Sprite icon;

    [SerializeField] int price;
    [SerializeField] int quantity;

    [SerializeField] ShopCategory[] allowedCategories;
    [SerializeField] ShopItemType itemType;

    [SerializeField] GameObject itemPrefab;

    public string DisplayName => displayName;
    public string Description => description;
    public Sprite Icon => icon;
    public int Price => price;
    public int Quantity => quantity;
    public ShopCategory[] AllowedCategories => allowedCategories;
    public ShopItemType ItemType => itemType;
    public GameObject ItemPrefab => itemPrefab;

    [SerializeField] UpgradeDefinitions upgradeDefinition;
    public UpgradeDefinitions UpgradeDefinition => upgradeDefinition;
}
