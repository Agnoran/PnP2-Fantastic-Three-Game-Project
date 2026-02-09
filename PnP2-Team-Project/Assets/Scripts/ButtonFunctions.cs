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
}
