using UnityEngine;

public class FishAndHook : MonoBehaviour
{
    public static FishAndHook instance;

    [SerializeField] UnityEngine.UI.Image fish;
    [SerializeField] UnityEngine.UI.Image hook;


    private void Awake()
    {
        instance = this;
    }



}
