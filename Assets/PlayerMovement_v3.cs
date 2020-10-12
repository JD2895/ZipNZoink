using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_v3 : MonoBehaviour
{
    /*** HOOK DATA ***/
    public GameObject hookR_Object;     // The hook head for the right hook.
    public GameObject hookL_Object;     // The hook head for the left hook.
    private HookController hookR_controller;
    private HookController hookL_controller;
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
    //Using to decelerate
    private float halfSpeed;

    /*** INPUT VARS ***/
    private float curHorInput = 0;
    private bool fireRightHook = false;
    private float reelRightHook = 0;
    private bool fireLeftHook = false;
    private float reelLeftHook = 0;
    private bool jumpInput = false;

    /*** VISUAL DATA ***/
    public SpriteRenderer playerSprite;

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
        hookR_controller = this.gameObject.AddComponent<HookController>();
        hookR_controller.SetupHook(hookR_Object, commonHookData);
        hookL_controller = this.gameObject.AddComponent<HookController>();
        hookL_controller.SetupHook(hookL_Object, commonHookData);

        //playerSprite = this.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        halfSpeed = maxSpeed * 0.5f;
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

        if (curHorInput > 0)
        {
            directionFacing = HoriDirection.Right;
            playerSprite.flipX = false;
        }
        else if (curHorInput < 0)
        {
            directionFacing = HoriDirection.Left;
            playerSprite.flipX = true;
        }

        horiToApply = 0;

        //Debug.Log(directionWhenJumpStarted);
        if (IsGrounded())
        {
            horiToApply = curHorInput * horMoveMult;
            if (!jumpQueued)
                jumpQueued = jumpInput;
        }
        else
        {
            if (hookL_connected || hookR_connected)
            {
                horiToApply = curHorInput * horHookMoveMult;
                directionWhenJumpStarted = directionFacing; // temp fix
            }
            else
            {
                if (directionFacing != directionWhenJumpStarted)
                {
                 //   directionWhenJumpStarted = directionFacing;
                    horiToApply = curHorInput * horReverseAirMoveMult;  // Give the player more force to cancel the jump

                }
                else
                {
                    horiToApply = curHorInput * horAirMoveMult;
                }
                if (Input.GetButtonUp("Jump"))
                {
                    if (rb.velocity.y > 4.0f)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, 4.0f);
                    }
                }
            }
        }

        // Right hook control
        if (fireRightHook)
        {
            //hookR_Controller.FireHook(new Vector2(1, 1));
            hookR_controller.FireHook(transform.up + transform.right);
            hookR_connected = hookR_connected ? false : true;   // If the hook is already connected, the hook is now going ot be disconnected
        }
        hookR_controller.ReelHook(reelRightHook);

        // Left hook control
        if (fireLeftHook)
        {
            //hookL_Controller.FireHook(new Vector2(-1, 1));
            hookL_controller.FireHook(transform.up + -transform.right);
            hookL_connected = hookL_connected ? false : true;   // If the hook is already connected, the hook is now going ot be disconnected
        }
        hookL_controller.ReelHook(reelLeftHook);
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        Vector2 forceToApply = new Vector2(horiToApply * Time.fixedDeltaTime, 0);
        rb.AddForce(forceToApply);
        if (IsGrounded())
        {
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.AddForce(forceToApply * -1); // push equally in the oppposite direction once the max speed limit is reached
            }
            //Attempt at deceleration 
          /*  if(curHorInput == 0 && Mathf.Abs(rb.velocity.x) > halfSpeed)
            {
                if (rb.velocity.x > 0)
                {
                    rb.velocity = new Vector2(halfSpeed, rb.velocity.y);
                }
                else if(rb.velocity.x < 0)
                {
                    rb.velocity = new Vector2(-halfSpeed, rb.velocity.y);
                }
            }*/
        }

        if (jumpQueued)
        {
            rb.AddForce(new Vector2(0, jumpForceMult * Time.fixedDeltaTime), ForceMode2D.Impulse);
            jumpQueued = false;
            directionWhenJumpStarted = directionFacing;
        }
    }

    private void HookHitGround(HookSide hookSide)
    {
        if (hookSide == HookSide.Right)
        {
            hookR_controller.ChangeHookConnectedState(true);
            hookR_connected = true;
        }
        else if (hookSide == HookSide.Left)
        {
            hookL_controller.ChangeHookConnectedState(true);
            hookL_connected = true;
        }
    }

    private bool IsGrounded()
    {
        float extraHeight = 0.05f;
        RaycastHit2D raycastHit = Physics2D.CircleCast(bottomCollider.bounds.center, bottomCollider.radius, Vector2.down, extraHeight, groundLayer);
        //Debug.DrawLine(bottomCollider.bounds.center, bottomCollider.bounds.center + (Vector3.down * extraHeight), Color.red);
        return raycastHit.collider != null;
    }

    enum HoriDirection
    {
        Left = -1,
        Right = 1
    }
}
