using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Fishing_forMarch : MonoBehaviour
{
    public static Fishing_forMarch instance;

    [SerializeField] Transform parentCanvas;
    [SerializeField] public GameObject minigamePosition;

    [Header("Minigames")]
    [SerializeField] GameObject minigameInstance;
    [SerializeField] GameObject cutTheLine;
    [SerializeField] GameObject onLossCutLinePos;
    Vector3 originalCutLinePos;
    Vector3 originalCutLineScale;
    GameObject minigame;





    [Header("Fish")]
    [SerializeField] UnityEngine.UI.Image fishHookParent;
    [SerializeField] UnityEngine.UI.Image fishWinPosition;
    [SerializeField] UnityEngine.UI.Image fishLosePosition;
    float winPositionY;
    float losePositionY;
    float fhPositionY;
    float fhPositionX;



    [Header("Game Stats")]
    [SerializeField] float baseAddVal;
    [SerializeField] float baseSubVal;
    [Tooltip("purchased upgrade")]
    [SerializeField] float testRodPowerUpgrade;
    [Tooltip("purchased upgrade")]
    [SerializeField] float testRodControlUpgrade;
    [Tooltip("base value")]
    [SerializeField] int baseRodPower;
    [Tooltip("base value")]
    [SerializeField] int baseRodControl;
    //calculated on startup:
    // - affects successful "attempt" 
    float rodPower;
    // - stronger rod reduces how much fish passively goes down
    float rodControl;
    //this is how much the fish will passively move down  
    float baseMoveVal;
    Vector3 baseMoveVec3;
    //how much "attempt" will raise/lower fish
    float addProgressVal;
    float subProgressVal;


    [Header("Hard coded fish for testing difficulties")]
    [SerializeField] FishType testType;
    [SerializeField] float testSize;
    FishType type;
    float fishSize;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        //image+position stuff
        setupUI();

        //calculate stats to drive fish+rod during gameplay
        calcGameStats();


        
    }

    private void Update()
    {
        baseMovement();
    }

    public void destroyGame()
    {
        if (minigame != null)
        {
            Destroy(minigame);
            minigame = null;
        }

    }

    public void startFishing()
    {
        //ensure single instance
        destroyGame();
        //run the fishing minigame
        minigameInstance.SetActive(true);
        //minigameInstance.setAwake(true);
    }

    //called inside the minigame on startup
    public float calcDifficulty()
    {

        float difficultyValue = 1f;

        if (type == FishType.Shark)
        {
            difficultyValue *= 3;
        }
        else if (type == FishType.Boot)
        {
            difficultyValue = -1f;
        }

        return difficultyValue;
    }


    void setupUI()
    {
        winPositionY = fishWinPosition.rectTransform.localPosition.y;
        losePositionY = fishLosePosition.rectTransform.localPosition.y;
        fhPositionY = fishHookParent.rectTransform.localPosition.y;
        fhPositionX = fishHookParent.rectTransform.localPosition.x;

        originalCutLinePos = cutTheLine.transform.position;
        originalCutLineScale = cutTheLine.transform.localScale;
    }

    void calcGameStats()
    {
        //rod stats
        rodPower = baseRodPower + testRodPowerUpgrade;
        rodControl = baseRodControl + testRodControlUpgrade;

        //fish stats
        fishSize = testSize;
        //subtract from current Ypos
        baseMoveVal = (50 * fishSize) - (10 * rodControl);

        //calculate add/subtract values
        // - add this on success
        addProgressVal = baseAddVal + rodPower;
        // - sub this on failure
        subProgressVal = baseSubVal + fishSize - rodControl;
    }


    //on successful hit
    public void addProgress()
    {
        fhPositionY += addProgressVal;
        fishHookParent.rectTransform.localPosition = new Vector2(0, fhPositionY);
        
    }

    //on unsuccessful hit
    public void subProgress()
    {
        fhPositionY -= subProgressVal;
        fishHookParent.rectTransform.localPosition = new Vector2(0, fhPositionY);
    }

   
    public void baseMovement()
    {
        //passive fish movement
        fhPositionY -= baseMoveVal * Time.deltaTime;
        fishHookParent.rectTransform.anchoredPosition = new Vector2(0, fhPositionY);
        //fishHookParent.rectTransform.localPosition = Vector2.Lerp(fishHookParent.rectTransform.localPosition, new Vector2(0, fhPositionY),  2);

        //check win/loss condition
        if (fishHookParent.rectTransform.anchoredPosition.y > fishWinPosition.rectTransform.anchoredPosition.y)
        {
            endCurrentGame(true);
        }

        if (fishHookParent.rectTransform.anchoredPosition.y < fishWinPosition.rectTransform.anchoredPosition.y)
        {
            endCurrentGame(false);
        }
    }

    void endCurrentGame(bool win)
    {
        //setAwake(false);
        if (win)
        {
            WorldController.instance.ResolveFishingAttempt(true);
            
        }
        else
        {
            Destroy(minigame);
            //runCutLine();
        }
    }


    void runCutLine()
    {
        cutTheLine.transform.position = Vector3.Lerp(originalCutLinePos, onLossCutLinePos.transform.position, Time.deltaTime);
        cutTheLine.transform.localScale = Vector3.Lerp(originalCutLineScale, originalCutLineScale * 2, Time.deltaTime);
        Instantiate(cutTheLine);
    }



    //info section
/*
    [Header("Info Section")]
    [SerializeField] public GameObject infoPos;
    [SerializeField] public InGameInfo_Chunk infoEntry;
    [SerializeField] bool knownFish;
    List<InGameInfo_Chunk> fishInfoChunks;
    Vector2 infoListPosition;
    int infoListIndex;
    void prepFishInfo()
    {
        infoListPosition = infoPos.transform.position;
        //create fields and add to list:

        // - fish image
        InGameInfo_Chunk imageChunk = Instantiate(infoEntry, infoPos.transform);
        imageChunk.createChunk_Image(type, knownFish);
        fishInfoChunks.Add(imageChunk);
        // - type
        InGameInfo_Chunk typeChunk = Instantiate(infoEntry, infoPos.transform);
        typeChunk.createChunk_Text("TYPE:", "TESTTYPE");
        fishInfoChunks.Add(typeChunk);
        // - size
        InGameInfo_Chunk sizeChunk = Instantiate(infoEntry, infoPos.transform);
        sizeChunk.createChunk_Text("SIZE:", "TESTTYPE");
        fishInfoChunks.Add(sizeChunk);
        // - quality
        InGameInfo_Chunk qualityChunk = Instantiate(infoEntry, infoPos.transform);
        qualityChunk.createChunk_Text("QUALITY:", "TESTTYPE");
        fishInfoChunks.Add(qualityChunk);
        // - value
        InGameInfo_Chunk valueChunk = Instantiate(infoEntry, infoPos.transform);
        valueChunk.createChunk_Text("QUALITY:", "TESTTYPE");
        fishInfoChunks.Add(valueChunk);

        //set positions
        for (int i = 0; i < fishInfoChunks.Count; i++)
        {
            fishInfoChunks[i].transform.position = new Vector2(infoListPosition.x, infoListPosition.y - (i * 20));
        }

        //set list position to first text field, to start by revealing
        infoListIndex = 1;
    }

    void revealText()
    {
        //toggle the current list item's "obfuscator" to reveal full info
        fishInfoChunks[infoListIndex].revealField();

        //increment list position for next time
        infoListIndex++;
    }
*/

}