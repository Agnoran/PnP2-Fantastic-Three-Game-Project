using System;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System.Runtime.InteropServices;
using UnityEditor.Purchasing;
using System.Collections.Generic;
using System.Collections;

public enum RodStat
{
    lineHealthMax,
    rodHealthMax,
    rodDamagePower,
    rodLuck,
    rodControl
}
public enum BoatStat
{
    boatHealthMax,
    boatSpeed,
    boatManeuverability
}

public class WorldController : MonoBehaviour
{
    public static WorldController instance;
    [SerializeField] GameObject menuActive;

    [SerializeField] GameObject menuStart;
    [SerializeField] GameObject menuTutorialOne;
    [SerializeField] GameObject menuTutorialTwo;

    [SerializeField] GameObject prevMenuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuFishing;
    [SerializeField] GameObject menuFishCaught;
    [SerializeField] GameObject menuFishLost;
    [SerializeField] GameObject menuInventory;
    [SerializeField] GameObject menuCatalogue;
    [SerializeField] GameObject menuWinGame;
    [SerializeField] GameObject menuShop;

    [SerializeField] AudioClip[] castingSounds;
    [SerializeField] AudioClip[] cheeringSounds;
    [SerializeField] AudioClip[] losingSounds;

    [SerializeField] GameObject localShopButton;
    [SerializeField] GameObject globalShopButton;
    [SerializeField] Shop globalShop;
    public Shop GlobalShop => globalShop;
    [SerializeField] bool globalShopUnlocked = false;
    public bool GlobalShopUnlocked => globalShopUnlocked;
    Shop activeLocalShop;
    public Shop ActiveLocalShop => activeLocalShop;

    int fishValueToWinGame;

    [SerializeField] boatCamera cameraScript;

    Fishing game;

    [SerializeField] TMP_Text playerMoneyTracker;

    [SerializeField] TMP_Text baitDisplay;

    public bool isPaused;
    public bool isFishing;
    public bool invOpen;
    public bool catalogueOpen;
    public bool shopOpen;

    public bool canFish;
    public GameObject currentPool;
    public FishInstance fishToAttempt;

    float timeScaleOrig;
    private bool gameWon;
    public bool GameWon => gameWon;

    private bool startOfGame = false;

    [SerializeField] GameObject player;

    [SerializeField] int playerMoney = 0;
    public int PlayerMoney => playerMoney;

    playerBoat playerScript;

    public bool CanAfford(int cost)
    {
        return playerMoney >= cost;
    }

    public bool TrySpendMoney(int cost)
    {
        if (cost < 0) return false;
        if (playerMoney < cost) return false;
        playerMoney -= cost;
        UpdateFishValueTracker();
        return true;
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0) return;
        playerMoney += amount;
        UpdateFishValueTracker();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        instance = this;
        timeScaleOrig = Time.timeScale;
        startOfGame = true;
        StateBeginGame();
        isPaused = false;
        isFishing = false;
        invOpen = false;
        catalogueOpen = false;

        canFish = false;
        fishToAttempt = null;
        currentPool = null;
        gameWon = false;


        if (cameraScript == null)
            cameraScript = FindAnyObjectByType<boatCamera>();


