using Unity.VisualScripting;
using UnityEngine;


public class Minigame_Loops : MonoBehaviour
{
    [Header("pivot points")]
    [SerializeField] UnityEngine.UI.Image middle;
    [SerializeField] protected GameObject leftCenter;
    [SerializeField] protected GameObject rightCenter;
    protected GameObject targetCenter;
    Vector2 leftOffset;
    Vector2 rightOffset;
    Vector3 sliderRotAxis;

    [Header("play objects")]
    [SerializeField] UnityEngine.UI.Image sliderBar;
    UnityEngine.UI.Image slider;
    [SerializeField] UnityEngine.UI.Image hitField;
    [Tooltip("how far out from the center of the circle should the slider + hitfields be ?")]
    [SerializeField] int offsetFromCenterY;

    

    //private float rotTimer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        //get offset to place slider
        leftOffset = new Vector2(leftCenter.transform.position.x, leftCenter.transform.position.y + offsetFromCenterY);
        rightOffset = new Vector2(rightCenter.transform.position.x, rightCenter.transform.position.y + offsetFromCenterY);
        sliderRotAxis = new Vector3(0, 0, 1);

        

        //instantiate hitfields + place along the circle
        int layoutOffset = Random.Range(0, 1);
        setHitfields(leftCenter, leftOffset, layoutOffset);
        setHitfields(rightCenter, rightOffset, layoutOffset);

        //instantiate slider bar + place
        slider = Instantiate(sliderBar, leftCenter.transform);
        setSlider(leftCenter, leftOffset);

        targetCenter = leftCenter;
    }

    // Update is called once per frame
    void Update()
    {
        //rotate slider around the layout
        slider.transform.RotateAround(targetCenter.transform.position, sliderRotAxis, 360 * Time.deltaTime);


        //check for input


        //update progress

    }


    void setHitfields(GameObject rotationCenter, Vector2 offsetFromCenterLR, int randomOffset)
    {
        for (int i = 0; i < 4; i++)
        {
            UnityEngine.UI.Image tempHitField = Instantiate(hitField, rotationCenter.transform);
            tempHitField.transform.position = offsetFromCenterLR;
            tempHitField.transform.RotateAround(rotationCenter.transform.position, sliderRotAxis, i * 90 - (30 * randomOffset));
        }

    }

    void setSlider(GameObject rotationCenter, Vector2 offsetFromCenter)
    {
        slider.transform.SetParent(rotationCenter.transform.parent, false);
        slider.transform.position = offsetFromCenter;
        slider.transform.RotateAround(rotationCenter.transform.position, sliderRotAxis, 90 + (45 * (Random.Range(-1, 5))));

    }














}




