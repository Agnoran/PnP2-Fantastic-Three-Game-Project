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
    [SerializeField] GameObject fishDisplay;
    [SerializeField] GameObject fishCatchLoc;
    [SerializeField] GameObject fishLossLoc;

    [SerializeField] GameObject player;
    public playerBoat playerBoat;
    [SerializeField] public float difficultyMod;
    public float DifficultyMod => difficultyMod;
    [SerializeField] public int progLoss;
    public int ProgLoss => progLoss;
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
        playerBoat = player.GetComponent<playerBoat>();

        minigame = Instantiate(minigame_LeftRight);
        minigame.transform.SetParent(parentCanvas, true);

        // TODO JUNE:
        // grab fish instance from worldController
        // update fish display text(s) with fish info (type, difficulty, etc)
    }


    public void destroyGame()
    {
        if (minigame != null)
        {
            Destroy(minigame);
            minigame = null;
        }

    }
    public void UpdateFishLocation(float amount)
    {
        // float difference = (fishCatchLoc.transform.position.y + fishLossLoc.transform.position.y);
        fishDisplay.transform.position = new Vector2(fishDisplay.transform.position.x, fishLossLoc.transform.position.y + amount);
    }

}