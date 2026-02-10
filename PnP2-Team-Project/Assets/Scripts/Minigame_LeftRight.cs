using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Minigame_LeftRight : Fishing
{
    //for instantaition inside Fishing class
    public static Minigame_LeftRight instance;

    //slider renderer for hit feedback
    [SerializeField] Renderer sliderRenderer;

    //how quickly the slider moves back and forth
    [SerializeField] int sliderTravelSpeed;

    //how much progress a successful quicktime hit will inject into the fishing HP
    [SerializeField] int progAdd;
    //how much progress an unsuccessful quicktime hit will inject into the fishing HP
    [SerializeField] int progSub;
    //how much progress to complete the fishing. 0 = lose the fish
    [SerializeField] int progHP;

    //temp srlz field for how much constant HP increase. later this will be driven per-fish
    [SerializeField] int fishProgMod;

    [SerializeField] GameObject hitFieldCube;
    [SerializeField] GameObject sliderCube;

    [SerializeField] Image fishingHP;

    float currentHP;
    
    bool sliderInTrigger;
    Color sliderOriginal;
    Vector3 sliderDir;
    Vector3 sliCubePos;

    void Start()
    {

        sliCubePos = sliderCube.transform.position;
        sliderDir = Vector3.right;

        //fish starts halfway up for now
        currentHP = progHP / 2;
        updateFishProgress();
        sliderOriginal = Color.white;
    }


    // Update is called once per frame
    void Update()
    {

        //move slider back and forth
        moveSlider();

        //check input
        if (Input.GetButtonDown("Jump"))
        {
            hitBehavior();
        }

        //adjust progress bar
        updateFishProgress();


    }

    void moveSlider()
    {
        //move
        sliCubePos = (sliderDir * sliderTravelSpeed * Time.deltaTime);


        //check left/right
        if (sliderCube.transform.position.x <= -50)
        {
            sliderDir = Vector3.right;
        }
        else if (sliderCube.transform.position.x >= 50)
        {
            sliderDir = Vector3.left;
        }


        //add base progress
        currentHP += fishProgMod;
    }

    void hitBehavior()
    {
        if (sliderInTrigger)
        {
            addProgress();
        }
        else
        {
            subProgress();
        }
    }

    //updates progress bar and checks for game ending conditions
    void updateFishProgress()
    {
        fishingHP.fillAmount = (float)currentHP / progHP;

        //check loss/win
        if (currentHP <= 0)
        {
            WorldController.instance.ResolveFishingAttempt(false);
        }
        else if (currentHP >= progHP)
        {
            WorldController.instance.ResolveFishingAttempt(true);
        }

    }


    //on successful hit
    void addProgress()
    {

        progHP += progAdd;
        updateFishProgress();
        StartCoroutine(flashGood());

    }

    //on unsuccessful hit
    void subProgress()
    {
        progHP -= progSub;
        updateFishProgress();
        StartCoroutine(flashBad());

    }


    IEnumerator flashGood()
    {
        sliderRenderer.material.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        sliderRenderer.material.color = sliderOriginal;
    }
    IEnumerator flashBad()
    {
        sliderRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sliderRenderer.material.color = sliderOriginal;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("sliderCube_LeftRight"))
        {
            sliderInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("sliderCube_LeftRight"))
        {
            sliderInTrigger = false;
        }
    }

}
