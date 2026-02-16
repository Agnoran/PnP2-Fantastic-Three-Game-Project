using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    public static Fishing instance;

    [SerializeField] GameObject gameToMake;

    [SerializeField] GameObject minigame_LeftRight;
    [SerializeField] GameObject minigame_Circle;
    [SerializeField] GameObject minigame_TugOfWar;

    GameObject minigame;
    [SerializeField] Transform parentCanvas;


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
        destroyGame();
        
        //get fish
        FishType type = WorldController.instance.fishToAttempt.Type;

        //call appropriate game
        switch (type)
        {
            case FishType.Trout:
                gameToMake = minigame_LeftRight;
                break;
            case FishType.Shark:
                gameToMake = minigame_Circle;
                break;

        }
        minigame = Instantiate(gameToMake);



        minigame.transform.SetParent(parentCanvas, true);
        

        //future: 
        //get fish type, pick appropriate game
    }


    public void destroyGame()
    {
        if (minigame != null)
        {
            Destroy(minigame);
            minigame = null;
        }

    }


    public float calcDifficulty()
    {
        float diff = 1f;


        return diff;
    }

}