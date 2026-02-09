using UnityEngine;

/*
 * TEMPORARY / PROTOTYPE CODE
 * -------------------------
 * This script was generated with AI assistance and exists ONLY to support
 * early integration testing (movement, triggers, and basic flow).
 *
 * This is NOT intended to be used in the final project.
 * This code is expected to be replaced, refactored, or deleted.
 *
 * Do not build gameplay systems, tuning, or architecture on top of this.
 * Final player control logic will be authored by the assigned developer.
 * 
 * * If you are reading this in the future and it's still here — something went wrong
 */

[RequireComponent(typeof(Rigidbody))]
public class TempPlayerControl : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float turnSpeedDegreesPerSecond = 180f;

    [Header("Setup")]
    [Tooltip("If true, forces the rigidbody to stay on the XZ plane (Y = 0).")]
    [SerializeField] bool lockToPlane = true;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Simple "boat on a plane" defaults
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (lockToPlane)
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionY;
        }
    }

    void FixedUpdate()
    {
        // WASD/Arrow keys via Unity's default "Horizontal" and "Vertical" axes
        float forwardInput = Input.GetAxisRaw("Vertical");   // W/S or Up/Down
        float turnInput = Input.GetAxisRaw("Horizontal");    // A/D or Left/Right

        // Rotate
        if (Mathf.Abs(turnInput) > 0f)
        {
            float turnDelta = turnInput * turnSpeedDegreesPerSecond * Time.fixedDeltaTime;
            Quaternion turn = Quaternion.Euler(0f, turnDelta, 0f);
            rb.MoveRotation(rb.rotation * turn);
        }

        // Move forward/back along the facing direction
        Vector3 forwardMove = transform.forward * (forwardInput * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + forwardMove);
    }
}