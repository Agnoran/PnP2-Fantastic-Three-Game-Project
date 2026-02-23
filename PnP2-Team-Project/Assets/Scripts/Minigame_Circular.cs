using System.Collections;
using UnityEngine;


public class Minigame_Circular : MonoBehaviour
{
    public static Minigame_Circular instance;

    [Header("objects")]
    [SerializeField] UnityEngine.UI.Image rotationCenter;
    [SerializeField] UnityEngine.UI.Image slider;
    [SerializeField] UnityEngine.UI.Image hitField;
    UnityEngine.UI.Image sliderBar;
    Color sliderColorOrig = Color.white;
    bool inHitField;

    [Header("numbers")]
    [Tooltip("how quickly the slider will rotate around the circle BEFORE difficulty modifier")]
    [SerializeField] float gameSpeed;
    [Tooltip("radius of minigame circle - this affects slider + hitfields placement")]
    [SerializeField] int offsetFromCenterY;
    Vector2 offsetPos;
    Vector3 rotationAxis = new Vector3(0, 0, 1);

    //this will be calculated per-fish in the fishing() script
    float difficultyMod;
    float angle;

    //AWAKE START UPDATE - - - - - - - - -v//
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    void Start()
    {
        inHitField = false;

        difficultyMod = Fishing.instance.calcDifficulty();
        angle = gameSpeed * difficultyMod / 2;

        layoutGame();

    }

    void Update()
    {
        //freeze on pause
        if (Time.timeScale == 0) { return; }

        //move slider back and forth
        moveSlider();

    }


    //OTHER METHODS - - - - - - - -v//
    void layoutGame()
    {
        //move minigame to the spot in the canvas
        rotationCenter.transform.position = Fishing.instance.minigamePosition.transform.position;

        //get offset (radius of circle)
        offsetPos = new Vector2(rotationCenter.transform.position.x, rotationCenter.transform.position.y + offsetFromCenterY);

        //instantiate hitfields + place along the circle
        int ranLayoutOffset = Random.Range(0, 2);
        for (int i = 0; i < 4; i++)
        {
            //create a hitfield
            UnityEngine.UI.Image tempHitField = Instantiate(hitField, rotationCenter.transform);
            //adjust width based on difficulty
            Vector2 adjustVec = new Vector2(160 - (16 * difficultyMod), 100);
            tempHitField.rectTransform.sizeDelta = adjustVec;
            tempHitField.GetComponent<BoxCollider2D>().size = adjustVec;
            //place it on the circle
            tempHitField.transform.position = offsetPos;
            //rotate it to distribute around the circle
            tempHitField.transform.RotateAround(rotationCenter.transform.position, rotationAxis, i * 90 - (45 * ranLayoutOffset));
        }

        //place slider bar on the circle
        sliderBar = Instantiate(slider, rotationCenter.transform);
        sliderBar.transform.position = offsetPos;

        //place the fish at the start and update the UI
        //currentHP = progHP / 2;
        //updateFishProgress();
    }

    void moveSlider()
    {
        //rotate slider around the center
        sliderBar.transform.RotateAround(rotationCenter.transform.position, rotationAxis, angle * Time.deltaTime);

        //check input
        if (Input.GetButtonDown("Attempt"))
        {
            onHit();
        }


        //add base progress
        //currentHP += fishProgMod;
    }

    public void toggleBool(bool b)
    {
        inHitField = b;
    }

    void onHit()
    {
        
        if (inHitField)
        {

            addProgress();
            
        }
        else
        {
            subProgress();
            
        }

    }





    //on successful hit
    void addProgress()
    {
        Debug.Log("Hit");
        //currentHP += progAdd;
        updateFishProgress();
        StartCoroutine(flashGood());

    }

    //on unsuccessful hit
    void subProgress()
    {
        Debug.Log("Miss");
        //currentHP -= progSub;
        updateFishProgress();
        StartCoroutine(flashBad());

    }

    IEnumerator flashGood()
    {
        sliderBar.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        sliderBar.color = sliderColorOrig;
    }
    IEnumerator flashBad()
    {
        slider.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        slider.color = sliderColorOrig;
    }


    //updates progress bar and checks for game ending conditions
    void updateFishProgress()
    {

        //fishingHP.fillAmount = (float)currentHP / progHP;

        ////check loss/win
        //if (currentHP <= 0)
        //{
        //    WorldController.instance.ResolveFishingAttempt(false);
        //    Destroy(gameObject);

        //}
        //else if (currentHP >= progHP)
        //{
        //    WorldController.instance.ResolveFishingAttempt(true);
        //    Destroy(gameObject);

        //}


    }
}
