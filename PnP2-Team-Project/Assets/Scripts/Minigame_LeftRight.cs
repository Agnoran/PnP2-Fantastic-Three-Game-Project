using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Minigame_LeftRight : Fishing
{
    //for instantaition inside Fishing class
    public static Minigame_LeftRight instance;

    //how much progress a successful quicktime hit will inject into the fishing HP
    [SerializeField] int progAdd;
    //how much progress an unsuccessful quicktime hit will inject into the fishing HP
    [SerializeField] int progSub;
    //how much progress to complete the fishing. 0 = lose the fish
    [SerializeField] int progHP;
    //temp srlz field for how much constant HP increase. later this will be driven per-fish
    [SerializeField] float fishProgMod;
    [SerializeField] Image fishingHP;
    float currentHP;
    
    //the spot you're SUPPOSED to hit during the skillcheck
    [SerializeField] Image hitField;

    //the image that slides along the bar for the skillcheck
    [SerializeField] Image slider;
    //how quickly the slider moves back and forth
    [SerializeField] int sliderTravelSpeed;
    //original slider color 
    Color sliderColorOrig;
    //original slider position
    Vector2 sliderPos;

    //the image that frames the skillcheck minigame. the slider will travel along it
    [SerializeField] Image sliderBar;
    [SerializeField] GameObject targetLeft;
    [SerializeField] GameObject targetRight;
    //targets for the slider movement
    Vector2 v2TargetLeft;
    Vector2 v2TargetRight;
    //this changes to be the left or right target
    Vector2 v2Target;



    void Start()
    {
        

        //set slider components
        sliderPos = slider.rectTransform.position;
        
        //set targets
        v2TargetLeft = targetLeft.transform.position;
        v2TargetRight = targetRight.transform.position;
        v2Target = v2TargetRight;
        

        sliderColorOrig = slider.color;

        //fish starts halfway up for now
        currentHP = progHP / 2;
        updateFishProgress();
    }


    // Update is called once per frame
    void Update()
    {
        //move slider back and forth
        moveSlider();

        //check input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hitBehavior();
        }

        //adjust progress bar
        updateFishProgress();
    }

    void moveSlider()
    {
        //move
        sliderPos = Vector2.MoveTowards(sliderPos, v2Target, sliderTravelSpeed * Time.deltaTime);
        slider.transform.position = sliderPos;


        // Change direction if the pointer reaches one of the points
        if (Vector2.Distance(sliderPos, v2TargetRight) < 0.1f)
        {
            v2Target = v2TargetLeft;
        }
        else if (Vector2.Distance(sliderPos, v2TargetLeft) < 0.1f)
        {
            v2Target = v2TargetRight;
        }


        //add base progress
        currentHP += fishProgMod;
    }

    //logic for a passing/failing quicktime press
    void hitBehavior()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(hitField.rectTransform, sliderPos))
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

        currentHP += progAdd;
        updateFishProgress();
        StartCoroutine(flashGood());

    }

    //on unsuccessful hit
    void subProgress()
    {
        currentHP -= progSub;
        updateFishProgress();
        StartCoroutine(flashBad());

    }


    IEnumerator flashGood()
    {
        slider.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        slider.color = sliderColorOrig;
    }
    IEnumerator flashBad()
    {
        slider.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        slider.color = sliderColorOrig;
    }

}
