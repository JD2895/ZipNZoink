using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // Constants
    private const float distanceToBottom = 0.5f;
    private const float bottomCheckBuffer = 0.05f;
    private const float sideCheckBuffer = 0.2f;
    private const float inputDeadZone = 0.1f;

    // Check bools
    private bool groundBelowCheck;

    // Gravity
    public float gravity;
    private bool applyGravity = true;
    private float gravityToApply;

    // Movement
    public float maxSpeed;
    public float cutOffSpeed;
    public float timeToMaxSpeed;
    public float timeToNoSpeed;
    private float accelPerSec;
    private float decelPerSec;
    private float hInputToAppliy;

    // Helpers
    private Rigidbody2D rb;
    private Vector3 directionToMove;
    private Vector3 newPosition;

    void Awake()
    {
        directionToMove = Vector3.zero;
        rb = this.GetComponent<Rigidbody2D>();

        // Initial setup
        gravityToApply = 0f;
        hInputToAppliy = 0f;
        accelPerSec = maxSpeed / timeToMaxSpeed;
        decelPerSec = -maxSpeed / timeToNoSpeed;
    }

    private void FixedUpdate()
    {
        // Setup
        directionToMove = Vector3.zero;
        newPosition = transform.position;
        groundBelowCheck = CheckGround();

        // Check to reset directionToMove
        if (groundBelowCheck)
        {
            gravityToApply = 0f;
        }
        
        // Gravity
        applyGravity = !groundBelowCheck;
        if (applyGravity)
        {
            gravityToApply -= gravity;
        }

        // Check input
        float curHorInput = Input.GetAxis("Horizontal");
        bool fireHook = Input.GetButton("Right Hook Fire");
        //bool fireHook = Input.GetButtonDown("Right Hook Fire");
        float reelHook = Input.GetAxis("Right Hook Reel");

        // Apply horiztonal input
        if (Mathf.Abs(curHorInput) > inputDeadZone)
        {
            if (Mathf.Sign(hInputToAppliy) != Mathf.Sign(curHorInput))
                hInputToAppliy *= -1f;  // Immediately change direction but maintain the previous speed
            hInputToAppliy += curHorInput * accelPerSec * Time.deltaTime;
            if (Mathf.Abs(hInputToAppliy) > maxSpeed)
                hInputToAppliy = Mathf.Sign(hInputToAppliy) * maxSpeed;
        }
        else
        {
            if (hInputToAppliy > cutOffSpeed || cutOffSpeed * -1f > hInputToAppliy)
                hInputToAppliy += decelPerSec * Time.deltaTime * Mathf.Sign(hInputToAppliy);
            else
                hInputToAppliy = 0f;
        }

        // Fire Hooks
        if (fireHook)
            Debug.Log("Right hook fired");

        if (reelHook > 0.1)
            Debug.Log("Right hook reeled");

        // Add values
        directionToMove.y += gravityToApply;
        directionToMove.x += hInputToAppliy;

        // Set position
        newPosition += directionToMove * Time.deltaTime;
        rb.MovePosition(newPosition);
    }

    private bool CheckGround()
    {
        RaycastHit2D checkGroundRay;
        Vector3 rayOrigin = transform.position;
        rayOrigin.y -= distanceToBottom + 0.001f;
        Vector3 tempOrigin = rayOrigin;

        // Direct beneath check
        checkGroundRay = Physics2D.Raycast(rayOrigin, Vector2.down, bottomCheckBuffer);
        if (checkGroundRay.collider != null)
        {
            if (checkGroundRay.collider.CompareTag("Ground"))
            {
                return true;
            }
        }

        // Left max check
        rayOrigin = tempOrigin;
        rayOrigin.x -= sideCheckBuffer;
        checkGroundRay = Physics2D.Raycast(rayOrigin, Vector2.down, bottomCheckBuffer);
        if (checkGroundRay.collider != null)
        {
            if (checkGroundRay.collider.CompareTag("Ground"))
            {
                return true;
            }
        }

        // Right max check
        rayOrigin = tempOrigin;
        rayOrigin.x += sideCheckBuffer;
        checkGroundRay = Physics2D.Raycast(rayOrigin, Vector2.down, bottomCheckBuffer);
        if (checkGroundRay.collider != null)
        {
            if (checkGroundRay.collider.CompareTag("Ground"))
            {
                return true;
            }
        }

        return false;
    }
}
