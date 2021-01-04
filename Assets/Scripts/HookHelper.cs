using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class HookHelper : MonoBehaviour
{
    public static event Action<HookSide> OnHookHitGround;
    public static event Action<HookSide> OnHookHitHazard;

    public float firingSpeed;
    public HookSide hookSide;
    public Rigidbody2D rb;

    private bool hitGround = false;
    private bool firing = false;
    private Vector2 nextPosition;
    private Vector2 firingDirection = new Vector2();
    private Transform origParent;

    #region Platform Interaction Variables

    private GameObject collisionObj;

    private Vector3 targetLastPos;
    private Vector3 hookOffset;

    private bool hookAttached;
    private bool targetMoving;
    #endregion

    private void Start()
    {
        origParent = this.transform.parent;
    }

    private void OnEnable()
    {
        if(origParent != null)
        {
            this.transform.SetParent(origParent);
        }
    }

    private void OnDisable()
    {
        hookAttached = false;
        firing = false;
        //collisionObj = null;
        rb.freezeRotation = false;
    }

    private void FixedUpdate()
    {    
        if (firing && !hitGround)
        {
            nextPosition = this.transform.position;
            nextPosition += firingDirection * firingSpeed * Time.fixedDeltaTime;
            rb.MovePosition(nextPosition);
        }
    }

    public void FireHook(Vector2 startingPosition, Vector2 directionToFire)
    {
        firingDirection = directionToFire.normalized;
        this.transform.position = startingPosition;
        firing = true;
        hitGround = false;
        this.transform.eulerAngles = new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up, firingDirection));
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.freezeRotation = true;
    }

    //Called while the Hook is touching a Ground collider
    //Moves the hook to match the position of the collider if the collider is moving
    private void AlignHookWithGround(Vector3 collisionPos)
    {
        if (collisionPos != targetLastPos)
        {
            transform.position = collisionPos + hookOffset;

            if (transform.position.sqrMagnitude - (collisionPos + hookOffset).sqrMagnitude > 0.001f) throw new Exception("WTF");
        }

        targetLastPos = collisionPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !hookAttached)
        {
            //Debug.Log("Hook Hit Ground");
            hitGround = true;
            firing = false;
            hookAttached = true;

            collisionObj = collision.gameObject;
            targetLastPos = collision.transform.position;
            hookOffset = transform.position - targetLastPos;

            transform.position = targetLastPos + hookOffset;

            this.transform.SetParent(collision.transform);

            rb.bodyType = RigidbodyType2D.Kinematic;    // Fixes the hook in place
            OnHookHitGround?.Invoke(hookSide);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Hazard") && !hookAttached)
        {
            //Debug.Log("Hook Hit Hazard");
            hitGround = false;
            firing = false;
            hookAttached = false;
            OnHookHitHazard?.Invoke(hookSide);
        }
    }

    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == collisionObj && hookAttached)
        {
            AlignHookWithGround(collision.transform.position);
        }
    }
    */

    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == collisionObj)
        {
            hookAttached = false;
            collisionObj = null;

            hookOffset = Vector3.zero;
            targetLastPos = Vector3.zero;
        }

    }
    */

    private GUIStyle bigFont = new GUIStyle();

    private void OnGUI()
    {
        if (DebugOptions.debugText)
        {
            bigFont.fontSize = 20;

            GUI.Label(new Rect(400, 200, 1000, 1000),
                "CollisionObj: " + collisionObj +
                "\ntargetLastPos: " + targetLastPos +
                "\nHookOffset: " + hookOffset +
                "\nLastPos + HO: " + (targetLastPos + hookOffset) +
                "\nPosition:     " + transform.position
                , bigFont);

        }
    }

}
