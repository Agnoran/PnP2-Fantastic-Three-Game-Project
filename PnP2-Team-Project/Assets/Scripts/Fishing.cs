using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    public static Fishing instance;

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

        minigame = Instantiate(minigame_LeftRight);
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
}