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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Debug.Log("Hook Hit Ground");
            hitGround = true;
            firing = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            OnHookHitGround?.Invoke(hookSide);
        }
    }
}
