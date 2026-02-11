using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    [SerializeField] GameObject minigame_LeftRight;
    [SerializeField] GameObject minigame_Circle;
    [SerializeField] GameObject minigame_TugOfWar;

    FishType fType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get fish type
        fType = WorldController.instance.fishToAttempt.Type;
        //decide which minigame to run
        if (fType == FishType.Shark)
        {
            Instantiate(minigame_LeftRight);
        }
        else if (fType == FishType.Catfish)
        {
            Instantiate(minigame_LeftRight);
        }
        else
        {
            Instantiate(minigame_LeftRight);
        }

            
    }


}