using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HookHelper : MonoBehaviour
{
    public static event Action OnHookHitGround;
    public float firingSpeed;

    private Rigidbody2D rb;
    private bool hitGround = false;
    private bool firing = false;
    private HookDirection hookFiringDirection;
    private Vector2 nextPosition;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (firing && !hitGround)
        {
            nextPosition = this.transform.position;

            if (hookFiringDirection == HookDirection.Right)
            {   // Going right
                nextPosition += new Vector2(1, 1) * firingSpeed * Time.fixedDeltaTime;
            }
            else
            {   // Going left
                nextPosition += new Vector2(-1, 1) * firingSpeed * Time.fixedDeltaTime;
            }
            rb.MovePosition(nextPosition);
        }
    }

    public void FireHook(Vector2 startingPosition, HookDirection direction)
    {
        hookFiringDirection = direction;
        this.transform.position = startingPosition;
        firing = true;
        hitGround = false;
        if (direction == HookDirection.Right)
            this.transform.eulerAngles = new Vector3(0f, 0f, -45f);
        else
            this.transform.eulerAngles = new Vector3(0f, 0f, 45f);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public enum HookDirection
    {
        Right = 1,
        Left = -1
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            hitGround = true;
            firing = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            Debug.Log("hit ground");
            OnHookHitGround?.Invoke();
        }
    }
}
