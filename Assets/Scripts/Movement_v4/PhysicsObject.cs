using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsObject : MonoBehaviour
{
    public float minGroundNormalY = 0.65f;
    public float gravityModifier = 1f;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFiler;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    
    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Set what the player collides with, base it on the objects layer as set in the inspector
        contactFiler.useTriggers = false;
        contactFiler.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFiler.useLayerMask = true;
    }

    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    private void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;  // Gravity
        velocity.x = targetVelocity.x;  // Horizontal movement

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime; // The amount of change in position

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x); // A vector at the bottom of the player pointing in the direction the player is facing

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false); // Applying x movement

        move = Vector2.up * deltaPosition.y;    // How much to move the player in the y direction

        Movement(move, true);   // Applying y movement
    }

    void Movement (Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        // Cutoff for checking for collisions
        if (distance > minMoveDistance)
        {
            // Cast any colliders connected to the rigidbody, collect any collisions detected
            int count = rb2d.Cast(move, contactFiler, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
                hitBufferList.Add(hitBuffer[i]);

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                // Compare normals to check if the player should be 'on' the object (eg. should be 'on' the ground, but not 'on' a wall)
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                // Compare velocity with the current normal (to stop the player from moving into objects)
                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    // Cancel out the part of velocity that would have been stopped by the collision
                    velocity = velocity - projection * currentNormal;
                }

                // modify distance as needed based on modified velocity
                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }
}
