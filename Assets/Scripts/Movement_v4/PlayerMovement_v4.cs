using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_v4 : PhysicsObject
{
    public float maxSpeed = 6f;
    public float jumpTakeOffSpeed = 5f;

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        targetVelocity = move * maxSpeed;
    }
}
