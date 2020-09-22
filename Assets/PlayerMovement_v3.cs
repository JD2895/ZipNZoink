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

    /*** MOVEMENT DATA ***/
    private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D bottomCollider;
    [SerializeField] private LayerMask groundLayer;
    public float horMoveMult;
    public float horAirMoveMult;
    private float horToApply;

    /*** INPUT VARS ***/
    private float curHorInput = 0;
    private bool fireRightHook = false;
    private float reelRightHook = 0;
    private bool fireLeftHook = false;
    private float reelLeftHook = 0;

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
        fireRightHook = Input.GetButtonDown("Right Hook Fire");
        reelRightHook = Input.GetAxis("Right Hook Reel");
        fireLeftHook = Input.GetButtonDown("Left Hook Fire");
        reelLeftHook = Input.GetAxis("Left Hook Reel");

        if (IsGrounded())
        {
            horToApply = curHorInput * horMoveMult;
        }
        else
        { 
            horToApply = curHorInput * horAirMoveMult;
        }

        // Right hook control
        if (fireRightHook)
            hookR_Controller.FireHook(new Vector2(1, 1));
        hookR_Controller.ReelHook(reelRightHook);

        // Left hook control
        if (fireLeftHook)
            hookL_Controller.FireHook(new Vector2(-1, 1));
        hookL_Controller.ReelHook(reelLeftHook);
    }

    private void FixedUpdate()
    {
        Vector2 forceToApply = new Vector2(horToApply, 0);
        rb.AddForce(forceToApply);
    }

    private void HookHitGround(HookSide hookSide)
    {
        if (hookSide == HookSide.Right)
            hookR_Controller.ChangeHookConnectedState(true);
        else if (hookSide == HookSide.Left)
            hookL_Controller.ChangeHookConnectedState(true);
    }

    private bool IsGrounded()
    {
        float extraHeight = 1f;
        //RaycastHit2D rayHit = Physics2D.CircleCast()
        RaycastHit2D raycastHit = Physics2D.CircleCast(bottomCollider.bounds.center, bottomCollider.radius, Vector2.down, extraHeight, groundLayer);
        //if (raycastHit.collider != null)
        return raycastHit.collider != null;
    }
}
