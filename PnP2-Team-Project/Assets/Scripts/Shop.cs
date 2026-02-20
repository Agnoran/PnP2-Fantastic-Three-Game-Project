using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] ShopCategory shopCategory;

    [SerializeField] ShopItem[] itemsForSale;

    [SerializeField] bool requirePlayerNearby = true;

    bool playerNearby = false;
    GameObject player;
    GameObject shopButton;

    public ShopCategory ShopCategory => shopCategory;
    public ShopItem[] ItemsForSale => itemsForSale;

    void OnTriggerEnter(Collider other)
    {
        if (requirePlayerNearby && !other.CompareTag("Player")) { return; }

        playerNearby = true;
        player = other.gameObject;
        WorldController.instance.showShopButton(true, this);
    }
    private void OnTriggerExit(Collider other)
    {
        if(requirePlayerNearby && !other.CompareTag("Player")) { return; }

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
