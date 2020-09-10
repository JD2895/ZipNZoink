using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_v2 : MonoBehaviour
{
    /*** HOOK DATA ***/
    public GameObject hookObject;
    // Physics
    private DistanceJoint2D hookJoint;
    public float maxReelSpeed;
    public float timeToMaxReelSpeed;
    private float reelPerSec;
    private float reelToApply;
    // Line drawing
    private GameObject hookLineContainer;
    private LineRenderer hookLine;
    // Hook helpers
    private bool h_out = false;
    private bool h_onGround = false;
    private float h_reelValue = 0;

    /*** INPUT VARS ***/
    private float curHorInput = 0;
    private bool fireHook = false;
    private float reelHook = 0;

    /*** HELPERS ***/

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

        /***** Initial setup *****/

        /*** HOOK SETUP ***/
        // Line (drawing) setup
        hookLineContainer = new GameObject("HookContainer");
        hookLineContainer.transform.parent = transform;
        hookLine = hookLineContainer.AddComponent<LineRenderer>();
        hookLine.widthMultiplier = 0.1f;
        hookLine.positionCount = 2;
        // Joint setup
        hookJoint = gameObject.AddComponent<DistanceJoint2D>();
        hookJoint.enabled = false;
        hookJoint.autoConfigureDistance = false;
        hookJoint.connectedBody = hookObject.GetComponent<Rigidbody2D>();
        // Hook movement setup
        reelPerSec = maxReelSpeed / timeToMaxReelSpeed;
    }

    private void Update()
    {
        // Check input
        curHorInput = Input.GetAxis("Horizontal");
        fireHook = Input.GetButtonDown("Right Hook Fire");
        reelHook = Input.GetAxis("Right Hook Reel");

        // Fire/Disconnect Hook
        if (fireHook && !h_out)
        {
            Debug.Log("Right hook fired");
            h_out = true;
            ChangeRightHookConnectedState(true);
        }
        else if (fireHook && h_out)
        {
            Debug.Log("Right hook disconnected");
            h_out = false;
            ChangeRightHookConnectedState(false);
        }

        if (h_out && reelHook >= 0.1f)    //TODO: replace with 'hookOnGround' once grappling hook firing is implemented
        {
            h_reelValue = reelHook;
        }
        else
        {
            reelToApply = 0;
            h_reelValue = 0;
        }
    }

    private void FixedUpdate()
    {
        if (h_out && h_reelValue > 0)
        {
            ReelRightHook(h_reelValue);
        }

    }

    private void LateUpdate()
    {
        // Drawing
        if (h_out)
        {
            hookLine.enabled = true;
            DrawHook(hookObject);
        }
        else
        {
            hookLine.enabled = false;
        }
    }

    private void DrawHook(GameObject hookPoint)
    {
        hookLine.SetPosition(0, this.transform.position);
        hookLine.SetPosition(1, hookPoint.transform.position);
    }

    private void ReelRightHook (float inputReelValue)
    {
        reelToApply += reelPerSec;
        if (reelToApply >= maxReelSpeed)
            reelToApply = maxReelSpeed;
        Debug.Log(reelToApply + " : " + reelToApply * Time.fixedDeltaTime);
        hookJoint.distance -= reelToApply * inputReelValue * Time.fixedDeltaTime;
        // This next bit is needed to prevent a bug that causes reeling to not work if the player collided with the ground while swinging
        hookJoint.enabled = false;
        hookJoint.enabled = true;
    }

    private void HookHitGround()
    {
        h_onGround = true;
    }

    private void ChangeRightHookConnectedState(bool toState)
    {
        hookJoint.enabled = toState;
        if (toState)
        {
            float dist = Vector2.Distance(this.transform.position, hookObject.transform.position);
            hookJoint.distance = dist;
            Debug.Log(dist);
        }
    }
}
