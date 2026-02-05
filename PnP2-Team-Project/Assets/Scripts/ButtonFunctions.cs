using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        WorldController.instance.stateUnpaused();
    }
    public void inventory()
    {
        WorldController.instance.stateInvFromPause();
    }
    public void catalogue()
    {
        WorldController.instance.stateCatFromPause();
    }
    public void quit()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
}
