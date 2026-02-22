using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetShopButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text quantityText;
    [SerializeField] Image icon;


    // I don't know if I want to do Disable on sold out or Greyed out on sold out
    [SerializeField] GameObject soldOutOverlay;

    ShopItem targetItem;
    Shop targetShop;
    int targetIndex = -1;

    public ShopItem TargetItem => targetItem;

    public void Bind(Shop shop, ShopItem item, int itemIndex)
    {
        targetShop = shop;
        targetItem = item;
        targetIndex = itemIndex;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClicked);
        }
        Refresh();
    }

    public void Clear()
    {
        targetShop = null;
        targetItem = null;
        targetIndex = -1;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.interactable = false;
        }

        if (nameText != null) nameText.text = "";
        if (priceText != null) priceText.text = "";
        if (quantityText != null) quantityText.text = "";

        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }

        if (soldOutOverlay != null)
        {
            soldOutOverlay.SetActive(false);
        }

        gameObject.SetActive(false);
    }


    public void Refresh()
    {
        if (targetShop == null || targetItem == null || targetIndex < 0)
        {
            Clear();
            return;
        }

        gameObject.SetActive(true);

        int stock = targetShop.GetStockAtIndex(targetIndex);

        if (nameText != null) nameText.text = targetItem.DisplayName;
        if (priceText != null) priceText.text = targetItem.Price.ToString();
        if (quantityText != null) quantityText.text = "x" + stock.ToString();

        if (icon != null)
        {
            icon.sprite = targetItem.Icon;
            icon.enabled = (targetItem.Icon != null);
        }

        bool hasStock = stock > 0;
        bool canAfford = ShopUI.instance != null && ShopUI.instance.CanAfford(targetItem.Price);

        if (soldOutOverlay != null)
        {
            soldOutOverlay.SetActive(!hasStock);
        }

        if (button != null)
        {
            button.interactable = hasStock && canAfford;
        }
    }

    void OnClicked()
    {
        if (ShopUI.instance == null) return;

        ShopUI.instance.TryPurchase(targetShop, targetIndex);

        Refresh();
    }
}
