using System.Collections;
using UnityEngine;


public class Minigame_Circular : MonoBehaviour
{
    public static Minigame_Circular instance;

    [Header("objects")]
    [SerializeField] UnityEngine.UI.Image circleFrame;
    [SerializeField] UnityEngine.UI.Image hitField;
    [SerializeField] UnityEngine.UI.Image slider;
    UnityEngine.UI.Image sliderBar;
    Color sliderColorOrig = Color.white;
    bool inHitField;


    [Header("game layout numbers")]
    [Tooltip("how quickly the slider will rotate around the circle BEFORE difficulty modifier")]
    [SerializeField] float gameSpeed;
    [Tooltip("radius of minigame circle - this affects slider + hitfields placement")]
    [SerializeField] int offsetFromCenterY;
    Vector2 offsetPos;
    Vector3 rotationAxis = new Vector3(0, 0, 1);
    Vector2 rotationCenter;

    //this will be calculated per-fish in the fishing() script
    float difficultyMod;
    float angle;



    //AWAKE START UPDATE - - - - - - - - -v//
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        //infoTimer = 0;
        inHitField = false;

        difficultyMod = Fishing.instance.calcDifficulty();
        angle = gameSpeed * difficultyMod / 3;

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
        rotationCenter = Fishing.instance.minigamePosition.transform.position;

        //get offset (radius of circle)
        offsetPos = new Vector2(rotationCenter.x, rotationCenter.y + offsetFromCenterY);

        //instantiate circle
        UnityEngine.UI.Image circle = Instantiate(circleFrame, Fishing.instance.minigamePosition.transform);
        circle.rectTransform.sizeDelta = new Vector2(offsetFromCenterY*2.3f, offsetFromCenterY*2.3f);

        //instantiate hitfields + place along the circle
        int ranLayoutOffset = Random.Range(0, 2);
        for (int i = 0; i < 4; i++)
        {
            //create a hitfield
            UnityEngine.UI.Image tempHitField = Instantiate(hitField, Fishing.instance.minigamePosition.transform);
            //adjust width based on difficulty
            Vector2 adjustVec = new Vector2(240 - (20 * difficultyMod), 140);
            tempHitField.rectTransform.sizeDelta = adjustVec;
            tempHitField.GetComponent<BoxCollider2D>().size = adjustVec;
            //place it on the circle
            tempHitField.transform.position = offsetPos;
            //rotate it to distribute around the circle
            tempHitField.transform.RotateAround(rotationCenter, rotationAxis, i * 90 - (45 * ranLayoutOffset));
        }

        //place slider bar on the circle
        sliderBar = Instantiate(slider, Fishing.instance.minigamePosition.transform);
        sliderBar.transform.position = offsetPos;

    }

    void moveSlider()
    {
        //infoTimer += Time.deltaTime;

        //rotate slider around the center
        sliderBar.transform.RotateAround(rotationCenter, rotationAxis, angle * Time.deltaTime);

        //check input
        if (Input.GetButtonDown("Attempt"))
        {
            onAttempt();
        }
    }

    public void toggleBool(bool b)
    {
        inHitField = b;
    }

    void onAttempt()
    {
        
        if (inHitField)
        {
            Debug.Log("Hit");
            Fishing.instance.addProgress();
            StartCoroutine(flashGood());
        }
        else
        {
            Debug.Log("Miss");
            Fishing.instance.subProgress();
            StartCoroutine(flashBad());
        }

    }

    IEnumerator flashGood()
    {
        sliderBar.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        sliderBar.color = sliderColorOrig;
    }
    IEnumerator flashBad()
    {
        sliderBar.color = Color.darkRed;
        yield return new WaitForSeconds(0.1f);
        sliderBar.color = sliderColorOrig;
    }


}
