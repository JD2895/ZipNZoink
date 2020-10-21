using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement_v3 : MonoBehaviour
{
    public static UnityEvent DetachHook; 
    
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
    private bool isAirJumping;
    private HoriDirection directionFacing;    // -1 is left, 1 is right
    private HoriDirection directionWhenJumpStarted;
    public float maxSpeed;
    public float jumpCutoff = 4.0f;

    public float accelerateValue;
    public float decelerateValue;

    public float airAccelerateValue;
    public float maxAirSpeed;

    public float currentSpeed;

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

        //playerSprite = this.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        halfSpeed = maxSpeed * 0.5f;
        currentSpeed = 0.0f;
    }
    #endregion

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

        if (IsGrounded())
        {
            isAirJumping = false;
            
            if (curHorInput == 0)
            {
                if (currentSpeed > 0)
                {
                    currentSpeed -= decelerateValue;
                    if (currentSpeed < 0)
                    {
                        currentSpeed = 0;
                    }
                }
                else if (currentSpeed < 0)
                {
                    currentSpeed += decelerateValue;
                    if (currentSpeed > 0)
                    {
                        currentSpeed = 0;
                    }
                }
            }
            else
            {
                currentSpeed += accelerateValue * curHorInput;
                if (Mathf.Abs(currentSpeed) > maxSpeed)
                {
                    currentSpeed = maxSpeed * curHorInput;
                }
            }

            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
            if (!jumpQueued)
            {
                jumpQueued = jumpInput;
            }
        }
        else
        {
            if (IsHooked())
            {
                currentSpeed = rb.velocity.x;
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
                        rb.velocity = new Vector2(rb.velocity.x, jumpCutoff);
                    }
                }
                currentSpeed += airAccelerateValue * curHorInput;
                if (Mathf.Abs(currentSpeed) > maxAirSpeed)
                {
                    currentSpeed = maxAirSpeed * curHorInput;
                }
                rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
                //may delete
                /*  if (curHorInput != 0 && rb.velocity.x * curHorInput < 0)
                  {
                      rb.velocity = new Vector2(0, rb.velocity.y);
                  }*/
            }
        }

        //Debug.Log(directionWhenJumpStarted);
        /* if (IsGrounded())
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
         }*/

        ControlHooks();
    }

    private bool IsGrounded()
    {
        float extraHeight = 0.05f;
        RaycastHit2D raycastHit = Physics2D.CircleCast(bottomCollider.bounds.center, bottomCollider.radius, Vector2.down, extraHeight, groundLayer);

        
        //One Way Platform Conditional: Make sure the player is ABOVE the platform before IsGrounded() is true
        if (raycastHit.collider != null && raycastHit.collider.TryGetComponent(out PlatformEffector2D platEffector))
        {
            if (platEffector.useOneWay && rb.velocity.y > 0.1f || this.transform.position.y - 0.3f < raycastHit.collider.bounds.max.y)
            {
                return false;
            }
        }

        //Debug.DrawLine(bottomCollider.bounds.center, bottomCollider.bounds.center + (Vector3.down * extraHeight), Color.red);
        return raycastHit.collider != null;
    }

    private void ControlHooks()
    {
        
        //Stephen: I think we should check the hook state using public bools from Hook Controller, rather than a local variable that gets toggled
        hookR_connected = hookR_controller.h_onGround;
        hookL_connected = hookL_controller.h_onGround;
        
        // Right hook control
        if (fireRightHook)
        {
            //hookR_Controller.FireHook(new Vector2(1, 1));
            hookR_controller.FireHook(transform.up + transform.right);
        }
        hookR_controller.ReelHook(reelRightHook);

        // Left hook control
        if (fireLeftHook)
        {
            //hookL_Controller.FireHook(new Vector2(-1, 1));
            hookL_controller.FireHook(transform.up + -transform.right);
        }
        hookL_controller.ReelHook(reelLeftHook);
    }

    private void FixedUpdate()
    {
        if(!IsGrounded())
        {
            Vector2 forceToApply = new Vector2(horiToApply * Time.fixedDeltaTime, 0);
            rb.AddForce(forceToApply);
        }

        JumpLogic();

        //    ApplyMovement();
    }

    #region FixedUpdate Movement Methods

    private void JumpLogic()
    {
       
        if (jumpQueued && IsGrounded())
        {
            jumpQueued = false;
            directionWhenJumpStarted = directionFacing;

            ApplyJump(0, jumpForceMult * Time.fixedDeltaTime);


        } else if (jumpQueued && EvaluateHookState() == (int)HookedState.One)
        {
            Debug.Log("Jump!");
            DetachHook.Invoke();

            isAirJumping = true;
            jumpQueued = false;
            directionWhenJumpStarted = directionFacing;

            rb.velocity = new Vector2(rb.velocity.x, 0f);
            ApplyJump(1.5f * rb.velocity.x * Time.fixedDeltaTime, 1.25f * jumpForceMult * Time.fixedDeltaTime);
            

        } else if (jumpQueued && EvaluateHookState() == (int)HookedState.Both)
        {
            //ADD "SUPER JUMP" HERE
            Debug.Log("Super Jump!");
            DetachHook.Invoke();

            isAirJumping = true;
            jumpQueued = false;

            rb.velocity = Vector2.zero;
            ApplyJump(0, jumpForceMult * 3 * Time.fixedDeltaTime);

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
            "\nHookR_Connected: " + hookR_connected,
            bigFont);
    }

}
