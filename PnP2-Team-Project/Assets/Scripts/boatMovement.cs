using UnityEngine;
using System.Collections;
using System;

public class boatMovement : MonoBehaviour
{
    // component for body

    [SerializeField] Rigidbody rb;

    // fishing aspect to the boat

    bool isFishing = false;
    bool isInFishingZone = false;

    GameObject currentPool = null;

    // Boat stats

    [SerializeField] int HP = 10;

    [SerializeField] float moveSpeed = 15f;

    [SerializeField] float maxSpeed = 10f;

    [SerializeField] float turnSpeed = 50f;

    [SerializeField] float waterDrag = 3.5f;

    [SerializeField] float sidewaysDragMultiplier = 5f;

    // Camera 

    [SerializeField] boatCamera cameraScript;
    [SerializeField] GameObject fishingPromptUI;

    [SerializeField] Transform normalCameraPosition;
    [SerializeField] Transform topDownCameraPosition;
    [SerializeField] float cameraTransitionSpeed;


    private int HPOrig;
   
    private float moveInput;
    private float turnInput;

    //private bool isFishing = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;

        // Auto-assign Rigidbody if not set

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        // Configure RigidBody

        rb.linearDamping = waterDrag;
        rb.angularDamping = waterDrag;
        rb.useGravity = false;      // No free falling through the water

        // locks the rotation of the X and the Y  

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Uses the boatsCamera if none, assigned assign

        if (cameraScript == null)
            cameraScript = FindAnyObjectByType<boatCamera>();

        if (fishingPromptUI != null)
            fishingPromptUI.SetActive(false);
    }


    //cameraScript = Camera.main;


    // Update is called once per frame
    void Update()
    {
        if (isFishing)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space))
            {
                StopFishing();

            }

            return;
        }



        // Get the inputs to move Just forward, and turning side to side 

        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        // Fallback to key inputs

        if (Input.GetKey(KeyCode.W)) moveInput = 1f;
        if (Input.GetKey(KeyCode.S)) moveInput = -1f;
        if (Input.GetKey(KeyCode.A)) turnInput = -1f;
        if (Input.GetKey(KeyCode.D)) turnInput = 1f;


        // Check for the fishing input, which i think is spacebar from what ben said

        if (Input.GetKeyDown(KeyCode.Space) && isInFishingZone)
        {
            StartFishing();
        }

        // Debug to see what inputs are going through or not

        Debug.Log($"Move: {moveInput}, Turn: {turnInput}, Vel: {rb.linearVelocity.magnitude}, IsKinematic: {rb.isKinematic}");


    }

    void FixedUpdate()
    {
        //if (isFishing)
        //    return;

        movement();
        
    }


    void movement()
    {
        if (WorldController.instance.IsMenuOpen())
        {
            return;
        }

       // apply forward,backward force
       Vector3 forceDirection = transform.forward * moveInput * moveSpeed;
        rb.AddForce(forceDirection, ForceMode.Force);
        // add turning force
        rb.AddTorque(Vector3.up * turnInput * turnSpeed, ForceMode.Force);

        // attempting to add some kind of buoyancy 
        Vector3 sidewaysVelocity = Vector3.Dot(rb.linearVelocity, transform.right) * transform.right;
        rb.AddForce(-sidewaysVelocity * sidewaysDragMultiplier, ForceMode.Acceleration);


        // Cap the speed max
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

       //Debug.DrawRay(transform.position, forceDirection.normalized * 5f, Color.green);
       //Debug.DrawRay(transform.position, rb.linearVelocity.normalized * 3f, Color.red);

    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("FishingSpot"))
    //    {
    //        isInFishingZone = true;
    //        currentPool = other.gameObject;
    //        Debug.Log("Entered Fishing Zone: " + other.gameObject.name);

    //        if (fishingPromptUI != null)
    //        {
    //            fishingPromptUI.SetActive(true);
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("FishingSpot"))
    //    {
    //        isInFishingZone = false;
    //        currentPool = null;
    //        Debug.Log("Left Fishing Zone: ");

    //        if (fishingPromptUI != null)
    //        {
    //            fishingPromptUI.SetActive(false);
    //        }

    //    }
    //}

    void StartFishing()
    {
        isFishing = true;

        //rb.linearVelocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;

        if (cameraScript != null) 
            cameraScript.EnterFishingMode();

            //if (fishingPromptUI != null)
            //    fishingPromptUI.SetActive(false);

            //Debug.Log("Started fishing at: " + currentPool?.name);

    }

    void StopFishing()
    {
        isFishing = false;

        if (cameraScript != null)
            cameraScript.ExitFishingMode();

        isInFishingZone = false;

        //Debug.Log("Stopped fishing, back to movement");


    }

    

            

    void CheckForFishingSpot()
    {
        Debug.Log("Checking for the fishing spot: ");
    }



}
