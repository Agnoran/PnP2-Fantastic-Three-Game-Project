using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;

    [Header("Menu Root")]
    [SerializeField] GameObject shopMenuRoot;

    [Header("Grid Slots (fixed buttons)")]
    [SerializeField] SetShopButton[] itemButtonSlots;

    [Header("Optional UI")]
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text shopTitleText;

    [Header("Temporary Money")]
    // later we'll make player Money equal the money on the Player
    [SerializeField] int playerMoney = 250;

    Shop currentShop;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public bool CanAfford(int cost)
    {
        return playerMoney >= cost;
    }

    public void Open(Shop shop)
    {
        if (shop == null) return;
        if (WorldController.instance == null) return;
        if (WorldController.instance.IsMenuOpen()) return;

        currentShop = shop;

        if (shopMenuRoot != null)
        {
            shopMenuRoot.SetActive(true);
        }

        if (shopTitleText != null)
        {
            shopTitleText.text = shop.ShopCategory.ToString();
        }

        PopulateFromShop(shop);
        RefreshMoneyUI();

        WorldController.instance.StateOpenShop();
    }

    public void Close()
    {
        if (WorldController.instance == null) return;

        currentShop = null;

        if (shopMenuRoot != null)
        {
            shopMenuRoot.SetActive(false);
        }

        ClearAllSlots();
        WorldController.instance.StateCloseShop();
    }

    void PopulateFromShop(Shop shop)
    {
        if (itemButtonSlots == null || itemButtonSlots.Length == 0)
            return;

        ShopItem[] items = shop.ItemsForSale;
        int itemCount = (items != null) ? items.Length : 0;

        for (int i = 0; i < itemButtonSlots.Length; i++)
        {
            SetShopButton slot = itemButtonSlots[i];
            if (slot == null)
                continue;

            if (i < itemCount && items[i] != null)
            {
                slot.Bind(shop, items[i], i);
            }
            else
            {
                slot.Clear();
            }
        }
    }

    void ClearAllSlots()
    {
        if (itemButtonSlots == null)
            return;

        for (int i = 0; i < itemButtonSlots.Length; i++)
        {
            if (itemButtonSlots[i] != null)
            {
                itemButtonSlots[i].Clear();
            }
        }
    }

    void RefreshMoneyUI()
    {
        // we'll add a playerMoney = player.GetMoney() or soemthing like that
        if (moneyText != null)
        {
            moneyText.text = playerMoney.ToString();
        }
    }

    public void RefreshAllSlots()
    {
        if (itemButtonSlots == null)
            return;

        for (int i = 0; i < itemButtonSlots.Length; i++)
        {
            if (itemButtonSlots[i] != null && itemButtonSlots[i].gameObject.activeSelf)
            {
                itemButtonSlots[i].Refresh();
            }
        }
    }

    // Purchase by index instead of the scriptable objects, just in case 2 shops are given the same item, we want stock to be per shop
    public void TryPurchase(Shop targetShop, int itemIndex)
    {
        if (targetShop == null)
            return;

        if (WorldController.instance == null || !WorldController.instance.IsMenuOpen())
            return;

        ShopItem[] items = targetShop.ItemsForSale;
        if (items == null)
            return;

        if (itemIndex < 0 || itemIndex >= items.Length)
            return;

        ShopItem targetItem = items[itemIndex];
        if (targetItem == null)
            return;

        // Stock check
        int stock = targetShop.GetStockAtIndex(itemIndex);
        if (stock <= 0)
            return;

        // Money check
        if (!CanAfford(targetItem.Price))
            return;

        // Spend money
        playerMoney -= targetItem.Price;
        RefreshMoneyUI();

        // Consume 1 stock
        bool consumed = targetShop.TryConsumeStockAtIndex(itemIndex, 1);
        if (!consumed)
        {
            playerMoney += targetItem.Price;
            RefreshMoneyUI();
            RefreshAllSlots();
            return;
        }

        // Apply purchase effects

        // right now the boat does not have IUpgrade on it, so it's not going to do anything
        // this will change once Jose finishes Boat/player stat logic and includes the interface functions
        if (targetItem.ItemType == ShopItemType.Upgrade)
        {
            GameObject playerObject = targetShop.GetPlayer();
            if (playerObject != null)
            {
                IUpgrade upgradeTarget = playerObject.GetComponent<IUpgrade>();
                if (upgradeTarget != null && targetItem.UpgradeDefinition != null)
                {
                    targetItem.UpgradeDefinition.Apply(upgradeTarget);
                }
            }
        }
        else
        {
            // Consumable/Gear placeholder:
            // this else will expand to handle other effects
            // as long as Consumables and gear are scriptable objects with a "public override void Apply(IUpgrade upgradeTarget)" function, they will work
        }

        RefreshAllSlots();
    }
}