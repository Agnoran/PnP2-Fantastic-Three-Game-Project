using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    [Header("Info Section")]
    [SerializeField] public GameObject infoPos;
    [SerializeField] public InGameInfo_Chunk infoEntry;
    [SerializeField] bool knownFish;
    List<InGameInfo_Chunk> fishInfoChunks;
    Vector2 infoListPosition;
    int infoListIndex;

    [Header("Minigames")]
    [SerializeField] GameObject fishingMinigame;
    [SerializeField] GameObject cutTheLine;
    GameObject minigame;

    [Header("Hard coded fish for testing difficulties")]
    [SerializeField] FishType type;
    FishInstance fishinst;

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

        //prepare stats to reveal during game progress
        prepFishInfo();


        //run the fishing minigame
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

  
    public void updateGameProgress()
    {

    }


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


}