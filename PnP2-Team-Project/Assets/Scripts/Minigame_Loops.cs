using Unity.VisualScripting;
using UnityEngine;


public class Minigame_Loops : MonoBehaviour
{
    [Header("objects")]
    [SerializeField] GameObject leftCenter;
    [SerializeField] GameObject rightCenter;
    [SerializeField] UnityEngine.UI.Image sliderBar;
    [SerializeField] UnityEngine.UI.Image hitField;

    [SerializeField] Vector3 sliderRotAxis;

    [Tooltip("how far out from the center of the circle should the slider + hitfields be ?")]
    [SerializeField] int offsetFromCenterY;

    GameObject targetCenter;
    Vector2 leftOffset;
    Vector2 rightOffset;

    float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get offset to place slider
        leftOffset = new Vector2(leftCenter.transform.position.x, leftCenter.transform.position.y + offsetFromCenterY);
        rightOffset = new Vector2(rightCenter.transform.position.x, rightCenter.transform.position.y + offsetFromCenterY);

        //place slider bar
        sliderBar.transform.position = leftOffset;

        //instantiate hitfields + place along the circle
        int layoutOffset = Random.Range(0, 1);
        setLayout(leftCenter, leftOffset, layoutOffset);
        setLayout(rightCenter, rightOffset, layoutOffset);

        targetCenter = leftCenter;
    }

    // Update is called once per frame
    void Update()
    {
        //rotate slider around the layout

        moveLoops(timer);

        //check for input


        //update progress

    }


    void setLayout(GameObject rotationCenter, Vector2 offsetFromCenterLR, int randomOffset)
    {
        for (int i = 0; i < 4; i++)
        {
            UnityEngine.UI.Image tempHitField = Instantiate(hitField, rotationCenter.transform);
            tempHitField.transform.position = offsetFromCenterLR;
            tempHitField.transform.RotateAround(rotationCenter.transform.position, sliderRotAxis, i * 90 - (30 * randomOffset));
        }

    }

    void moveLoops(float timer)
    {
        //move the slider
        sliderBar.transform.RotateAround(targetCenter.transform.position, sliderRotAxis, 360 * Time.deltaTime);

        //increment timer
        timer++;
        
        //check to swap rotation centers
        if (timer >= 360)
        {
            if (targetCenter.transform.position == leftCenter.transform.position)
            {
                targetCenter = rightCenter;
            }
            else
            {
                targetCenter = leftCenter;
            }
                timer = 0;
        }

    }

}




