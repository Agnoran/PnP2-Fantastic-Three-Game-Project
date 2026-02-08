using UnityEngine;
using System.Collections;
using System;

public class boatMovement : MonoBehaviour
{
    // component for body

    [SerializeField] Rigidbody rb;

    // Boat stats

    [SerializeField] int HP;

    [SerializeField] float moveSpeed;

    [SerializeField] float maxSpeed;

    [SerializeField] float turnSpeed;

    [SerializeField] float drag;

    [SerializeField] Camera boatCamera;

    [SerializeField] Transform normalCameraPosition;
    [SerializeField] Transform topDownCameraPosition;
    [SerializeField] float cameraTransitionSpeed;


    private int HPOrig;
   
    private float moveInput;
    private float turnInput;
    private bool isFishing = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;

        // Auto-assign Rigidbody if not set

        if (rb == null) 
            rb = GetComponent<Rigidbody>();

        // Configure RigidBody

        rb.linearDamping = drag;
        rb.angularDamping = drag;
        rb.useGravity = false;      // No free falling through the water

        // locks the rotation of the X and the Y  

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Uses the boatsCamera if none, assigned assign

        if (boatCamera == null)
            boatCamera = Camera.main;


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


        // Check for the fishing input, which i think is spacebar from what ben said

        if (Input.GetKeyDown(KeyCode.Space) && !isFishing)
        {
            CheckForFishingSpot();
        }

        // Debug to see what inputs are going through or not

        Debug.Log($"Move: {moveInput}, Turn: {turnInput}, Vel: {rb.linearVelocity.magnitude}, IsKinematic: {rb.isKinematic}");


    }

    void FixedUpdate()
    {
        movement();
        
    }


    void movement()
    {

        Vector3 forceDirection = transform.forward * moveInput * moveSpeed;

        rb.AddForce(forceDirection, ForceMode.Force);

        rb.AddTorque(Vector3.up * turnInput * turnSpeed);

        // Apply forces of torq

        if (Mathf.Abs(moveInput) > 0.1f || rb.linearVelocity.magnitude > 0.5f)
        {
            rb.AddTorque(Vector3.up * turnInput * turnSpeed);
        }

        // Cap the speed max

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        Debug.DrawRay(transform.position, forceDirection.normalized * 5f, Color.green);
       
    }

    void CheckForFishingSpot()
    {
        throw new NotImplementedException();
    }

}
