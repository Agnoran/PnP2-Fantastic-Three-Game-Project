using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;




    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }


    public void Open(Shop shop)
    {
        if (shop == null) return;
        if (WorldController.instance == null) return;
        if (WorldController.instance.IsMenuOpen()) return;
        WorldController.instance.StateOpenShop();
    }

    public void Close(Shop shop)
    {
        if (shop == null) return;
        if (WorldController.instance == null) return;
        if (!WorldController.instance.IsMenuOpen()) return;
        WorldController.instance.StateCloseShop();
    }

    public void TryPurchase(Shop targetShop, ShopItem targetItem)
    {
        if(targetShop == null || targetItem == null) return;

        if (WorldController.instance.IsMenuOpen() == false)
        {
            // Nothing to buy if the shop isn't open
            // This could change if we make "quick buy" buttons or something

            return;
        }

        GameObject playerObject = targetShop.GetPlayer();
        if (playerObject == null) return;

    }
}
