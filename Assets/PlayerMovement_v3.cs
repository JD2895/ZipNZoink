using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_v3 : MonoBehaviour
{
    /*** HOOK DATA ***/
    public GameObject hookR_Object;     // The hook head for the right hook.
    public GameObject hookL_Object;     // The hook head for the left hook.
    private HookController hookR_Controller;
    private HookController hookL_Controller;
    public HookControllerCommonSetup commonHookData;    // Common data for hook setup
    private bool hookR_connected = false;
    private bool hookL_connected = false;

    /*** MOVEMENT DATA ***/
    private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D bottomCollider;
    [SerializeField] private LayerMask groundLayer;
    public float jumpForceMult;
    public float horMoveMult;
    public float horAirMoveMult;
    public float horReverseAirMoveMult;
    public float horHookMoveMult;
    private float horiToApply;
    private bool jumpQueued;
    private HoriDirection directionFacing;    // -1 is left, 1 is right
    private HoriDirection directionWhenJumpStarted;
    public float maxSpeed;

    /*** INPUT VARS ***/
    private float curHorInput = 0;
    private bool fireRightHook = false;
    private float reelRightHook = 0;
    private bool fireLeftHook = false;
    private float reelLeftHook = 0;
    private bool jumpInput = false;

    private void OnEnable()
    {
        HookHelper.OnHookHitGround += HookHitGround;
    }

    private void OnDisable()
    {
        HookHelper.OnHookHitGround -= HookHitGround;
    }

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        hookR_Controller = this.gameObject.AddComponent<HookController>();
        hookR_Controller.SetupHook(hookR_Object, commonHookData);
        hookL_Controller = this.gameObject.AddComponent<HookController>();
        hookL_Controller.SetupHook(hookL_Object, commonHookData);
    }

    private void Update()
    {
        // Check input
        curHorInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        fireRightHook = Input.GetButtonDown("Right Hook Fire");
        reelRightHook = Input.GetAxis("Right Hook Reel");
        fireLeftHook = Input.GetButtonDown("Left Hook Fire");
        reelLeftHook = Input.GetAxis("Left Hook Reel");

        if (curHorInput > 0.1)
        {
            directionFacing = HoriDirection.Right;
        }
        else if (curHorInput < -0.1)
        {
            directionFacing = HoriDirection.Left;
        }

        horiToApply = 0;

        Debug.Log(directionWhenJumpStarted);
        if (IsGrounded())
        {
            horiToApply = curHorInput * horMoveMult;
            if (!jumpQueued)
                jumpQueued = jumpInput;
        }
        else
        {
            if(hookL_connected || hookR_connected)
            {
                horiToApply = curHorInput * horHookMoveMult;
                directionWhenJumpStarted = directionFacing;
            }
            else
            {
                if (directionFacing != directionWhenJumpStarted)
                {
                    horiToApply = curHorInput * horReverseAirMoveMult;  // Give the player more force to cancel the jump
                }
                else
                {
                    horiToApply = curHorInput * horAirMoveMult;
                }
            }
        }

        // Right hook control
        if (fireRightHook)
        {
            hookR_Controller.FireHook(new Vector2(1, 1));
            hookR_connected = hookR_connected ? false : true;   // If the hook is already connected, the hook is now going ot be disconnected
        }
        hookR_Controller.ReelHook(reelRightHook);

        // Left hook control
        if (fireLeftHook)
        {
            hookL_Controller.FireHook(new Vector2(-1, 1));
            hookL_connected = hookL_connected ? false : true;   // If the hook is already connected, the hook is now going ot be disconnected
        }
        hookL_Controller.ReelHook(reelLeftHook);
    }

    private void FixedUpdate()
    {
        Vector2 forceToApply = new Vector2(horiToApply * Time.fixedDeltaTime, 0);
        rb.AddForce(forceToApply);
        if (jumpQueued)
        {
            rb.AddForce(new Vector2(0, jumpForceMult * Time.fixedDeltaTime), ForceMode2D.Impulse);
            jumpQueued = false;
            directionWhenJumpStarted = directionFacing;
        }

        /*if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.AddForce(forceToApply * -1); // pussh equally in the oppposite direction once the max speed limit is reached
        }*/
    }

    private void HookHitGround(HookSide hookSide)
    {
        if (hookSide == HookSide.Right)
        {
            hookR_Controller.ChangeHookConnectedState(true);
            hookR_connected = true;
        }
        else if (hookSide == HookSide.Left)
        {
            hookL_Controller.ChangeHookConnectedState(true);
            hookL_connected = true;
        }
    }

    private bool IsGrounded()
    {
        float extraHeight = 0.05f;
        RaycastHit2D raycastHit = Physics2D.CircleCast(bottomCollider.bounds.center, bottomCollider.radius, Vector2.down, extraHeight, groundLayer);
        Debug.DrawLine(bottomCollider.bounds.center, bottomCollider.bounds.center + (Vector3.down * extraHeight), Color.red);
        return raycastHit.collider != null;
    }

    enum HoriDirection
    {
        Left = -1,
        Right = 1
    }
}
