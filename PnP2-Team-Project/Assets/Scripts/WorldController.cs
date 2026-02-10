using System;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static WorldController instance;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject prevMenuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuFishing;
    [SerializeField] GameObject menuFishCaught;
    [SerializeField] GameObject menuFishLost;
    [SerializeField] GameObject menuInventory;
    [SerializeField] GameObject menuCatalogue;

    public bool isPaused;
    public bool isFishing;
    public bool invOpen;
    public bool catalogueOpen;

    public bool canFish;
    public GameObject currentPool;
    public FishInstance fishToAttempt;

    float timeScaleOrig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;

        isPaused = false;
        isFishing = false;
        invOpen = false;
        catalogueOpen = false;
        menuActive = null;

        canFish = false;
        fishToAttempt = null;
        currentPool = null;

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
                if (menuActive == null)
                {
                    StateStartFishing();
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
    private void StateOpenCatalogue()
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

        TempFishingPool pool = currentPool.GetComponent<TempFishingPool>();
        if (pool == null)
        {
            return;
        }

        fishToAttempt = pool.GenerateFishToAttempt();
        

        if (fishToAttempt != null)
        {
            isFishing = true;
            // 
            // TODO: pass the fishToAttempt to Minigame
            //

            menuActive = menuFishing;
            menuActive.SetActive(true);
        }
        else return;

    }
    /// <summary>
    /// stops fishing, resets values, and closes fishing ui
    /// </summary>
    public void StateStopFishing()
    {
        isFishing = false;

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
        if (wasCaught)
        {
            menuActive = menuFishCaught;
            menuActive.SetActive(true);
        }
        else
        {
            menuActive = menuFishLost;
            menuActive.SetActive(true);
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
    }
    public void CutTheLine()
    {
        StateStopFishing();     // closes fishing UI + sets isFishing false
        fishToAttempt = null;   // flush the attempt
    }

}
