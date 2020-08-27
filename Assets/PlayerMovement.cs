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
    private bool hitGround;

    // Gravity
    public float maxGravity;
    public float timeToMaxGravity;
    private float gravPerSec;
    private bool applyGravity = true;
    private float gravityToApply = 0f;

    // Movement
    public float maxSpeed;
    public float cutOffSpeed;
    public float timeToMaxSpeed;
    public float timeToNoSpeed;
    private float accelPerSec;
    private float decelPerSec;
    private float hInputToAppliy = 0f;

    // Grappling Hook
    public GameObject hookObject;
    private GameObject hookContainer;
    private bool hookOut = false;
    private bool hookOnGround = false;
    private LineRenderer hookLine;
    public float hookForceMultiplier = 1.0f;

    // Helpers
    private Rigidbody2D rb;
    private Vector3 directionToMove;
    private Vector3 momentum;
    private Vector3 newPosition;
    private Vector2 hookMovementDirection;

    private void OnEnable()
    {
        HookHelper.OnHookHitGround += HookHitGround;
    }

    private void OnDisable()
    {
        HookHelper.OnHookHitGround -= HookHitGround;
    }

    void Awake()
    {
        directionToMove = Vector3.zero;
        rb = this.GetComponent<Rigidbody2D>();

        //***** Initial setup *****
        // Movement Setup
        accelPerSec = maxSpeed / timeToMaxSpeed;
        decelPerSec = -maxSpeed / timeToNoSpeed;
        gravPerSec = maxGravity / timeToMaxGravity;
        momentum = Vector3.zero;
        // Hook Setup
        hookContainer = new GameObject("HookContainer");
        hookContainer.transform.parent = transform;
        hookLine = hookContainer.AddComponent<LineRenderer>();
        hookLine.widthMultiplier = 0.1f;
        hookLine.positionCount = 2;
    }

    private void FixedUpdate()
    {
        // Setup
        directionToMove = Vector2.zero;
        hookMovementDirection = Vector2.zero;
        newPosition = transform.position;
        groundBelowCheck = CheckGround();

        // Check to reset directionToMove
        if (groundBelowCheck && hitGround)
        {
            hitGround = false;
            gravityToApply = 0f;
        }
        
        // Gravity
        applyGravity = !groundBelowCheck;
        if (applyGravity)
        {
            gravityToApply -= gravPerSec * Time.fixedDeltaTime;
            if (gravityToApply < (maxGravity * -1f))
                gravityToApply = maxGravity * -1f;
        }

        // Check input
        float curHorInput = Input.GetAxis("Horizontal");
        bool fireHook = Input.GetButton("Right Hook Fire");
        float reelHook = Input.GetAxis("Right Hook Reel");

        // Apply horiztonal input
        if (Mathf.Abs(curHorInput) > inputDeadZone)
        {
            if (Mathf.Sign(hInputToAppliy) != Mathf.Sign(curHorInput))
                hInputToAppliy *= -1f;  // Immediately change direction but maintain the previous speed
            hInputToAppliy += curHorInput * accelPerSec * Time.fixedDeltaTime;
            if (Mathf.Abs(hInputToAppliy) > maxSpeed)
                hInputToAppliy = Mathf.Sign(hInputToAppliy) * maxSpeed;
        }
        else
        {
            if (hInputToAppliy > cutOffSpeed || cutOffSpeed * -1f > hInputToAppliy)
                hInputToAppliy += decelPerSec * Time.fixedDeltaTime * Mathf.Sign(hInputToAppliy);
            else
                hInputToAppliy = 0f;
        }

        // Fire Hook
        if (fireHook && !hookOut)
        {
            hookOut = true;
            DrawHook(hookObject);
            Debug.Log("Right hook fired");
        }

        // Reel Hook
        if (reelHook > 0.1 && hookOnGround)
        {
            Debug.Log("Right hook reeling");
            hookMovementDirection = GetHookDirection(hookObject);
            hookMovementDirection *= hookForceMultiplier * Time.fixedDeltaTime;
        }

        // Drawing
        if (hookOut)
        {
            DrawHook(hookObject);
        }

        // Add values
        directionToMove.y += gravityToApply;
        directionToMove.y += hookMovementDirection.y;
        directionToMove.x += hInputToAppliy;
        directionToMove.x += hookMovementDirection.x;

        /*
        // Momentum
        if (groundBelowCheck)
        {
            momentum = Vector3.zero;
        }
        else
        {
            directionToMove.y += momentum.y;
            directionToMove.x += momentum.x;
        }*/

        // Set position
        newPosition += directionToMove * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        // Post calculations
        momentum = directionToMove;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            hitGround = true;
        }
    }

    private void DrawHook(GameObject hookPoint)
    {
        hookLine.SetPosition(0, this.transform.position);
        hookLine.SetPosition(1, hookPoint.transform.position);
    }

    private Vector2 GetHookDirection(GameObject hookPoint)
    {
        Vector2 hookDirection = new Vector2();

        hookDirection.x = hookPoint.transform.position.x - this.transform.position.x;
        hookDirection.y = hookPoint.transform.position.y - this.transform.position.y;

        return hookDirection.normalized;
    }

    private void HookHitGround()
    {
        hookOnGround = true;
    }
}
