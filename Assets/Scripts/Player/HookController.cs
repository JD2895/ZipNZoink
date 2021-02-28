using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    // Setup variables
    private GameObject h_Object;        // The hook 'head' that gets fired and is one end of the joint.
    private float maxReelSpeed;         // The maximum speed the player can reach while reeling in a connected hook.
    private float timeToMaxReelSpeed;   // The time it takes to reach the maximum reel speed.
    private float inputReelMinimum;     // The minimum input that needs to be registered before reeling happens (for controller trigger deadzones).
    private float minJointDistance;     // The smallest size the joint can get to.
    // Physics
    private DistanceJoint2D h_Joint;    // The hinge joint that is the basis of all the swing/reel mechanics.
    private float reelPerSec;           // The amount of 'reeling' to apply per second if reeling is happening. reelPerSec = maxReelSpeed / timeToMaxReelSpeed.
    private float reelToApply;          // Is used to keep track of how much the joint is going to be reducing in size.
    // Line drawing
    private GameObject h_LineContainer = null; // The gameobject that contains the line renderer for this hook
    private LineRenderer h_Line;        // The line renderer componenet for this hook
    // Hook helpers
    public bool h_out = false;         // Keeps track of if the hook is 'out' of the player
    public bool h_onGround = false;    // Keeps track of if the hook is currently connected to the ground

    // Particle system
    public ParticleSystem line_ps;

    void Start()
    {
        line_ps = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (h_onGround)
        {
            // Reduce joint length by reel amount
            float newJointDistance = h_Joint.distance - reelToApply * Time.fixedDeltaTime;
            if (newJointDistance <= minJointDistance)
                h_Joint.distance = minJointDistance;
            else
                h_Joint.distance -= reelToApply * Time.fixedDeltaTime;
            // This next bit is needed to prevent a bug that causes reeling to not work if the player collided with the ground while swinging
            h_Joint.enabled = false;
            h_Joint.enabled = true;
        }
    }

    private void LateUpdate()
    {
        // Drawing Hook
        if (h_out)
        {
            h_Line.enabled = true;
            DrawHook();
        }
        else
        {
            h_Line.enabled = false;
        }
    }

    public void SetupHook(GameObject newHookObject, HookControllerCommonSetup newHookCommonData)
    {
        // Variables
        h_Object = newHookObject;
        inputReelMinimum = newHookCommonData.inputReelMinimum;
        maxReelSpeed = newHookCommonData.maxReelSpeed;
        timeToMaxReelSpeed = newHookCommonData.timeToMaxReelSpeed;
        minJointDistance = newHookCommonData.minJointDistance;
        // Line (drawing) setup
        if (h_LineContainer == null)
        {
            h_LineContainer = new GameObject("HookLine");
            h_LineContainer.transform.parent = newHookCommonData.controllerParent.transform;
            h_Line = h_LineContainer.AddComponent<LineRenderer>();
        }
        else
        {
            h_Line = h_LineContainer.GetComponent<LineRenderer>();
        }
        h_Line.widthMultiplier = 0.1f;
        h_Line.positionCount = 2;
        h_Line.sortingOrder = -2;
        // Joint setup
        h_Joint = newHookCommonData.controllerParent.AddComponent<DistanceJoint2D>();   // Attach one end of the joint to the player.
        h_Joint.enabled = false;    // Disabled at start by default.
        h_Joint.autoConfigureDistance = false;  // Setting to false allows for changing distances.
        h_Joint.connectedBody = h_Object.GetComponent<Rigidbody2D>();               // Attach the other end of the joint to the hook head.
        h_Joint.maxDistanceOnly = true;     // The joint can be 'compressed' but not 'stretched'. Helps when more than one hook is in use.
        // Hook movement setup
        reelPerSec = maxReelSpeed / timeToMaxReelSpeed;
        // Other
        PlayerMovement_OneHook.DetachHook.AddListener(DisconnectHook);
        h_Object.SetActive(false);
    }

    public void FireHook(Vector2 firingDirection)
    {
        // Disconnect and fire are two button presses (also works for 'holding' the hook)
        if (DebugOptions.hookFireVarient == HookFireVariant.TwoPress || DebugOptions.hookFireVarient == HookFireVariant.Hold || DebugOptions.hookFireVarient == HookFireVariant.OneHook)
        {
            if (!h_out)
            {   // If the hook isn't out yet, activate the hook and fire it
                if (firingDirection != Vector2.zero)
                {
                    h_out = true;
                    h_Object.SetActive(true);
                    h_Object.GetComponent<HookHelper>().FireHook(this.transform.position, firingDirection);
                }
            }
            else if (h_out)
            {   // If the hook is already out
                DisconnectHook();
            }
        }

        // Disconnect and fire are one button press
        if (DebugOptions.hookFireVarient == HookFireVariant.OnePress)
        {
            DisconnectHook();
            h_out = true;
            h_Object.SetActive(true);
            h_Object.GetComponent<HookHelper>().FireHook(this.transform.position, firingDirection);
        }
    }

    public void DisconnectHook()
    {
        DisconnectParticleEffect();
        h_out = false;
        ChangeHookConnectedState(false);
        h_Object.SetActive(false);
        h_onGround = false;
    }

    private void DisconnectParticleEffect()
    {
        // Get info for changing shape
        Vector3 hereToHook = h_Object.transform.position - this.transform.position;
        float rotation = Vector3.SignedAngle(Vector3.up, hereToHook, Vector3.forward);
        float lineLength = Vector3.Distance(this.transform.position, h_Object.transform.position);

        // Change number of particles emitted based on length
        var em = line_ps.emission;
        em.SetBursts(
             new ParticleSystem.Burst[] {
                  new ParticleSystem.Burst (0.0f, lineLength * 8)
             });

        // Change shape
        var sh = line_ps.shape;
        sh.position = Vector3.zero + hereToHook.normalized * (lineLength / 2);
        sh.rotation = new Vector3(0, 0, rotation);
        sh.scale = new Vector3(0, lineLength, 0);

        line_ps.Play();
    }

    public void ChangeHookConnectedState(bool toState)
    {
        // Disable/Enable the joint and set its distance
        h_Joint.enabled = toState;
        if (toState)
        {
            h_onGround = true;
            float dist = Vector2.Distance(this.transform.position, h_Object.transform.position);
            h_Joint.distance = dist;
        }
    }

    public void ReelHook(float inputReelValue)
    {
        // Reel if the hook is on ground
        if (h_onGround && inputReelValue >= inputReelMinimum)
        {
            // Calculate amount to reel
            reelToApply += reelPerSec;
            if (reelToApply >= maxReelSpeed)
                reelToApply = maxReelSpeed;
            reelToApply *= inputReelValue;
        }
        else
        {
            reelToApply = 0;
        }
    }

    private void DrawHook()
    {
        h_Line.SetPosition(0, this.transform.position);
        h_Line.SetPosition(1, h_Object.transform.position);
    }

    public void SetLineContainer(GameObject lineContainer)
    {
        h_LineContainer = lineContainer;
    }
}

[Serializable]
public class HookControllerCommonSetup
{
    public float minJointDistance;      // The minimum distance the grapple can reel to //TODO: Implement this.
    public float inputReelMinimum;      // The minimum input that needs to be registered before reeling happens (for controller trigger deadzones).
    public float maxReelSpeed;          // The maximum speed the player can reach while reeling in a connected hook.
    public float timeToMaxReelSpeed;    // The time it takes to reach the maximum reel speed.
    public GameObject controllerParent; // The parent object that the hook joints are attached to (the hook heads are on the other end of the joint). This should be the player.
}


