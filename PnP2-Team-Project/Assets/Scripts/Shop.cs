using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] ShopCategory shopCategory;
    [SerializeField] ShopItem[] itemsForSale;
    [SerializeField] bool requirePlayerNearby = true;

    [Header("Restock")]
    [SerializeField] bool useRestock = true;
    [SerializeField] int restockEveryMinutes = 30;
    [SerializeField] int restockAmountPerInterval = 1;

    bool playerNearby;
    GameObject player;

    int[] stockRemaining;
    int[] stockMax;

    double nextRestockMinute;

    public ShopCategory ShopCategory => shopCategory;
    public ShopItem[] ItemsForSale => itemsForSale;

    void Awake()
    {
        InitializeStockFromTemplates();
        nextRestockMinute = GetNowMinutes() + restockEveryMinutes;
    }

    void OnEnable()
    {
        if (WorldClock.instance != null)
        {
            WorldClock.instance.OnMinuteChanged += HandleMinuteChanged;
        }
    }

    void OnDisable()
    {
        if (WorldClock.instance != null)
        {
            WorldClock.instance.OnMinuteChanged -= HandleMinuteChanged;
        }
    }

    void InitializeStockFromTemplates()
    {
        if (itemsForSale == null)
        {
            stockRemaining = new int[0];
            stockMax = new int[0];
            return;
        }

        stockRemaining = new int[itemsForSale.Length];
        stockMax = new int[itemsForSale.Length];

        for (int i = 0; i < itemsForSale.Length; i++)
        {
            ShopItem item = itemsForSale[i];
            int templateQty = (item != null) ? Mathf.Max(0, item.Quantity) : 0;

            stockRemaining[i] = templateQty;
            stockMax[i] = templateQty;
        }
    }

    double GetNowMinutes()
    {
        if (WorldClock.instance != null)
        {
            return WorldClock.instance.TotalMinutes;
        }

        return Time.timeAsDouble / 60.0;
    }

    void HandleMinuteChanged()
    {
        if (!useRestock)
            return;

        double now = GetNowMinutes();

        if (now < nextRestockMinute)
            return;

        while (now >= nextRestockMinute)
        {
            RestockOnce();
            nextRestockMinute += restockEveryMinutes;
        }

        if (ShopUI.instance != null)
        {
            ShopUI.instance.RefreshShopIfShowing(this);
        }
    }

    void RestockOnce()
    {
        if (stockRemaining == null || stockMax == null)
            return;

        for (int i = 0; i < stockRemaining.Length; i++)
        {
            if (stockMax[i] <= 0)
                continue;

            stockRemaining[i] = Mathf.Min(stockMax[i], stockRemaining[i] + restockAmountPerInterval);
        }
    }

    public int GetStockAtIndex(int index)
    {
        if (stockRemaining == null)
            return 0;

        if (index < 0 || index >= stockRemaining.Length)
            return 0;

        return stockRemaining[index];
    }

    public bool TryConsumeStockAtIndex(int index, int amount)
    {
        if (amount <= 0)
            return false;

        if (stockRemaining == null)
            return false;

        if (index < 0 || index >= stockRemaining.Length)
            return false;

        if (stockRemaining[index] < amount)
            return false;

        stockRemaining[index] -= amount;
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (requirePlayerNearby && !other.CompareTag("Player"))
            return;

        playerNearby = true;
        player = other.gameObject;

        if (WorldController.instance != null)
        {
            WorldController.instance.showShopButton(true, this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (requirePlayerNearby && !other.CompareTag("Player"))
            return;

        player = null;
        playerNearby = false;

        if (WorldController.instance != null)
        {
            WorldController.instance.showShopButton(false, this);
        }
    }

    public bool CanOpenShop()
    {
        return playerNearby;
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}