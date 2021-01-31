using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_v2 : MonoBehaviour
{
    /*** HOOK DATA ***/
    public GameObject hr_Object;
    public GameObject hl_Object;
    // Physics
    private DistanceJoint2D hr_Joint;
    private DistanceJoint2D hl_Joint;
    public float maxReelSpeed;
    public float timeToMaxReelSpeed;
    private float reelPerSec;
    private float reelToApplyRight;
    private float reelToApplyLeft;
    // Line drawing
    private GameObject hr_LineContainer;
    private LineRenderer hr_Line;
    private GameObject hl_LineContainer;
    private LineRenderer hl_Line;
    // Hook helpers
    private bool hr_out = false;
    private bool hr_onGround = false;
    private float hr_reelValue = 0;
    private bool hl_out = false;
    private bool hl_onGround = false;
    private float hl_reelValue = 0;

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

    void Awake()
    {
        /*** HOOK SETUP ***/
        // Line (drawing) setup
        hr_LineContainer = new GameObject("RightHookLine");
        hr_LineContainer.transform.parent = transform;
        hr_Line = hr_LineContainer.AddComponent<LineRenderer>();
        hr_Line.widthMultiplier = 0.1f;
        hr_Line.positionCount = 2;
        // Joint setup
        hr_Joint = gameObject.AddComponent<DistanceJoint2D>();
        hr_Joint.enabled = false;
        hr_Joint.autoConfigureDistance = false;
        hr_Joint.connectedBody = hr_Object.GetComponent<Rigidbody2D>();
        // Hook movement setup
        reelPerSec = maxReelSpeed / timeToMaxReelSpeed;
        // Other
        hr_Object.SetActive(false);

        // Line (drawing) setup
        hl_LineContainer = new GameObject("LeftHookLine");
        hl_LineContainer.transform.parent = transform;
        hl_Line = hl_LineContainer.AddComponent<LineRenderer>();
        hl_Line.widthMultiplier = 0.1f;
        hl_Line.positionCount = 2;
        // Joint setup
        hl_Joint = gameObject.AddComponent<DistanceJoint2D>();
        hl_Joint.enabled = false;
        hl_Joint.autoConfigureDistance = false;
        hl_Joint.connectedBody = hl_Object.GetComponent<Rigidbody2D>();
        // Other
        hl_Object.SetActive(false);
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
        if (fireRightHook && !hr_out)
        {   // Fire hook
            FireHook(HookSide.Right);
        }
        else if (fireRightHook && hr_out)
        {   // Disconnect right hook
            hr_out = false;
            ChangeHookConnectedState(false, HookSide.Right);
            hr_Object.SetActive(false);
            hr_onGround = false;
        }

        if (hr_onGround && reelRightHook >= 0.05f)
        {
            hr_reelValue = reelRightHook;
        }
        else
        {
            reelToApplyRight = 0;
            hr_reelValue = 0;
        }
        
        // Left hook control
        if (fireLeftHook && !hl_out)
        {   // Fire hook
            FireHook(HookSide.Left);
        }
        else if (fireLeftHook && hl_out)
        {   // Disconnect hook
            hl_out = false;
            ChangeHookConnectedState(false, HookSide.Left);
            hl_Object.SetActive(false);
            hl_onGround = false;
        }

        if (hl_onGround && reelLeftHook >= 0.05f)
        {
            hl_reelValue = reelLeftHook;
        }
        else
        {
            reelToApplyLeft = 0;
            hl_reelValue = 0;
        }
    }

    private void FixedUpdate()
    {
        // Reel in
        if (hr_onGround && hr_reelValue > 0)
        {
            ReelHook(hr_reelValue, HookSide.Right);
        }
        if (hl_onGround && hl_reelValue > 0)
        {
            ReelHook(hl_reelValue, HookSide.Left);
        }
    }

    private void LateUpdate()
    {
        // Drawing Right Hook
        if (hr_out)
        {
            hr_Line.enabled = true;
            DrawHook(HookSide.Right);
        }
        else
        {
            hr_Line.enabled = false;
        }
        // Drawing Left Hook
        if (hl_out)
        {
            hl_Line.enabled = true;
            DrawHook(HookSide.Left);
        }
        else
        {
            hl_Line.enabled = false;
        }
    }

    private void DrawHook(HookSide hookside)
    {
        if (hookside == HookSide.Right)
        {
            hr_Line.SetPosition(0, this.transform.position);
            hr_Line.SetPosition(1, hr_Object.transform.position);
        }
        else if (hookside == HookSide.Left)
        {
            hl_Line.SetPosition(0, this.transform.position);
            hl_Line.SetPosition(1, hl_Object.transform.position);
        }
    }

    private void ReelHook (float inputReelValue, HookSide hookSide)
    {
        if (hookSide == HookSide.Right)
        {
            // Calculate amount to reel
            reelToApplyRight += reelPerSec;
            if (reelToApplyRight >= maxReelSpeed)
                reelToApplyRight = maxReelSpeed;
            // Reduce joint length by reel amount
            hr_Joint.distance -= reelToApplyRight * inputReelValue * Time.fixedDeltaTime;
            // This next bit is needed to prevent a bug that causes reeling to not work if the player collided with the ground while swinging
            hr_Joint.enabled = false;
            hr_Joint.enabled = true;
        }
        else if (hookSide == HookSide.Left)
        {
            // Calculate amount to reel
            reelToApplyLeft += reelPerSec;
            if (reelToApplyLeft >= maxReelSpeed)
                reelToApplyLeft = maxReelSpeed;
            // Reduce joint length by reel amount
            hl_Joint.distance -= reelToApplyLeft * inputReelValue * Time.fixedDeltaTime;
            // This next bit is needed to prevent a bug that causes reeling to not work if the player collided with the ground while swinging
            hl_Joint.enabled = false;
            hl_Joint.enabled = true;
        }
    }

    private void FireHook(HookSide hookSide)
    {
        if (hookSide == HookSide.Right)
        {
            // Activate the hook and fire it
            hr_out = true;
            hr_Object.SetActive(true);
            hr_Object.GetComponent<HookHelper>().FireHook(this.transform.position, new Vector2(1, 1));
        }
        else if (hookSide == HookSide.Left)
        {
            // Activate the hook and fire it
            hl_out = true;
            hl_Object.SetActive(true);
            hl_Object.GetComponent<HookHelper>().FireHook(this.transform.position, new Vector2(-1, 1));
        }
    }

    private void HookHitGround(HookSide hookSide)
    {
        if (hookSide == HookSide.Right)
        {
            ChangeHookConnectedState(true, HookSide.Right);
        }
        else if (hookSide == HookSide.Left)
        {
            ChangeHookConnectedState(true, HookSide.Left);
        }
    }

    private void ChangeHookConnectedState(bool toState, HookSide hookSide)
    {
        if (hookSide == HookSide.Right)
        {
            // Enable the joint and set its distance
            hr_Joint.enabled = toState;
            if (toState)
            {
                hr_onGround = true;
                float dist = Vector2.Distance(this.transform.position, hr_Object.transform.position);
                hr_Joint.distance = dist;
            }
        }
        else if (hookSide == HookSide.Left)
        {
            // Enable the joint and set its distance
            hl_Joint.enabled = toState;
            if (toState)
            {
                hl_onGround = true;
                float dist = Vector2.Distance(this.transform.position, hl_Object.transform.position);
                hl_Joint.distance = dist;
            }
        }
    }

}

public enum HookSide
{
    Right = 0,
    Left = 1
}