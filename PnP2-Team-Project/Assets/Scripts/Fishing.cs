using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    [SerializeField] GameObject minigame_LeftRight;
    [SerializeField] GameObject minigame_Circle;
    [SerializeField] GameObject minigame_TugOfWar;

    //ScriptableObject fish;
    //int whichGame;

    public GameObject hitFieldCube;
    public GameObject sliderCube;
    public Image fishingHP;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get public stuff
        hitFieldCube = GameObject.FindWithTag("");
        sliderCube = GameObject.FindWithTag("");


        //get which kind of fish
        // fish = fishing pool fish 

        //decide which minigame to run
        Instantiate(minigame_LeftRight);
    }


}