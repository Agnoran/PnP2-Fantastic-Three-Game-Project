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
    public Vector3 topDownOffSet = new Vector3(0f, 12f, 0f);
    public float topDownSmoothSpeed = 3f;

    // Transition
    public float transitionSpeed = 3f;

    //camera mode
    private bool isFishingMode = false;      // toggle
    private bool isTransitioning = false;
    private Vector3 currentVelocity;

    private Vector3 savedPosition;
    private Quaternion savedRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (boat == null)
        {
            GameObject boatObject = GameObject.FindGameObjectWithTag("Player");  // Object>Objects
            if (boatObject != null)
            {
                boat = boatObject.transform;
            }
        }

        if (boat != null)
        {
            Vector3 startPos = boat.position + boat.TransformDirection(followOffset);
            transform.position = startPos;
            transform.LookAt(boat.position + Vector3.up * 1.5f);
        }

        isFishingMode = false;
        isTransitioning = false;


    }

    void LateUpdate()
    {
        if (boat == null)
        {
            return;
        }

        if (isFishingMode)
        {
            // Follow camera mode       // Here is the bug SUPPOSED TO BE IN TOP DOWN MODE
            TopDownCamera();
        }
        else if (isTransitioning)
        {   // should be follow camera mode

            TransitionBackToFollow();       // go back to follow
        }
        else
        {
            FollowBoatCamera();
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

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition,
            ref currentVelocity, 1f / followSmoothSpeed);

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

        Quaternion targetRotation = Quaternion.Euler(90f, boat.eulerAngles.y, 0f);

        // LOOK STRAIGHT DOWN

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * topDownSmoothSpeed);
    }

    void TransitionBackToFollow()
    {
        Vector3 followTarget;
        if (rotateWithBoat)
        {
            followTarget = boat.position + boat.TransformDirection(followOffset);
        }
        else
        {
            followTarget = boat.position + followOffset;
        }

        transform.position = Vector3.Lerp(transform.position, followTarget, Time.deltaTime * transitionSpeed);

        Quaternion lookAtBoat = Quaternion.LookRotation(
            (boat.position + Vector3.up * 1.5f) - transform.position
            );
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtBoat, Time.deltaTime * transitionSpeed);

        float distance = Vector3.Distance(transform.position, followTarget);
            if(distance < 0.5f)
        {
            isTransitioning = false;
            currentVelocity = Vector3.zero;
            Debug.Log("Camera transition complete, back to follow mode");
        }
    }

    // call this to swtich to fishing mode
    public void EnterFishingMode()
    {

        savedPosition = transform.position;
        savedRotation = transform.rotation;

        isFishingMode = true;       // SWITCH ON FISHING MODE
        isTransitioning = false;
        currentVelocity = Vector3.zero;

        Debug.Log("Camera: entering top down fishing mode");
    }
    // call this to return back to follow mode
    public void ExitFishingMode()
    {
       
        isFishingMode = false;          // SWITCH OFF FISHING MODE
        isTransitioning = true;
        currentVelocity = Vector3.zero;
        Debug.Log("Camera: transitioning back to follow mode");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFishingMode = !isFishingMode;
        }

    }
}
