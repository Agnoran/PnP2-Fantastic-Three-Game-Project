using UnityEngine;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask irgnoreLayer;

    // Boat stats
    [SerializeField] int HP;

    [SerializeField] float moveSpeed;
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;
    [SerializeField] float drag;


    int HPOrig;
   
    float moveInput;
    float turnInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        if (rb == null) rb = GetComponent<Rigidbody>();

        rb.linearDamping = drag;
        rb.angularDamping = drag;

        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        

        //Debug.Log(moveInput);

        
    }

    void movement()
    {
        rb.AddForce(transform.forward * moveInput * moveSpeed, ForceMode.Acceleration);

        rb.AddTorque(turnInput * turnSpeed * Vector3.up);
    }
}
