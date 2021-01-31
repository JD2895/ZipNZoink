using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement_v3 : MonoBehaviour
{
    #region Variables

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
    [SerializeField] private CircleCollider2D bottomCollider = new CircleCollider2D();
    [SerializeField] private LayerMask groundLayer = new LayerMask();

    [Header("Ground Movement")]
    public float accelerateValueHorizontal = 30;
    public float decelerateValueHorizontal = 50;
    public float maxGroundSpeedViaInput = 7;
    public float movementCancelHelper = 4;

    [Header("Jump Movement")]
    public float jumpForce = 15.2f;
    public float jumpCutoff = 4;
    public float jumpCancelGravityMult = 1.5f;
    //Jump buffer adds a buffer when the player presses the jump button, which can help if the press the button "too early" while in the air
    public float jumpBufferTime = 0.1f;
    //like above does not need to be public, currently is for testing
    public float jumpBufferTimer;

    [Header("Air Movement")]
    public float airAccelerateValue = 10;
    public float airDecelerateValue = 10;
    public float maxHorizontalAirSpeed = 7;
    public float dashForceMult = 38.3f;

    [Header("Hook Movement")]
    public float horHookMoveMult = 30;
    public float hookJumpForceMult = 3;
    public float hookJumpHorizMult = 0.7f;
    public float superHookJumpForceMult = 10;

    [Header("Wall Movement")]
    //The speed the player slides down a wall
    //Moving entirely by gravity with y set to 0
    //NOTE: might, if using values lower than 0, have to clamp to that value
    public float wallSlideSpeed = 0.0f;
    //The speed the player jumps off the wall in the x direction
    public float wallJumpOffSpeed = 8.5f;
    //hangTime is used to allow the player a little bit of time to jump after walking off of a ledge 
    public float hangTime = 0.2f;
    //hangTimer does not need to be public, currently is public for testing
    public float hangTimer;
    private bool isWallSliding = false;
    private bool wallJumpQueued;
    private int wallSide = 0;
    public float curHorSpeed; // Use inspector in debug mode to see this
    public Vector2 watchthis;

    [Header("Movement Helpers")]
    private float hookSwingToApply;
    private bool jumpQueued = false;
    private bool spinFlipQueued = false;
    private bool dashQueued = false;
    private bool isAirJumping;
    private HoriDirection directionFacing;    // -1 is left, 1 is right
    private HoriDirection directionWhenJumpStarted;
    private float rbDefaultGravityScale;
    private static float deadZoneValue = 0.1f;
    //TODO: Need to investigate how this works with air movement

    [Header("Input Containers")]
    private float curHorInput = 0;
    private float curVerInput = 0;  // One Hook Variant
    private bool fireRightHook = false;
    private bool unhookRightHook = false;
    private float reelRightHook = 0;
    private bool unhookLeftHook = false;
    private bool fireLeftHook = false;
    private float reelLeftHook = 0;
    private bool jumpInput = false;
    private bool dashDown = false;
    private bool spinFlip = false;

    [Header("Visual Data")] // Most of this should eventually just be handles by an animator
    public Sprite[] playerSprites;
    public GameObject lineRenderContainerR;
    public GameObject lineRenderContainerL;
    private SpriteRenderer playerSprite;
    
    #endregion

    #region Initialization Methods

    private void OnEnable()
    {
        HookHelper.OnHookHitGround += HookHitGround;
        HookHelper.OnHookHitHazard += HookHitHazard;
    }

    private void OnDisable()
    {
        HookHelper.OnHookHitGround -= HookHitGround;
        HookHelper.OnHookHitHazard -= HookHitHazard;
    }

    private void Awake()
    {
        if (DetachHook == null) DetachHook = new UnityEvent();

        hookR_controller = this.gameObject.AddComponent<HookController>();
        hookR_controller.SetLineContainer(lineRenderContainerR);
        hookR_controller.SetupHook(hookR_Object, commonHookData);

        hookL_controller = this.gameObject.AddComponent<HookController>();
        hookL_controller.SetLineContainer(lineRenderContainerL);
        hookL_controller.SetupHook(hookL_Object, commonHookData);

        rb = GetComponent<Rigidbody2D>();
        rbDefaultGravityScale = rb.gravityScale;

        playerSprite = this.GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        curHorSpeed = 0.0f;
    }
    #endregion

    private void Update()
    {
        CheckInput();

        if (curHorInput > 0)
        {
            directionFacing = HoriDirection.Right;
            playerSprite.sprite = playerSprites[0];
        }
        else if (curHorInput < 0)
        {
            directionFacing = HoriDirection.Left;
            playerSprite.sprite = playerSprites[1];
        }

        hookSwingToApply = 0;

        if (IsGrounded())
        {
            // Set defaults
            rb.gravityScale = rbDefaultGravityScale;
            isAirJumping = false;
            isWallSliding = false;

            hangTimer = hangTime;

            if (jumpBufferTimer >= 0)
            {
                jumpBufferTimer = 0;
                jumpQueued = true;
            }
            
            if (!spinFlipQueued)
            {
                if (spinFlip)
                {
                    spinFlipQueued = spinFlip;
                }
            }
        }
        else
        {
            hangTimer -= Time.deltaTime;

            // Down dash queueing 
            if (!dashQueued)
            {
                dashQueued = dashDown;
            }

            if (IsHooked())
            {
                // Set defaults
                rb.gravityScale = rbDefaultGravityScale;

                curHorSpeed = rb.velocity.x;
                hookSwingToApply = curHorInput * horHookMoveMult;
                //directionWhenJumpStarted = directionFacing; // temp fix

                if (!jumpQueued)
                {
                    jumpQueued = jumpInput;
                }

                isWallSliding = false;
            }
            else if(isWallSliding)
            {
                if (!wallJumpQueued)
                {
                    wallJumpQueued = jumpInput;
                }
            }
            //In the air, unhooked
            else
            {
                if (!Input.GetButton("Jump") && !isAirJumping)
                {
                    if (rb.velocity.y > jumpCutoff)
                    {
                        rb.gravityScale = rbDefaultGravityScale * jumpCancelGravityMult; // need to put in a check, if the player is falling faster than a 'max fall speed', change gravity scale back to default
                    }
                }
                //check for wall in the direction the player is currently moving
                //if player colides with that wall, set wall sliding to true
                //To make sure the player doesn't get stuck on small blocks
                if (rb.velocity.y < 12.0f)
                {
                    if (curHorInput != 0)
                    {
                        float distance = 0.03f;
                        Vector3 castOrigin = bottomCollider.bounds.center;
                        Vector2 castSize = new Vector2(bottomCollider.radius * 2, bottomCollider.radius * 2);
                        RaycastHit2D raycastHit = Physics2D.BoxCast(castOrigin, castSize, 0f, new Vector2((int)directionFacing, 0.0f), distance, groundLayer);

                        isWallSliding = raycastHit.collider != null;
                        if (isWallSliding)
                        {
                            //Using this to correct for oddities with adjusting the gravity scale above
                            rb.gravityScale = rbDefaultGravityScale;

                            wallSide = (int)directionFacing;
                        }
                    }
                }

            }
        }
        // Hang time
        if (hangTimer > 0.0f)
        {
            if (!jumpQueued)
            {
                jumpQueued = jumpInput;
            }
        }

        // Jump buffering
        if(jumpInput && !IsHooked())
        {
            //Debug.Log("Jump Buffering");
            jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        ControlHooks();
        watchthis = rb.velocity;
    }

    private void CheckInput()
    {
        // Check input
        curHorInput = Input.GetAxis("Horizontal");
        curVerInput = Input.GetAxis("Vertical");    // One Hook Variant
        if (Mathf.Abs(curHorInput) < deadZoneValue) //deadzone check
        {
            curHorInput = 0;
        }
        jumpInput = Input.GetButtonDown("Jump");
        fireRightHook = Input.GetButtonDown("Right Hook Fire");
        unhookRightHook = Input.GetButtonUp("Right Hook Fire");
        reelRightHook = Input.GetAxis("Right Hook Reel");
        fireLeftHook = Input.GetButtonDown("Left Hook Fire");
        unhookLeftHook = Input.GetButtonUp("Left Hook Fire");
        reelLeftHook = Input.GetAxis("Left Hook Reel");
        dashDown = Input.GetButtonDown("Cancel");
        spinFlip = Input.GetButtonDown("Fire3");
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
        if (fireRightHook && DebugOptions.hookFireVarient == HookFireVariant.OneHook)
        {
            hookR_controller.FireHook(SnapOctDirection(curHorInput, curVerInput));
        }
        else if (fireRightHook)
        {
            hookR_controller.FireHook(transform.up + transform.right);
        }
        else if (unhookRightHook && DebugOptions.hookFireVarient == HookFireVariant.Hold)
        {
            isAirJumping = true;    //temp fix? to avoid accidental jump cancelling. might be fixed with state machine?
            hookR_controller.DisconnectHook();
        }
        hookR_controller.ReelHook(reelRightHook);

        // Left hook control
        if (fireLeftHook && DebugOptions.hookFireVarient == HookFireVariant.OneHook)
        {
            hookR_controller.FireHook(SnapOctDirection(curHorInput, curVerInput));  // Intentionally only use the right hook
        } 
        else if (fireLeftHook)
        {
            hookL_controller.FireHook(transform.up + -transform.right);
        }
        else if (unhookLeftHook && DebugOptions.hookFireVarient == HookFireVariant.Hold)
        {
            isAirJumping = true;    //temp fix? to avoid accidental jump cancelling. might be fixed with state machine?
            hookL_controller.DisconnectHook();
        }
        hookL_controller.ReelHook(reelLeftHook);
    }

    private void FixedUpdate()
    {
        if (!IsGrounded())
        {
            if (IsHooked())
            {
                Vector2 forceToApply = new Vector2(hookSwingToApply * Time.fixedDeltaTime, 0);
                rb.AddForce(forceToApply);
            }
            else if (isWallSliding)
            {
                HandleWallSliding();
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

        if (dashQueued)
        {
            Debug.Log("dashing");
            dashQueued = false;
            Vector2 forceToApply = new Vector2(0, -1f * dashForceMult * Time.fixedDeltaTime);
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(forceToApply, ForceMode2D.Impulse);
        }
    }

    private void HandleWallSliding()
    {
        rb.velocity = new Vector2(0, wallSlideSpeed);
        if (wallJumpQueued)
        {
            // Regular jump
            wallJumpQueued = false;
            directionWhenJumpStarted = directionFacing;

            ApplyJump(0, jumpForce);
            curHorSpeed = -wallSide * wallJumpOffSpeed;
            isWallSliding = false;
            rb.gravityScale = rbDefaultGravityScale;
        }
        else
        {
            //Get off the wall if the player presses away from it
            if(curHorInput * wallSide < 0)
            {
                curHorSpeed = -wallSide * wallJumpOffSpeed;
                isWallSliding = false;
                rb.gravityScale = rbDefaultGravityScale;

            }
        }    
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
        // Ground Jump
        if (jumpQueued && EvaluateHookState() == (int)HookedState.None)
        {
            //Debug.Log("Regular Jump");
            jumpQueued = false;
            hangTimer = 0.0f;
            jumpBufferTimer = 0;
            directionWhenJumpStarted = directionFacing;

            ApplyJump(0, jumpForce);
        } 
        // Hook Jump
        else if (jumpQueued && EvaluateHookState() == (int)HookedState.One && DebugOptions.hookJump)
        {
            //Debug.Log("Hook Jump!");
            jumpQueued = false;
            DetachHook.Invoke();

            isAirJumping = true;
            directionWhenJumpStarted = directionFacing;

            rb.velocity = new Vector2(rb.velocity.x * hookJumpHorizMult, 0f);
            ApplyJump(0, hookJumpForceMult);


        }
        // Super Jump
        else if (jumpQueued && EvaluateHookState() == (int)HookedState.Both)
        {
            //Debug.Log("Super Hook Jump!!!");
            DetachHook.Invoke();

            isAirJumping = true;
            jumpQueued = false;

            rb.velocity = Vector2.zero;
            ApplyJump(0, superHookJumpForceMult);
        }

        // Flip Jump
        if (spinFlipQueued)
        {
            spinFlipQueued = false;
            directionWhenJumpStarted = directionFacing;

            ApplyJump(0, jumpForce);
            StartCoroutine("SpinPlayer");
        }
    }

    private void ApplyJump(float xforce, float yforce)
    {
        rb.velocity += new Vector2(xforce, yforce);
    }

    IEnumerator SpinPlayer()
    {
        rb.freezeRotation = false;

        float timeToSpin = 0.5f;
        float startTime = Time.time;
        float fracComplete = (Time.time - startTime) / timeToSpin;
        float newEuler = 0f;

        while (fracComplete < 0.99f)
        {
            //newEuler = Mathf.Lerp(0f, 370f, fracComplete);
            newEuler = Mathf.Lerp(0f, 360f, (1.6f + (1f / (-fracComplete - 0.65f))));
            newEuler *= -1f * (float) directionWhenJumpStarted;
            rb.MoveRotation(newEuler);
            yield return null;
            fracComplete = (Time.time - startTime) / timeToSpin;
        }

        // Reset rotation
        rb.rotation = 0f;
        yield return null;

        rb.freezeRotation = true;
    }
    
    #endregion

    #region Hook Methods and Enums

    private void HookHitGround(HookSide hookSide)
    {
        // Cancel any jump cancelling that might have been happening
        isAirJumping = false;

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

    private void HookHitHazard(HookSide hookSide)
    {

        if (hookSide == HookSide.Right)
        {
            hookR_controller.DisconnectHook();
        }
        else if (hookSide == HookSide.Left)
        {
            hookL_controller.DisconnectHook();
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

    public static Vector2 SnapOctDirection(float horVal, float verValue)
    {
        Vector2 snappedDirection = new Vector2();
        if (horVal > deadZoneValue)
            snappedDirection.x = 1f;
        else if (horVal < -deadZoneValue)
            snappedDirection.x = -1f;
        if (verValue > deadZoneValue)
            snappedDirection.y = 1f;
        else if (verValue < -deadZoneValue)
            snappedDirection.y = -1f;
        return snappedDirection.normalized;
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

    #region On-Screen Debug Text

    private GUIStyle bigFont = new GUIStyle();

    private void OnGUI()
    {
        if (DebugOptions.debugText)
        {
            bigFont.fontSize = 20;

            GUI.Label(new Rect(100, 200, 1000, 1000),
                "IsGrounded? " + IsGrounded() +
                "\nIsHooked? " + IsHooked() +
                "\ncurHorSpeed? " + curHorSpeed +
                "\nHookR_Connected: " + hookR_connected,
                bigFont);
        }
    }

    #endregion

}
