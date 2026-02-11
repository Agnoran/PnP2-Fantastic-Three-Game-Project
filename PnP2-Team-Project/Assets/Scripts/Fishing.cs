using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    public static Fishing instance;

    [SerializeField] GameObject minigame_LeftRight;
    [SerializeField] GameObject minigame_Circle;
    [SerializeField] GameObject minigame_TugOfWar;

    protected FishType fType;
    GameObject minigame;


    private void Awake()
    {
        instance = this;
    }

    public void startFishing()
    {
        fType = WorldController.instance.fishToAttempt.Type;
        minigame = Instantiate(minigame_LeftRight);
        
        //future: 
        //get fish type, pick appropriate game
    }


    protected void destroyGame()
    {
        Destroy(minigame);
    }
}