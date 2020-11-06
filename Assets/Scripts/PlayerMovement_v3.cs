﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement_v3 : MonoBehaviour
{
    public static UnityEvent DetachHook;

    [Header("Hook Setup")]
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

    [Header("Ground Movement")]
    public float accelerateValueHorizontal = 30;
    public float decelerateValueHorizontal = 50;
    public float maxGroundSpeedViaInput = 7;
    public float movementCancelHelper = 4;

    [Header("Air Movement")]
    public float jumpForceMult = 38.3f;
    public float jumpCutoff = 4;
    public float jumpCancelGravityMult = 1.5f;
    public float airAccelerateValue = 10;
    public float airDecelerateValue = 10;
    public float maxHorizontalAirSpeed = 7;

    [Header("Hook Movement")]
    public float horHookMoveMult = 30;
    public float hookJumpForceMult = 3;
    public float superHookJumpForceMult = 10;

    /*** MOVEMENT HELPERS ***/
    private float horiToApply;
    private bool jumpQueued;
    private bool isAirJumping;
    private HoriDirection directionFacing;    // -1 is left, 1 is right
    private HoriDirection directionWhenJumpStarted;
    private float rbDefaultGravityScale;

    private float curHorSpeed; // Use inspector in debug mode to see this

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

    #region Initialization Methods

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
        if (DetachHook == null) DetachHook = new UnityEvent();

        hookR_controller = this.gameObject.AddComponent<HookController>();
        hookR_controller.SetupHook(hookR_Object, commonHookData);
        hookL_controller = this.gameObject.AddComponent<HookController>();
        hookL_controller.SetupHook(hookL_Object, commonHookData);

        rb = GetComponent<Rigidbody2D>();
        rbDefaultGravityScale = rb.gravityScale;

        //playerSprite = this.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        halfSpeed = maxGroundSpeedViaInput * 0.5f;
        curHorSpeed = 0.0f;
    }
    #endregion

    private void Update()
    {
        CheckInput();

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

        if (IsGrounded())
        {
            // Set defaults
            rb.gravityScale = rbDefaultGravityScale;
            isAirJumping = false;

            // Jump queueing 
            if (!jumpQueued)
            {
                jumpQueued = jumpInput;
            }
        }
        else
        {
            if (IsHooked())
            {
                // Set defaults
                rb.gravityScale = rbDefaultGravityScale;

                curHorSpeed = rb.velocity.x;
                horiToApply = curHorInput * horHookMoveMult;
                directionWhenJumpStarted = directionFacing; // temp fix

                if (!jumpQueued)
                {
                    jumpQueued = jumpInput;
                }
            }
            //In the air, unhooked
            else
            {
                if (Input.GetButtonUp("Jump") && !isAirJumping)
                {
                    if (rb.velocity.y > jumpCutoff)
                    {
                        rb.gravityScale = rbDefaultGravityScale * jumpCancelGravityMult; // need to put in a check, if the player is falling faster than a 'max fall speed', change gravity scale back to default
                    }
                }

            }
        }

        ControlHooks();
    }

    private void CheckInput()
    {
        // Check input
        curHorInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(curHorInput) < 0.1) //deadzone check
        {
            curHorInput = 0;
        }
        jumpInput = Input.GetButtonDown("Jump");
        fireRightHook = Input.GetButtonDown("Right Hook Fire");
        reelRightHook = Input.GetAxis("Right Hook Reel");
        fireLeftHook = Input.GetButtonDown("Left Hook Fire");
        reelLeftHook = Input.GetAxis("Left Hook Reel");
    }

    private bool IsGrounded()
    {
        float extraHeight = 0.03f;
        Vector3 castOrigin = bottomCollider.bounds.center;
        Vector2 castSize = new Vector2(bottomCollider.radius * 2, bottomCollider.radius * 2);
        RaycastHit2D raycastHit = Physics2D.BoxCast(castOrigin, castSize, 0f, Vector2.down, extraHeight, groundLayer);
        //RaycastHit2D raycastHit = Physics2D.CircleCast(castOrigin, bottomCollider.radius, Vector2.down, extraHeight, groundLayer);

        //One Way Platform Conditional: Make sure the player is ABOVE the platform before IsGrounded() is true
        if (raycastHit.collider != null && raycastHit.collider.TryGetComponent(out PlatformEffector2D platEffector))
        {
            if (platEffector.useOneWay && rb.velocity.y > 0.1f || this.transform.position.y - 0.5f < raycastHit.point.y)
            {
                return false;
            }
        }

        return raycastHit.collider != null;
    }

    private void ControlHooks()
    {
        
        //TODO: Stephen: I think we should check the hook state using public bools from Hook Controller, rather than a local variable that gets toggled
        hookR_connected = hookR_controller.h_onGround;
        hookL_connected = hookL_controller.h_onGround;
        
        // Right hook control
        if (fireRightHook)
        {
            hookR_controller.FireHook(transform.up + transform.right);
        }
        hookR_controller.ReelHook(reelRightHook);

        // Left hook control
        if (fireLeftHook)
        {
            hookL_controller.FireHook(transform.up + -transform.right);
        }
        hookL_controller.ReelHook(reelLeftHook);
    }

    private void FixedUpdate()
    {
        if (!IsGrounded())
        {
            if (IsHooked())
            {
                Vector2 forceToApply = new Vector2(horiToApply * Time.fixedDeltaTime, 0);
                rb.AddForce(forceToApply);
            }
            else
            {
                ApplyHorizontalAirMovement();
            }

        }
        else
        {
            ApplyGroundMovement();
        }

        JumpLogic();
    }

    #region FixedUpdate Movement Methods

    private void ApplyHorizontalAirMovement()
    {
        curHorSpeed += airAccelerateValue * curHorInput * Time.fixedDeltaTime;

        if (Mathf.Abs(curHorSpeed) > maxHorizontalAirSpeed)
        {
            if (curHorSpeed > 0)
            {
                curHorSpeed -= airDecelerateValue * Time.fixedDeltaTime;
                if (curHorInput < 0)    // let the player contribute to slowing down
                {
                    curHorSpeed += airAccelerateValue * movementCancelHelper * curHorInput * Time.fixedDeltaTime;
                }
            }
            else if (curHorSpeed < 0)
            {
                curHorSpeed += airDecelerateValue * Time.fixedDeltaTime;
                if (curHorInput > 0)    // let the player contribute to slowing down
                {
                    curHorSpeed += airAccelerateValue * movementCancelHelper * curHorInput * Time.fixedDeltaTime;
                }
            }
        }
        rb.velocity = new Vector2(curHorSpeed, rb.velocity.y);
    }

    private void ApplyGroundMovement()
    {
        if (curHorInput == 0)
        {
            HandleGroundAcceleration();
        }
        else
        {
            HandleGroundDeceleration();
        }

        rb.velocity = new Vector2(curHorSpeed, rb.velocity.y);
    }

    private void HandleGroundAcceleration()
    {
        if (curHorSpeed > 0)
        {
            curHorSpeed -= decelerateValueHorizontal * Time.fixedDeltaTime;
            if (curHorSpeed < 0)
            {
                curHorSpeed = 0;
            }
        }
        else if (curHorSpeed < 0)
        {
            curHorSpeed += decelerateValueHorizontal * Time.fixedDeltaTime;
            if (curHorSpeed > 0)
            {
                curHorSpeed = 0;
            }
        }
    }

    private void HandleGroundDeceleration()
    {
        if (Mathf.Abs(curHorSpeed) > maxGroundSpeedViaInput)
        {
            if (curHorSpeed > 0)
            {
                curHorSpeed -= decelerateValueHorizontal * Time.fixedDeltaTime;
                if (curHorInput < 0)    // let the player contribute to slowing down
                {
                    curHorSpeed += accelerateValueHorizontal * movementCancelHelper * curHorInput * Time.fixedDeltaTime;
                }
            }
            else if (curHorSpeed < 0)
            {
                curHorSpeed += decelerateValueHorizontal * Time.fixedDeltaTime;
                if (curHorInput > 0)    // let the player contribute to slowing down
                {
                    curHorSpeed += accelerateValueHorizontal * movementCancelHelper * curHorInput * Time.fixedDeltaTime;
                }
            }
        }
        else
        {
            if (curHorSpeed >= 0)
            {
                if (curHorInput < 0)    // let the player contribute to slowing down
                {
                    curHorSpeed += accelerateValueHorizontal * movementCancelHelper * curHorInput * Time.fixedDeltaTime;
                }
                else
                {
                    curHorSpeed += accelerateValueHorizontal * curHorInput * Time.fixedDeltaTime;
                }
            }
            else if (curHorSpeed <= 0)
            {
                if (curHorInput > 0)    // let the player contribute to slowing down
                {
                    curHorSpeed += accelerateValueHorizontal * movementCancelHelper * curHorInput * Time.fixedDeltaTime;
                }
                else
                {
                    curHorSpeed += accelerateValueHorizontal * curHorInput * Time.fixedDeltaTime;
                }
            }
        }
    }
    private void JumpLogic()
    {
        if (jumpQueued && IsGrounded())
        {
            // Regular jump
            jumpQueued = false;
            directionWhenJumpStarted = directionFacing;

            ApplyJump(0, jumpForceMult * Time.fixedDeltaTime);


        } else if (jumpQueued && EvaluateHookState() == (int)HookedState.One)
        {
            // Hook jump
            Debug.Log("Hook Jump!");
            DetachHook.Invoke();

            isAirJumping = true;
            jumpQueued = false;
            directionWhenJumpStarted = directionFacing;

            rb.velocity = new Vector2(rb.velocity.x, 0f);
            ApplyJump(rb.velocity.x * Time.fixedDeltaTime, hookJumpForceMult * Time.fixedDeltaTime);
            

        } else if (jumpQueued && EvaluateHookState() == (int)HookedState.Both)
        {
            //ADD "SUPER JUMP" HERE
            Debug.Log("Super Jump!");
            DetachHook.Invoke();

            isAirJumping = true;
            jumpQueued = false;

            rb.velocity = Vector2.zero;
            ApplyJump(0, superHookJumpForceMult * Time.fixedDeltaTime);

        } else if(jumpQueued && EvaluateHookState() == (int)HookedState.None)
        {
            jumpQueued = false;
        }
    }

    private void ApplyJump(float xforce, float yforce)
    {
        Vector2 jumpVector = new Vector2(xforce, yforce);
        rb.AddForce(jumpVector, ForceMode2D.Impulse);
    }
    
    #endregion

    #region Hook Methods and Enums

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

    private int EvaluateHookState()
    {
        if ((hookL_controller.h_onGround && !hookR_controller.h_onGround)
            || (!hookL_controller.h_onGround && hookR_controller.h_onGround))
        {
            return 1;
        }

        if (hookL_controller.h_onGround && hookR_controller.h_onGround)
        {
            return 2;
        }

        return 0;
    }

    private bool IsHooked()
    {
        return (hookL_connected || hookR_connected);
    }

    enum HookedState
    { 
        None = 0,
        One = 1,
        Both = 2
    
    }
    
    enum HoriDirection
    {
        Left = -1,
        Right = 1
    }

    #endregion

    private GUIStyle bigFont = new GUIStyle();

    private void OnGUI()
    {
        bigFont.fontSize = 25;

        GUI.Label(new Rect(100, 200, 1000, 1000),
            "IsGrounded? " + IsGrounded() +
            "\nIsHooked? " + IsHooked() +
            "\ncurHorSpeed? " + curHorSpeed +
            "\nHookR_Connected: " + hookR_connected,
            bigFont);
    }

}
