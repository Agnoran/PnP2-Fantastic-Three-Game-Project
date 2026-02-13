using System;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;




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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get offset
        offsetPos = new Vector2(rotationCenter.transform.position.x, rotationCenter.transform.position.y + offsetFromCenterY);

        //place slider bar
        sliderBar.transform.position = offsetPos;


        


        

        //distribute hitfields

        //GameObject skillCheckBox = Instantiate(hitField, r);
        //Object original,
        //Vector3 position,
        //Quaternion rotation


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
