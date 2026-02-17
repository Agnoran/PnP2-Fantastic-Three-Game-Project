using UnityEngine;
using UnityEngine.UIElements;

public enum ShopType
{
    BaitShop,
    Sell,
    BoatUpgrade,
    RodUpgrade,
    Services

}
public class ShopItem
{
    [SerializeField] string name;
    [TextArea(2,6)] [SerializeField] string description;

    [SerializeField] GameObject item;

    [SerializeField] int price;
    [SerializeField] int quantity;
    [SerializeField] Image itemImage;
    [SerializeField] ShopType[] shopTypes;
}