        ShowGlobalShopButton();
        startOfGame = false;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        // Pause Menu
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                 
                StatePaused();
            }
            else if (menuActive != null && menuActive != menuPause)
            {    
                // if there is a menu active besides the pause menu
                // store that menu so it can be activated after unpause
                prevMenuActive = menuActive;
                if (menuActive != null)
                {
                    menuActive.SetActive(false);
                }

                StatePaused();
            }
            else if (menuActive == menuPause)
            {
                StateUnpaused();
            }
        }

        // Inventory Menu
        else if (Input.GetButtonDown("Inventory"))
        {
            if (menuActive == null)
            {
                StateOpenInventory();
            }
            else if (menuActive == menuInventory)
            {
                StateCloseInventory();
            }
        }

        // Fishing Minigame
        else if (Input.GetButtonDown("Fish"))
        {
            // makes sure the boat is by a pool
            // and double checks there's a pool reference
            if(canFish && currentPool != null)
            {
                if (menuActive != null)
                {
                    return;
                }
                if (menuActive == null)
                {
                    //if (player.GetBait() > 0)
                    //{
                    //    player.addBait(-1);
                    //    StateStartFishing();
                    //}
                    playerBoat pb = (player != null) ? player.GetComponentInChildren<playerBoat>() : null;
                    if (pb != null && pb.getCurrBait() > 0)
                    {
                        pb.addBait(-1);
                        UpdateBaitDisplay(pb.getCurrBait());
                        int soundIndex = UnityEngine.Random.Range(0, castingSounds.Length);
                        if (castingSounds != null && castingSounds.Length > 0)
                        {
                            AudioClip clip = castingSounds[soundIndex];
                            if (clip != null)
                            {
                                AudioSource source = player.GetComponentInChildren<AudioSource>();
                                if (source != null)
                                {
                                    source.PlayOneShot(clip);
                                }
                            }
                        }
                        StateStartFishing();
                    }


;                   
                    
                }
            }

            // this could later be expanded for areas where
            // the player can fish but not at a pool (like cage fishing)

            else return;
        }

        // Catalogue Menu
        else if (Input.GetButtonDown("Catalogue"))
        {
            if (menuActive == null)
            {
                StateOpenCatalogue();
            }
            else if (menuActive == menuCatalogue)
            {
                StateCloseCatalogue();
            }
        }
    }

    private void UpdateBaitDisplay(int currBait)
    {
        if (baitDisplay == null) return;
        baitDisplay.text = currBait.ToString();
    }

    private void UpdateFishValueTracker()
    {
        if (playerMoneyTracker == null) return;
        playerMoneyTracker.text = playerMoney.ToString();
    }

    public void StateBeginGame()
    {
        isPaused = true;
        // pauses Time
        Time.timeScale = 0;
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
        // activates the pause menu
        menuActive = menuStart;
        menuActive.SetActive(true);
        menuActive.transform.SetAsLastSibling(); // bring to front
    }
    public void StateTutorialOne()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
        menuActive = menuTutorialOne;
        menuActive.SetActive(true);
        menuActive.transform.SetAsLastSibling();
    }
    public void StateTutorialTwo()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
        menuActive = menuTutorialTwo;
        menuActive.SetActive(true);
        menuActive.transform.SetAsLastSibling();
    }
    public void StateStartGame()
    {
        StateUnpaused();
        playerBoat pb = (player != null) ? player.GetComponentInChildren<playerBoat>() : null;
        if (pb != null)
        {
            UpdateBaitDisplay(pb.getCurrBait());
        }
        UpdateFishValueTracker();
    }

    /// <summary>
    /// Optional Method to manually set the canFish
    /// </summary>
    /// <param name="set"></param>
    public void SetCanFish(bool set)
    {
        canFish = set;
    }

    /// <summary>
    /// Sets the active menu to the catalogue and activates it
    /// </summary>
    public void StateOpenCatalogue()
    {
        catalogueOpen = true;
        menuActive = menuCatalogue;
        menuActive.SetActive(true);
    }
    /// <summary>
    /// To be called while pause menu is active
    /// </summary>
    public void StateCatFromPause()
    {
        // de-activates the assumed pause Menu
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
        // opens the catalogue
        StateOpenCatalogue();
    }
    /// <summary>
    /// closes the catalogue and resets the menuActive
    /// </summary>
    public void StateCloseCatalogue()
    {
        catalogueOpen = false;
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
        menuActive = null;
    }

    /// <summary>
    /// Pauses the game and opens the pause Menu
    /// </summary>
    public void StatePaused()
    {
        isPaused = true;
        // pauses Time
        Time.timeScale = 0;

        // activates the pause menu
        menuActive = menuPause;
        menuActive.SetActive(true);
        menuActive.transform.SetAsLastSibling(); // bring to front
    }

    /// <summary>
    /// Unpauses the game, closes the pause menu, and then reopens the previous menu if applicable
    /// </summary>
    public void StateUnpaused()
    {
        isPaused = false;

        // resets the time scale
        Time.timeScale = timeScaleOrig;

        // closes pause menu
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }

        // if there was a previous menu, open it
        if (prevMenuActive != null)
        {
            menuActive = prevMenuActive;
            menuActive.SetActive(true);
            prevMenuActive = null;
        }
        else
        {
            menuActive = null;
        }
            
    }
    /// <summary>
    /// asks the current pool for a fish, tells the minigame about that fish, and opens fishing ui
    /// </summary>
    public void StateStartFishing()
    {
        if (currentPool == null)
        {
            return;
        }

        fishToAttempt = null;

        FishingSpot pool = currentPool.GetComponent<FishingSpot>();
        if (pool == null)
        {
            return;
        }

        float rodLuck = 0f;

        if (player != null)
        {
            playerBoat pb = player.GetComponentInChildren<playerBoat>();
            if (pb != null && pb.getCurrRod() != null)
            {
                rodLuck = pb.getCurrRod().RodLuck;
            }
        }

        fishToAttempt = pool.GenerateFishToAttempt(rodLuck);


        if (fishToAttempt != null)
        {
            isFishing = true;
            cameraScript.EnterFishingMode();
            // 
            // TODO: pass the fishToAttempt to Minigame

            menuActive = menuFishing;
            menuActive.SetActive(true);


            
            Fishing.instance.startFishing();
        }
        else return;

    }
    /// <summary>
    /// stops fishing, resets values, and closes fishing ui
    /// </summary>
    public void StateStopFishing()
    {
        isFishing = false;
        cameraScript.ExitFishingMode();
        Fishing.instance.destroyGame();

        if (menuFishing != null)
        {
            menuFishing.SetActive(false);
        }

        if (menuActive == menuFishing)
        {        
            menuActive = null;
        }

    }
    /// <summary>
    /// opens inventory ui
    /// </summary>
    public void StateOpenInventory()
    {
        invOpen = true;
        menuActive = menuInventory;
        menuActive.SetActive(true);

    }
    /// <summary>
    /// To be called while pause menu is active
    /// </summary>
    public void StateInvFromPause()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
        StateOpenInventory();
    }
    /// <summary>
    /// closes the inventory and resets the menuActive
    /// </summary>
    public void StateCloseInventory()
    {
        invOpen = false;
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
        menuActive = null;
        UpdateFishValueTracker();
    }

    private void checkForWinGame()
    {
        return;

        // we are now winning the game by an item now

        //if (InventorySystem.instance.GetTotalFishValue() >= fishValueToWinGame)
        //{
        //    StateWinGame();
        //}
    }

    public void StateWinGame()
    {
        if (gameWon) return;

        AudioClip clip = null;

        if (cheeringSounds != null && cheeringSounds.Length > 0)
        {
            int soundIndex = UnityEngine.Random.Range(0, cheeringSounds.Length);
            clip = cheeringSounds[soundIndex];
        }

        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }

        menuActive = menuWinGame;
        if (menuActive != null)
        {
            menuActive.SetActive(true);
            menuActive.transform.SetAsLastSibling();
        }

        gameWon = true;

        if (clip != null)
        {
            AudioSource source = (player != null) ? player.GetComponentInChildren<AudioSource>() : null;
            if (source != null)
            {
                source.PlayOneShot(clip);
                StartCoroutine(PauseAfterRealtime(clip.length * 0.9f));
                return;
            }
        }

        Time.timeScale = 0f;
    }

    IEnumerator PauseAfterRealtime(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// called by the pool whn the player enters proximity
    /// </summary>
    /// <param name="pool"></param>
    public void EnterPool(GameObject pool)
    {
        currentPool = pool;
        canFish = true;
    }
    /// <summary>
    /// called by the pool whn the player exits proximity
    /// </summary>
    public void ExitPool()
    {
        currentPool = null;
        canFish = false;
    }

    /// <summary>
    /// opens and populates the correct fish result ui
    /// </summary>
    /// <param name="wasCaught"></param>
    public void ResolveFishingAttempt(bool wasCaught)
    {
        StateStopFishing();
        Fishing.instance.destroyGame();
        if (wasCaught)
        {
            int soundIndex = UnityEngine.Random.Range(0, cheeringSounds.Length);
            if (cheeringSounds != null && cheeringSounds.Length > 0)
            {
                AudioClip clip = cheeringSounds[soundIndex];
                if (clip != null)
                {
                    AudioSource source = player.GetComponentInChildren<AudioSource>();
                    if (source != null)
                    {
                        source.PlayOneShot(clip);
                    }
                }
            }

            menuActive = menuFishCaught;
            menuActive.SetActive(true);
            UpdateFishValueTracker();
        }
        else
        {
            int soundIndex = UnityEngine.Random.Range(0, losingSounds.Length);
            if (losingSounds != null && losingSounds.Length > 0)
            {
                AudioClip clip = losingSounds[soundIndex];
                if (clip != null)
                {
                    AudioSource source = player.GetComponentInChildren<AudioSource>();
                    if (source != null)
                    {
                        source.PlayOneShot(clip);
                    }
                }
            }
            menuActive = menuFishLost;
            menuActive.SetActive(true);
            UpdateFishValueTracker();
        }
    }

    public void FinishFishingResult()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
        
        fishToAttempt = null;
        menuActive = null;
        isFishing = false;
        UpdateFishValueTracker();
    }
    public void CutTheLine()
    {
        StateStopFishing();     // closes fishing UI + sets isFishing false
        fishToAttempt = null;   // flush the attempt
    }

    public bool IsMenuOpen()
    {
        if (isFishing || isPaused || invOpen || catalogueOpen || gameWon || shopOpen)
        {
            return true;
        }
        return false;
    }

    public void SetGlobalShopUnlocked(bool unlocked)
    {
        globalShopUnlocked = unlocked;
        ShowGlobalShopButton();
    }
    public void ShowGlobalShopButton()
    {
        if (globalShopButton == null) { return; }

        bool show = globalShopUnlocked && globalShop != null;

        globalShopButton.SetActive(show);
    }
    public void showShopButton(bool show, Shop localShop)
    {
        if (localShopButton == null) { return; }

        if (show)
        {
            activeLocalShop = localShop;

            if (activeLocalShop == null)
            {
                localShopButton.SetActive(false);
                return;
            }

            localShopButton.SetActive(true);
            return;
        }

        if(localShop != null && localShop != activeLocalShop) { return; }

        activeLocalShop = null;
        localShopButton.SetActive(false);
    }

    internal void StateOpenShop()
    {
        shopOpen = true;

        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }

        menuActive = menuShop;
        if (menuActive != null)
        {
            menuActive.SetActive(true);
            menuActive.transform.SetAsLastSibling();
        }
    }

    internal void StateCloseShop()
    {
        shopOpen = false;

        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }

        menuActive = null;
    }
    public void TriggerWinFromShop()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
        menuActive = menuWinGame;
        menuActive.SetActive(true);
        gameWon = true;
    }
    public void RefreshBaitDisplay()
    {
        playerBoat pb = (player != null) ? player.GetComponentInChildren<playerBoat>() : null;
        if (pb == null) return;

        UpdateBaitDisplay(pb.getCurrBait());
    }
}