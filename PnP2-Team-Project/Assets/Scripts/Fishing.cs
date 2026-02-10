using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    [SerializeField] GameObject minigame_LeftRight;
    [SerializeField] GameObject minigame_Circle;
    [SerializeField] GameObject minigame_TugOfWar;

    //ScriptableObject fish;
    
    //int whichGame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        //get which kind of fish
        // fish = fishing pool fish 


        //decide which minigame to run
        Instantiate(minigame_LeftRight);
    }


}