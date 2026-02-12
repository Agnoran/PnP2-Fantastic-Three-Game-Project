using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JetBrains.Annotations;
using UnityEditor;

// written by jose
public class gamemanager : MonoBehaviour
{

    public static gamemanager instance;

    //Game Menu

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    //[SerializeField] GameObject menuInactive;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    // Game goal

    [SerializeField] TMP_Text gameGoalCountText;
    [SerializeField] int fishNeededToWin = 5;

    private int fishCaught = 0;

    private bool isPaused = false;
    private GameObject activeMenu = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (menuPause != null) menuPause.SetActive(false);
        if (menuWin != null) menuWin.SetActive(false);
        if (menuLose != null) menuLose.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        UpdateFishUI();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attempt"))
        {
            if (activeMenu != null && activeMenu == menuPause)
            {
                UnpauseGame();
            }
            else if (activeMenu == null)
            {
                PauseGame();
            }

        }
       
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (menuPause != null)
        {
            menuPause.SetActive(true);
            activeMenu = menuPause;
        }

    }

    public void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (menuPause != null)
                menuPause.SetActive(false);

            activeMenu = null;
    }

    public void FishCaught()
    {
        fishCaught++;
        Debug.Log("Fish caugh! Total: " + fishCaught + "/" + fishNeededToWin);

        UpdateFishUI();

        if (fishCaught >= fishNeededToWin)
        {
            WinGame();
        }

        
    }

    public void WinGame()
    {
        Debug.Log("YOU WIN! Caught all " + fishNeededToWin + " fish!");
        Time.timeScale = 0f;

        if (menuWin != null)
        {
            menuWin.SetActive(true);
            activeMenu = menuWin;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoseGame()
    {
        Debug.Log("GAME OVER!");
        Time.timeScale = 0f;

        if (menuLose != null)
        {
            menuLose.SetActive(true);
            activeMenu = menuLose;

        }

        Cursor.lockState= CursorLockMode.None;
        Cursor.visible = true;
    }

    void UpdateFishUI()
    {
        if (gameGoalCountText != null)
        {
            gameGoalCountText.text = fishCaught + " / " + fishNeededToWin;
        }
    }

    public void ButtonResume()
    {
        UnpauseGame();
    }

    public void ButtonRestart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            );
    }

    public void ButtonQuit()
    {
        Debug.Log("Quitting Game....");
        Application.Quit();
    }

    public bool IsPaused() { return isPaused;  }
    public int GetFishCaught() { return fishCaught; }
    public int GetFishNeeded() { return fishNeededToWin; }
}
