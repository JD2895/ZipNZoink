using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_v3 : MonoBehaviour
{
    /*** HOOK DATA ***/
    public GameObject hookR_Object;
    public GameObject hookL_Object;
    public float inputReelMinimum;
    public float maxReelSpeed;
    public float timeToMaxReelSpeed;
    private HookController hookR_Controller;
    private HookController hookL_Controller;

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
        hookR_Controller = this.gameObject.AddComponent<HookController>();
        hookR_Controller.SetupHook(hookR_Object, inputReelMinimum, maxReelSpeed, timeToMaxReelSpeed, this.gameObject);
        hookL_Controller = this.gameObject.AddComponent<HookController>();
        hookL_Controller.SetupHook(hookL_Object, inputReelMinimum, maxReelSpeed, timeToMaxReelSpeed, this.gameObject);
    }

    private void Update()
    {
        // Check input
        curHorInput = Input.GetAxis("Horizontal");
        fireRightHook = Input.GetButtonDown("Right Hook Fire");
        reelRightHook = Input.GetAxis("Right Hook Reel");
        fireLeftHook = Input.GetButtonDown("Left Hook Fire");
        reelLeftHook = Input.GetAxis("Left Hook Reel");

        // Right hook control
        if (fireRightHook)
            hookR_Controller.FireHook(new Vector2(1, 1));
        hookR_Controller.ReelHook(reelRightHook);

        // Left hook control
        if (fireLeftHook)
            hookL_Controller.FireHook(new Vector2(-1, 1));
        hookL_Controller.ReelHook(reelLeftHook);
    }

    private void HookHitGround(HookSide hookSide)
    {
        if (hookSide == HookSide.Right)
            hookR_Controller.ChangeHookConnectedState(true);
        else if (hookSide == HookSide.Left)
            hookL_Controller.ChangeHookConnectedState(true);
    }
}
