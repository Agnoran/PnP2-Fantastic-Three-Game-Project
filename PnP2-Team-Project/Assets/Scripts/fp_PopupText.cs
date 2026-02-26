using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class fp_PopupText : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] FishingSpot pool;
    FishDefinition[] availFish;
    //string[] availFish;

    void Start()
    {
        availFish = pool.availableFishies;
        string fishString = "";
        for (int i = 0; i < availFish.Length; i++)
        {
            if (availFish[i].Type == FishType.Boot)
            {
                fishString += "Boot\n";
            }
            if (availFish[i].Type == FishType.Trout)
            {
                fishString += "Trout\n";
            }
            if (availFish[i].Type == FishType.Shark)
            {
                fishString += "Shark\n";
            }
        }
        text.text = fishString;
    }


}
