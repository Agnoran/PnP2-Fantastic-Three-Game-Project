using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Minigame_LeftRight : Fishing
{
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

    float currentHP;

    GameObject sliderCube;
    bool sliderInTrigger;
    Color sliderOriginal;
    float sliderMoveTimer;

    

    void Start()
    {
        sliderMoveTimer = 0;
        sliderCube = GameObject.FindWithTag("sliderCube_LeftRight");
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
        if (Input.GetButtonDown("Hit"))
        {
            hitBehavior();
        }

        //adjust progress bar
        updateFishProgress();

        
    }

    void moveSlider()
    {
        //slider moves back and forth
        if (sliderMoveTimer <= 100)
        {
            sliderCube.transform.position = Vector3.right * sliderTravelSpeed * Time.deltaTime;
            sliderMoveTimer += Time.deltaTime;
        }
        else
        {
            sliderCube.transform.position = Vector3.left * sliderTravelSpeed * Time.deltaTime;
            sliderMoveTimer -= Time.deltaTime;
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
