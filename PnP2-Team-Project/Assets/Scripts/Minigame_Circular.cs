using UnityEngine;


public class Minigame_Circular : MonoBehaviour
{
    [Header("objects")]
    [SerializeField] UnityEngine.UI.Image rotationCenter;
    [SerializeField] UnityEngine.UI.Image sliderBar;
    [SerializeField] UnityEngine.UI.Image hitField;

    [SerializeField] Vector3 sliderRotAxis;

    [Tooltip("how far out from the center of the circle should the slider + hitfields be ?")]
    [SerializeField] int offsetFromCenterY;
    Vector2 offsetPos;

    //hitfield placement stuff
    //UnityEngine.UI.Image field1;
    //UnityEngine.UI.Image field2;
    //Vector3 hFieldPos;
    //Quaternion hFieldRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get offset
        offsetPos = new Vector2(rotationCenter.transform.position.x, rotationCenter.transform.position.y + offsetFromCenterY);

        //place slider bar
        sliderBar.transform.position = offsetPos;

        //instantiate hitfields + place along the circle
        int randomLayout = Random.Range(0, 1);

        for (int i = 0; i < 4; i++)
        {
            UnityEngine.UI.Image tempHitField = Instantiate(hitField, rotationCenter.transform);
            tempHitField.transform.position = offsetPos;
            tempHitField.transform.RotateAround(rotationCenter.transform.position, sliderRotAxis, i * 90 -(45*randomLayout));

        }

    }

    // Update is called once per frame
    void Update()
    {
        //rotate slider around the center
        sliderBar.transform.RotateAround(rotationCenter.transform.position, sliderRotAxis, 360 * Time.deltaTime);

        //check for input


        //update progress

    }



    // sliderBar.transform.RotateAround(rotationCenter.transform.position, sliderRotAxis, 360 * Time.deltaTime);



    //public Vector3 angularVelocity;
    //public Space space = Space.Self;
    //transform.Rotate(angularVelocity * Time.deltaTime, space);

}
