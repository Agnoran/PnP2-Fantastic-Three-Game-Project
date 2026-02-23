using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Audio.GeneratorInstance;

public class Fishing : MonoBehaviour
{
    public static Fishing instance;

    [Header("UI stuff")]
    [SerializeField] Canvas parentCanvas;
    [SerializeField] UnityEngine.UI.Image fish;
    [SerializeField] public GameObject minigamePosition;

    [Header("Minigames")]
    [SerializeField] GameObject fishingMinigame;
    [SerializeField] GameObject cutTheLine;
    GameObject minigame;

    [Header("Hard coded fish for testing difficulties")]
    [SerializeField] FishType type;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public void startFishing()
    {
        //ensure single instance
        destroyGame();

        minigame = fishingMinigame;

        //get fish
        //type = WorldController.instance.fishToAttempt.Type;

        minigame = Instantiate(fishingMinigame);


    }


    public void destroyGame()
    {
        if (minigame != null)
        {
            Destroy(minigame);
            minigame = null;
        }

    }


    //called inside the minigames on startup
    public float calcDifficulty()
    {
        float diff = 1f;

        if (type == FishType.Shark)
        {
            diff *= 3;
        } 
        else if (type == FishType.Boot)
        {
            diff = -1f;
        }

            return diff;
    }

 

}