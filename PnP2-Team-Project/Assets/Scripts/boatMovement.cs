using UnityEngine;
using System.Collections;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class boatMovement : MonoBehaviour
{
    // component for body

    [SerializeField] Rigidbody rb;

    // fishing aspect to the boat

    bool isFishing = false;
    bool isInFishingZone = false;

    GameObject currentPool = null;

    // Boat stats
    [Header("Boat Stats")]

    [SerializeField] int HP;

    [SerializeField] float moveSpeed;

    [SerializeField] float maxSpeed;

    [SerializeField] float turnSpeed;

    [SerializeField] float waterDrag;

    [SerializeField] float sidewaysDragMultiplier;

    // Camera 

    [SerializeField] boatCamera cameraScript;
    [SerializeField] GameObject fishingPromptUI;

    [SerializeField] Transform normalCameraPosition;
    [SerializeField] Transform topDownCameraPosition;
    [SerializeField] float cameraTransitionSpeed;


    public int HPOrig;
    public float moveInput;
    public float turnInput;

    // Fishing rod stats



    [Header("Fishing Rod")]
    [SerializeField] List<rodStats> rodList = new List<rodStats>();
    [SerializeField] GameObject rodModel;
    [SerializeField] int lineDamageOnSnap;



    // Im gonna need the array for the baitList
    [Header("Baits")]
    [SerializeField] List<baitList> baitInventory = new List<baitList>();


    


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


    


    // Update is called once per frame
    void Update()
    {

        // Get the inputs to move Just forward, and turning side to side 

        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        // Fallback to key inputs

        if (Input.GetKey(KeyCode.W)) moveInput = 1f;
        if (Input.GetKey(KeyCode.S)) moveInput = -1f;
        if (Input.GetKey(KeyCode.A)) turnInput = -1f;
        if (Input.GetKey(KeyCode.D)) turnInput = 1f;



        // Debug to see what inputs are going through or not

        Debug.Log($"Move: {moveInput}, Turn: {turnInput}, Vel: {rb.linearVelocity.magnitude}, IsKinematic: {rb.isKinematic}");


    }

    void FixedUpdate()
    {
       

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

   
    //public List<rodStats> GetRodList() { return rodList; }

}
