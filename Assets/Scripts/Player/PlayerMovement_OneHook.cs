using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement_OneHook : MonoBehaviour
{
    #region Variables

    public static UnityEvent DetachHook;

    [Header("Hook Setup")]
    public GameObject hookR_Object;     // The hook head for the right hook.
    private HookController hookR_controller;
    public HookControllerCommonSetup commonHookData;    // Common data for hook setup
    private bool hookR_connected = false;

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
    private bool jumpBuffered = false;
    private bool jumpCancelling = false;
    //hangTime is used to allow the player a little bit of time to jump after walking off of a ledge 
    public float hangTime = 0.2f;
    //hangTimer does not need to be public, currently is public for testing
    public float hangTimer;

    [Header("Air Movement")]
    public float airAccelerateValue = 10;
    public float airDecelerateValue = 10;
    public float maxHorizontalAirSpeed = 7;
    public float dashForceMult = 38.3f;
    public float wallJumpAirDisableTime = 0.2f;
    private bool allowAirMovement = true;

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
    public float wallStepOffSpeed = 8.5f;
    public float wallJumpOffForce = 8.5f;
    public float wallJumpBufferTime;
    private bool wallJumpBuffered = false;
    private bool isWallSliding = false;
    private bool hasWallBehind = false;
    private bool isOnWall = false;
    private bool wallJumpQueued;
    private int wallSide = 0;
    public float curHorSpeed; // Use inspector in debug mode to see this
    public Vector2 watchthis;

    [Header("Movement Helpers")]
    private float hookSwingToApply;
    private bool jumpQueued = false;
    private bool isAirJumping;
    private HoriDirection directionFacing;    // -1 is left, 1 is right
    private HoriDirection directionWhenJumpStarted;
    private float rbDefaultGravityScale;
    private static float deadZoneValue = 0.1f;
    //TODO: Need to investigate how this works with air movement

    [Header("Input Containers")]
    private float curHorInput = 0;
    private float curVerInput = 0;  // One Hook Variant
    //private bool fireRightHook = false;
    private float reelRightHook = 0;
    //private bool jumpInput = false;
    //private bool dashDown = false;

    [Header("Visual Data")] // Most of this should eventually just be handles by an animator
    public Sprite[] playerSprites;
    public GameObject lineRenderContainerR;
    public GameObject lineRenderContainerL;
    private SpriteRenderer playerSprite;

    // NEW INPUT SYSTEM
    PlayerControls controls;

    #endregion

    #region Initialization Methods

    private void Awake()
    {
        if (DetachHook == null) DetachHook = new UnityEvent();

        hookR_controller = this.gameObject.AddComponent<HookController>();
        hookR_controller.SetLineContainer(lineRenderContainerR);
        hookR_controller.SetupHook(hookR_Object, commonHookData);

        rb = GetComponent<Rigidbody2D>();
        rbDefaultGravityScale = rb.gravityScale;

        playerSprite = this.GetComponentInChildren<SpriteRenderer>();

        // New input system
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        HookHelper.OnHookHitGround += HookHitGround;
        HookHelper.OnHookHitHazard += HookHitHazard;

        controls.OneHook.HoriztonalAxis.performed += HandleHorizontalAxis;
        controls.OneHook.VerticalAxis.performed += HandleVerticalaxis;
        controls.OneHook.Jump.performed += HandleJump;
        controls.OneHook.Jump.canceled += HandleJump;
        controls.OneHook.Fire.performed += HandleFire;
        controls.OneHook.Reel.performed += HandleReel;

        controls.OneHook.HoriztonalAxis.Enable();
        controls.OneHook.VerticalAxis.Enable();
        controls.OneHook.Jump.Enable();
        controls.OneHook.Fire.Enable();
        controls.OneHook.Reel.Enable();
    }

    private void OnDisable()
    {
        HookHelper.OnHookHitGround -= HookHitGround;
        HookHelper.OnHookHitHazard -= HookHitHazard;

        controls.OneHook.HoriztonalAxis.performed -= HandleHorizontalAxis;
        controls.OneHook.VerticalAxis.performed -= HandleVerticalaxis;
        controls.OneHook.Jump.performed -= HandleJump;
        controls.OneHook.Jump.canceled -= HandleJump;
        controls.OneHook.Fire.performed -= HandleFire;
        controls.OneHook.Reel.performed -= HandleReel;

        controls.OneHook.HoriztonalAxis.Disable();
        controls.OneHook.VerticalAxis.Disable();
        controls.OneHook.Jump.Disable();
        controls.OneHook.Fire.Disable();
        controls.OneHook.Reel.Disable();
    }

    private void Start()
    {
        curHorSpeed = 0.0f;
    }
    #endregion

    #region Input Handlers
    private void HandleHorizontalAxis(InputAction.CallbackContext obj)
    {
        curHorInput = obj.ReadValue<float>();

        if (Mathf.Abs(curHorInput) < deadZoneValue) //deadzone check
        {
            curHorInput = 0;
        }
    }

    private void HandleVerticalaxis(InputAction.CallbackContext obj)
    {
        curVerInput = obj.ReadValue<float>();
    }

    private void HandleReel(InputAction.CallbackContext obj)
    {
        reelRightHook = obj.ReadValue<float>();
    }

    private void HandleFire(InputAction.CallbackContext obj)
    {
        hookR_controller.FireHook(SnapOctDirection(curHorInput, curVerInput));
    }

    private void HandleJump(InputAction.CallbackContext obj)
    {
        jumpCancelling = obj.canceled;

        if (IsHooked())
        {
            if (!jumpQueued)
            {
                jumpQueued = obj.performed;
            }
        }
        else if (isWallSliding)
        {
            if (!wallJumpQueued)
            {
                wallJumpQueued = obj.performed;
            }
        }
        else
        {
            if (obj.canceled && !isAirJumping)
            {
                if (rb.velocity.y > jumpCutoff)
                {
                    rb.gravityScale = rbDefaultGravityScale * jumpCancelGravityMult; // need to put in a check, if the player is falling faster than a 'max fall speed', change gravity scale back to default
                }
            }

        }

        // Hang time
        if (hangTimer > 0.0f)
        {
            if (!jumpQueued)
            {
                jumpQueued = obj.performed;
            }
        }

        // Jump buffering
        if (obj.performed && !IsHooked() && !IsGrounded())
        {
            //Debug.Log("Jump Buffered");
            StartCoroutine(JumpBufferTimer(jumpBufferTime));
            StartCoroutine(WallJumpBufferTimer(wallJumpBufferTime));
        }

    }
    #endregion

    #region Buffers
    public IEnumerator JumpBufferTimer(float bufferTime)
    {
        jumpBuffered = true;
        yield return new WaitForSeconds(bufferTime);
        jumpBuffered = false;
    }

    public IEnumerator WallJumpBufferTimer(float bufferTime)
    {
        wallJumpBuffered = true;
        yield return new WaitForSeconds(bufferTime);
        wallJumpBuffered = false;
    }

    public IEnumerator DisableAirMovement(float disableTime)
    {
        allowAirMovement = false;
        yield return new WaitForSeconds(disableTime);
        allowAirMovement = true;
    }
    #endregion

    private void Update()
    {
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
            wallJumpBuffered = false;
            isOnWall = false;
            hasWallBehind = false;

            hangTimer = hangTime;
        }
        else
        {
            hangTimer -= Time.deltaTime;

            if (IsHooked())
            {
                // Set defaults
                rb.gravityScale = rbDefaultGravityScale;

                curHorSpeed = rb.velocity.x;
                hookSwingToApply = curHorInput * horHookMoveMult;

                isWallSliding = false;
                isAirJumping = false;
                wallJumpBuffered = false;
                isOnWall = false;
                hasWallBehind = false;
            } 
            else
            {
                // Check for pressing against wall or if still on wall
                if (curHorInput != 0 || isOnWall)
                {
                    float distance = 0.03f;
                    Vector3 castOrigin = bottomCollider.bounds.center;
                    Vector2 castSize = new Vector2(bottomCollider.radius * 2f, bottomCollider.radius * 0.6f);
                    RaycastHit2D raycastHit = Physics2D.BoxCast(castOrigin, castSize, 0f, new Vector2((int)directionFacing, 0f), distance, groundLayer);
                    RaycastHit2D raycastHitBehind = Physics2D.BoxCast(castOrigin, castSize, 0f, new Vector2((int)directionFacing * -1, 0f), distance, groundLayer);
                    
                    isOnWall = raycastHit.collider != null;
                    hasWallBehind = raycastHitBehind.collider != null;
                    if (isOnWall)
                    {
                        wallSide = (int)directionFacing;
                        if (wallJumpBuffered)
                        {
                            wallJumpQueued = true;
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
                //jumpQueued = jumpInput;
            }
        }

        // Jump cancelling
        if (!IsHooked() && !isWallSliding && jumpCancelling)
        {
            rb.gravityScale = rbDefaultGravityScale * jumpCancelGravityMult; // need to put in a check, if the player is falling faster than a 'max fall speed', change gravity scale back to default
        }

        // Check for buffered jump
        if (jumpBuffered && EvaluateHookState() == (int)HookedState.None && IsGrounded())
        {
            jumpQueued = true;
        }

        ControlHooks();
        watchthis = rb.velocity;
    }

    private bool IsGrounded()
    {
        float extraHeight = 0.03f;
        Vector3 castOrigin = bottomCollider.bounds.center;
        Vector2 castSize = new Vector2(bottomCollider.radius * 2, bottomCollider.radius * 2);
        RaycastHit2D raycastHit = Physics2D.BoxCast(castOrigin, castSize, 0f, Vector2.down, extraHeight, groundLayer);

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

        hookR_controller.ReelHook(reelRightHook);
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
                //HandleWallSliding();
            }
            else
            {
                if (allowAirMovement)
                {
                    ApplyHorizontalAirMovement();
                }
                else
                {
                    curHorSpeed = rb.velocity.x;
                }
            }
        }
        else
        {
            ApplyGroundMovement();
        }

        HandleWallSliding();
        JumpLogic();
    }

    private void HandleWallSliding()
    {
        //check for wall in the direction the player is currently moving
        //if player collides with that wall, set wall sliding to true
        //To make sure the player doesn't get stuck on small blocks
        if (rb.velocity.y < 2.0f && isOnWall && curHorInput != 0)
        {
            isWallSliding = true;
            //Using this to correct for oddities with adjusting the gravity scale
            rb.gravityScale = rbDefaultGravityScale;
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(0, wallSlideSpeed);
            curHorSpeed = 0;
        }

        // Slide/Step off the wall
        if (hasWallBehind && curHorInput * wallSide < 0)
        {
            curHorSpeed = -wallSide * wallStepOffSpeed;
            hasWallBehind = false;
            //isWallSliding = false;
            rb.gravityScale = rbDefaultGravityScale;
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
            jumpBuffered = false;
            hangTimer = 0.0f;
            directionWhenJumpStarted = directionFacing;

            rb.velocity = new Vector2(rb.velocity.x, 0f);
            ApplyJump(0, jumpForce);
        }
        // Hook Jump
        else if (jumpQueued && EvaluateHookState() == (int)HookedState.One)
        {
            // Check if next to wall
            float distance = 0.03f;
            Vector3 castOrigin = bottomCollider.bounds.center;
            Vector2 castSize = new Vector2(bottomCollider.radius * 2f, bottomCollider.radius * 0.6f);
            RaycastHit2D raycastHit = Physics2D.BoxCast(castOrigin, castSize, 0f, new Vector2((int)directionFacing, 0f), distance, groundLayer);

            if (raycastHit.collider == null)
            {
                //Debug.Log("Hook Jump!");
                jumpQueued = false;
                DetachHook.Invoke();

                isAirJumping = true;
                directionWhenJumpStarted = directionFacing;

                rb.velocity = new Vector2(rb.velocity.x * hookJumpHorizMult, 0f);
                ApplyJump(0, hookJumpForceMult);
            }
            else
            {
                wallJumpQueued = true;
            }
        }

        // Wall Jump
        if (wallJumpQueued)
        {
            // Temporarily disable air movement
            StartCoroutine(DisableAirMovement(wallJumpAirDisableTime));
            wallJumpQueued = false;
            wallJumpBuffered = false;
            directionWhenJumpStarted = directionFacing;

            rb.velocity = new Vector2(0f, 0f);
            ApplyJump(-wallSide * wallJumpOffForce, jumpForce);
            isWallSliding = false;
            rb.gravityScale = rbDefaultGravityScale;
        }
    }

    private void ApplyJump(float xforce, float yforce)
    {
        rb.velocity += new Vector2(xforce, yforce);
    }
    #endregion

    #region Hook Methods and Enums

    private void HookHitGround(HookSide hookSide)
    {
        // Cancel any jump-cancel that might have been happening
        isAirJumping = false;

        if (hookSide == HookSide.Right)
        {
            hookR_controller.ChangeHookConnectedState(true);
            hookR_connected = true;
        }
    }

    private void HookHitHazard(HookSide hookSide)
    {
        DetachHook.Invoke();
    }

    public int EvaluateHookState()
    {
        if (hookR_controller.h_onGround)
        {
            return 1;
        }

        return 0;
    }

    private bool IsHooked()
    {
        return (hookR_connected);
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
            bigFont.normal.textColor = Color.white;

            GUI.Label(new Rect(100, 200, 1000, 1000),
                  "OnGround? " + IsGrounded() +
                "\nIsHooked? " + IsHooked() +
                "\nW. Slide? " + isWallSliding +
                //"\nWJmpBuff? " + jumpBuffered +
                "\nIsOnWall? " + isOnWall +
                "\nSpeed w wall? " + (curHorInput * wallSide) +
                "\nHasWallB? " + hasWallBehind
                //"\nJumpBuff? " + jumpBuffered +
                //"\ncurHorSpeed? " + curHorSpeed +
                //"\ncurHorInput? " + curHorInput
                //"\nHookR_Connected: " + hookR_connected
                ,
                bigFont);
        }
    }

    #endregion

}
