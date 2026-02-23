using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] ShopCategory shopCategory;
    [SerializeField] ShopItem[] itemsForSale;

    [SerializeField] bool requirePlayerNearby = true;

    bool playerNearby = false;
    GameObject player;

    int[] stockRemaining;

    public ShopCategory ShopCategory => shopCategory;
    public ShopItem[] ItemsForSale => itemsForSale;

    void Awake()
    {
        InitializeStockFromTemplates();
    }

    void InitializeStockFromTemplates()
    {
        if (itemsForSale == null)
        {
            stockRemaining = new int[0];
            return;
        }

        stockRemaining = new int[itemsForSale.Length];

        for (int i = 0; i < itemsForSale.Length; i++)
        {
            ShopItem item = itemsForSale[i];

            if (item == null)
            {
                stockRemaining[i] = 0;
                continue;
            }

            stockRemaining[i] = Mathf.Max(0, item.Quantity);
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
        if (requirePlayerNearby && !other.CompareTag("Player")) { return; }

        playerNearby = true;
        player = other.gameObject;
        WorldController.instance.showShopButton(true, this);
    }

    void OnTriggerExit(Collider other)
    {
        if (requirePlayerNearby && !other.CompareTag("Player")) { return; }

        player = null;
        playerNearby = false;

        WorldController.instance.showShopButton(false, this);
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