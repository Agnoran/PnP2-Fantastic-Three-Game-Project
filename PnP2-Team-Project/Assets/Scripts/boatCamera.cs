using UnityEngine;

public class boatCamera : MonoBehaviour
{
    //making the actual boat
    public Transform boat;

    //follow camera settings
    public Vector3 followOffset = new Vector3(0f, 4f, -7f);
    public float followSmoothSpeed = 5f;
    public bool rotateWithBoat = true;

    //topdown settings
    public Vector3 topDownOffSet = new Vector3(0f, 10f, 0f);
    public float topDownSmoothSpeed = 3f;

    //camera mode
    public bool isFishingMode = false;      // toggle

    private Vector3 currentVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (boat == null)
        {
            GameObject boatObject = GameObject.FindGameObjectWithTag("Player");  // Object>Objects
            if (boatObject != null)
            {
                boat = boatObject.GetComponent<Transform>();
            }
        }


    }

    void LateUpdate()
    {
        if (boat == null)
        {
            return;
        }

        if (!isFishingMode)
        {
            // Follow camera mode
            FollowBoatCamera();
        }
        else
        {   // top down
            TopDownCamera();
        }

    }

    void FollowBoatCamera()
    {
        Vector3 desiredPosition;

        if (rotateWithBoat)
        {
            // this offsets with the boats position
            desiredPosition = boat.position + boat.TransformDirection(followOffset);
        }
        else
        {
            // offset to not rotate i may have to use the worldManager well see 
            desiredPosition = boat.position + followOffset;
        }

        // make the camera move to the correct position

        transform.LookAt(boat.position + Vector3.up * 1.5f);
    }

    void TopDownCamera()
    {
        // position camera directly above the boat

        Vector3 desiredPosition = boat.position + topDownOffSet;

        // smoothly move the camera

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition,
            ref currentVelocity, 1f / topDownSmoothSpeed);

        // LOOK STRAIGHT DOWN

        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
    // call this to swtich to fishing mode
    public void EnterFishingMode()
    {
        isFishingMode = true;       // SWITCH ON FISHING MODE
    }
    // call this to return back to follow mode
    public void ExitFishingMode()
    {
        isFishingMode = false;          // SWITCH OFF FISHING MODE
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isFishingMode = !isFishingMode;
        }

    }
}
