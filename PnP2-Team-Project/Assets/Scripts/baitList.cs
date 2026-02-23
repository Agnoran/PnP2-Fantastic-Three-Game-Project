using UnityEngine;

[System.Serializable]
public class Bait
{

    public string baitName = "Worm";
    public int quantity = 0;

    [Tooltip("Bonus catch chance")]  
    public float catchBonus = 0f;
    public BaitType baitType = BaitType.None;


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
