using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HookHelper : MonoBehaviour
{
    public static event Action<HookSide> OnHookHitGround;
    public float firingSpeed;
    public HookSide hookSide;
    public Rigidbody2D rb;

    private bool hitGround = false;
    private bool firing = false;
    private Vector2 nextPosition;
    private Vector2 firingDirection = new Vector2();

    #region Platform Interaction Variables

    private GameObject collisionObj;

    private Vector3 targetLastPos;
    private Vector3 hookOffset;
    
    private bool hookAttached;
    private bool targetMoving;
    #endregion

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
    }

    private void AlignHookWithGround(Vector3 collisionPos)
    {
        if (collisionPos != targetLastPos)
        {
            rb.MovePosition(collisionPos + hookOffset);
        }

        targetLastPos = collisionPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !hookAttached)
        {
            //Debug.Log("Hook Hit Ground");
            hitGround = true;
            firing = false;
            hookAttached = true;

            collisionObj = collision.gameObject;
            targetLastPos = collision.transform.position;
            hookOffset = this.transform.position - targetLastPos;
            

            rb.bodyType = RigidbodyType2D.Kinematic;    // Fixes the hook in place
            OnHookHitGround?.Invoke(hookSide);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && hookAttached)
        {
            AlignHookWithGround(collision.transform.position);
        }
    }

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

}
