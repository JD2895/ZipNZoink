using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    /*** HOOK DATA ***/
    // Setup variables
    private GameObject h_Object;
    private float maxReelSpeed;
    private float timeToMaxReelSpeed;
    private float inputReelMinimum;
    // Physics
    private DistanceJoint2D h_Joint;
    private float reelPerSec;
    private float reelToApply;
    // Line drawing
    private GameObject h_LineContainer;
    private LineRenderer h_Line;
    // Hook helpers
    private bool h_out = false;
    private bool h_onGround = false;

    private void FixedUpdate()
    {
        if(h_onGround)
        {
            // Reduce joint length by reel amount
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

    public void SetupHook(GameObject newHookObject, float newInputReelMin, float newMaxReelSpeed, float newTimeToMaxReelSpeed, GameObject parentObject)
    {
        // Variables
        h_Object = newHookObject;
        inputReelMinimum = newInputReelMin;
        maxReelSpeed = newMaxReelSpeed;
        timeToMaxReelSpeed = newTimeToMaxReelSpeed;
        // Line (drawing) setup
        h_LineContainer = new GameObject("HookLine");
        h_LineContainer.transform.parent = parentObject.transform;
        h_Line = h_LineContainer.AddComponent<LineRenderer>();
        h_Line.widthMultiplier = 0.1f;
        h_Line.positionCount = 2;
        // Joint setup
        h_Joint = parentObject.AddComponent<DistanceJoint2D>();
        h_Joint.enabled = false;
        h_Joint.autoConfigureDistance = false;
        h_Joint.connectedBody = h_Object.GetComponent<Rigidbody2D>();
        // Hook movement setup
        reelPerSec = maxReelSpeed / timeToMaxReelSpeed;
        // Other
        h_Object.SetActive(false);
    }

    public void FireHook(Vector2 firingDirection)
    {
        if (!h_out)
        {   // If the hook isn't out yet, activate the hook and fire it
            h_out = true;
            h_Object.SetActive(true);
            h_Object.GetComponent<HookHelper>().FireHook(this.transform.position, firingDirection);
        } 
        else if (h_out)
        {   // If the hook is already out
            DisconnectHook();
        }
    }

    private void DisconnectHook()
    {
        h_out = false;
        ChangeHookConnectedState(false);
        h_Object.SetActive(false);
        h_onGround = false;
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
}
