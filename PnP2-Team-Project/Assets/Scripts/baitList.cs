using UnityEngine;

[System.Serializable]
public class baitList
{

    public string baitName = "Worm";
    public int quantity = 0;
    public float catchBonus = 0f;
    public string[] attractsFish;

    public bool Use()
    {
        if (quantity <= 0) return false;
        quantity--;
        return true;
    }
    public void Add(int amount)
    {
        quantity += amount;
    }

    public bool HasAny()
    {
        return quantity > 0;
    }

}
