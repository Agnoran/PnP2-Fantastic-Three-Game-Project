using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;

    [SerializeField] GameObject shopMenuRoot;
    [SerializeField] SetShopButton[] itemButtonSlots;

    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text shopTitleText;

    [Header("Sell Fish")]
    [SerializeField] GameObject sellFishButtonRoot;
    [SerializeField] TMP_Text sellFishButtonText;

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

    void OnEnable()
    {
        if (InventorySystem.instance != null)
        {
            InventorySystem.instance.OnInventoryChanged += HandleInventoryChanged;
        }
    }

    void OnDisable()
    {
        if (InventorySystem.instance != null)
        {
            InventorySystem.instance.OnInventoryChanged -= HandleInventoryChanged;
        }
    }

    void HandleInventoryChanged()
    {
        RefreshSellFishButton();
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
        RefreshShop();

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
        RefreshSellFishButton();

        WorldController.instance.StateCloseShop();
    }

    public void RefreshShopIfShowing(Shop shop)
    {
        if (shop == null) return;
        if (currentShop != shop) return;

        RefreshShop();
    }

    public void RefreshShop()
    {
        RefreshMoneyUI();
        RefreshAllSlots();
        RefreshSellFishButton();
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
        if (moneyText == null) return;
        if (WorldController.instance == null) return;

        moneyText.text = WorldController.instance.PlayerMoney.ToString();
    }

    void RefreshAllSlots()
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

    void RefreshSellFishButton()
    {
        if (sellFishButtonRoot == null || sellFishButtonText == null)
            return;

        bool atFishStore = (currentShop != null && currentShop.ShopCategory == ShopCategory.FishAndTackle);

        sellFishButtonRoot.SetActive(atFishStore);

        if (!atFishStore)
            return;

        int totalValue = 0;

        if (InventorySystem.instance != null)
        {
            totalValue = InventorySystem.instance.GetTotalFishValue();
        }

        sellFishButtonText.text = $"Sell All (${totalValue})";

        UnityEngine.UI.Button button = sellFishButtonRoot.GetComponent<UnityEngine.UI.Button>();
        if (button != null)
        {
            button.interactable = totalValue > 0;
        }
    }

    public void SellAllFish()
    {
        if (currentShop == null)
            return;

        if (currentShop.ShopCategory != ShopCategory.FishAndTackle)
            return;

        if (InventorySystem.instance == null)
            return;

        if (WorldController.instance == null)
            return;

        int totalValue = InventorySystem.instance.GetTotalFishValue();
        if (totalValue <= 0)
            return;

        var fishList = InventorySystem.instance.GetAllFish();
        for (int i = fishList.Count - 1; i >= 0; i--)
        {
            InventorySystem.instance.RemoveFish(fishList[i]);
        }

        WorldController.instance.AddMoney(totalValue);
        RefreshShop();
    }

    public void TryPurchase(Shop targetShop, int itemIndex)
    {
        if (targetShop == null) return;
        if (WorldController.instance == null) return;
        if (!WorldController.instance.shopOpen) return;

        ShopItem[] items = targetShop.ItemsForSale;
        if (items == null) return;

        if (itemIndex < 0 || itemIndex >= items.Length) return;

        ShopItem targetItem = items[itemIndex];
        if (targetItem == null) return;

        int stock = targetShop.GetStockAtIndex(itemIndex);
        if (stock <= 0) return;

        if (!WorldController.instance.TrySpendMoney(targetItem.Price))
            return;

        if (!targetShop.TryConsumeStockAtIndex(itemIndex, 1))
        {
            WorldController.instance.AddMoney(targetItem.Price);
            RefreshShop();
            return;
        }
        if (targetItem.ItemType == ShopItemType.Win)
        {
            if (WorldController.instance != null)
            {
                WorldController.instance.TriggerWinFromShop();
                return;
            }
        }
        if (targetItem.UpgradeDefinition != null)
        {
            
            GameObject playerObject = targetShop.GetPlayer();
            if (playerObject != null)
            {
                IUpgrade upgradeTarget = playerObject.GetComponent<IUpgrade>();
                if (upgradeTarget != null)
                {
                    targetItem.UpgradeDefinition.Apply(upgradeTarget);
                }
            }
        }
        else
        {
            if (targetItem.ItemPrefab != null)
            {
                Instantiate(targetItem.ItemPrefab);
            }
        }

        RefreshShop();
        if (WorldController.instance != null)
        {
            WorldController.instance.RefreshBaitDisplay();
        }

        if (WorldController.instance.GameWon)
        {
            Close();
        }
    }
    public bool CanAfford(int cost)
    {
        return WorldController.instance != null && WorldController.instance.CanAfford(cost);
    }
}