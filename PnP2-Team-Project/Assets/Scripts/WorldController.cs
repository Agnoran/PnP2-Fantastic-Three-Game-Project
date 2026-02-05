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
    float timeScaleOrig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;

        canFish = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePaused();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive != null && menuActive != menuPause)
            {
                statePaused();
                prevMenuActive = menuActive;
                menuActive.SetActive(false);
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpaused();
            }
        }
        else if (Input.GetButtonDown("Inventory"))
        {
            if (menuActive == null)
            {
                stateOpenInventory();
            }
            else if (menuActive == menuInventory)
            {
                stateCloseInventory();
            }
        }
        else if (Input.GetButtonDown("Fish"))
        {
            if(canFish)
            {
                if (menuActive == null)
                {
                    stateStartFishing();
                }
            }

            else return;
        }
        else if (Input.GetButtonDown("Catalogue"))
        {
            if (menuActive == null)
            {
                stateOpenCatalogue();
            }
            else if (menuActive == menuCatalogue)
            {
                stateCloseCatalogue();
            }
        }
    }
    public void SetCanFish(bool set)
    {
        canFish = set;
    }

    private void stateOpenCatalogue()
    {
        catalogueOpen = true;
        menuActive = menuCatalogue;
        menuActive.SetActive(true);
    }
    public void stateCatFromPause()
    {
        menuActive.SetActive(false);
        stateOpenCatalogue();
    }
    private void stateCloseCatalogue()
    {
        catalogueOpen = false;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void statePaused()
    {
        isPaused = true;
        Time.timeScale = 0;
    }
    public void stateUnpaused()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        menuActive.SetActive(false);
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
    public void stateStartFishing()
    {
        isFishing = true;
        menuActive = menuFishing;
        menuActive.SetActive(true);
    }
    public void stateStopFishing()
    {
        isFishing = false;
        menuActive.SetActive(false);
        menuActive = null;
    }
    public void stateOpenInventory()
    {
        invOpen = true;
        menuActive = menuInventory;
        menuActive.SetActive(true);

    }
    public void stateInvFromPause()
    {
        menuActive.SetActive(false);
        stateOpenInventory();
    }
    public void stateCloseInventory()
    {
        invOpen = false;
        menuActive.SetActive(false);
        menuActive = null;
    }
    
}
