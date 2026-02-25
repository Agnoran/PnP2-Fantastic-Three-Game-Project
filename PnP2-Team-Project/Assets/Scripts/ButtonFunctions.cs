using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        WorldController.instance.StateUnpaused();
    }
    public void inventory()
    {
        WorldController.instance.StateInvFromPause();
    }
    public void catalogue()
    {
        WorldController.instance.StateCatFromPause();
    }
    public void quit()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        WorldController.instance.StateUnpaused();
    }


    // Temporary UI Button Code
    public void FishCaught()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.ResolveFishingAttempt(true);
    }
    public void FishLost()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.ResolveFishingAttempt(false);
    }
    public void FishingContinue()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.FinishFishingResult();
    }
    public void CutTheLine()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.CutTheLine();
    }

    public void CloseInventory()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateCloseInventory();
    }
    public void CloseCatalogue()
    {
        if(WorldController.instance == null) return;
        WorldController.instance.StateCloseCatalogue();
    }

    // Shop Buttons
    public void OpenGlobalShop()
    {
        if(WorldController.instance == null) return;
        if (WorldController.instance.IsMenuOpen()) return;
        if (WorldController.instance.GlobalShopUnlocked && WorldController.instance.GlobalShop != null)
        {
            ShopUI.instance.Open(WorldController.instance.GlobalShop);
        }
    }
    public void OpenLocalShop()
    {
        if(WorldController.instance == null) return;
        if (WorldController.instance.IsMenuOpen()) return;
        if (WorldController.instance.ActiveLocalShop == null) { return; }


        ShopUI.instance.Open(WorldController.instance.ActiveLocalShop);
    }
    public void CloseShop()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateCloseShop();
    }
    public void SellAllFish()
    {
        if (ShopUI.instance == null)
            return;

        ShopUI.instance.SellAllFish();
    }
    public void StartGame()
    {
        if(WorldController.instance == null) return;
        WorldController.instance.StateStartGame();
    }
    public void BeginTutorial()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateTutorialOne();
    }
    public void ContinueTutorial()
    {
        if (WorldController.instance == null) return;
        WorldController.instance.StateTutorialTwo();
    }
}
