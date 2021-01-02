using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherEnemy : MonoBehaviour
{
    public float acceleration;
    public float maxForwardSpeed;
    public float rewindSpeed;
    public float checkDistance;

    public float recoveryTime;

    public LayerMask layermask;

    private Vector3 origPosition = new Vector3();
    private Vector3 nextPosition = new Vector3();
    private float currSpeed = 0f;

    private float recoveryStartTime = -1f;

    private DasherState currState = DasherState.Idle;
    private DasherState nextState = DasherState.Idle;

    void Start()
    {
        origPosition = transform.position;
        currState = DasherState.Idle;
    }

    void Update()
    {
        currState = nextState;
        switch (currState)
        {
            case DasherState.Idle:
                recoveryStartTime = -1f;
                currSpeed = 0f;
                RaycastHit2D raycast = Physics2D.Raycast(transform.position, -transform.up, checkDistance, layermask);
                if (raycast.collider.CompareTag("Player"))
                {
                    nextState = DasherState.Dashing;
                }
                break;
            case DasherState.Dashing:
                currSpeed = (currSpeed >= maxForwardSpeed) ? maxForwardSpeed : currSpeed + acceleration;
                nextPosition = transform.position + -transform.up * currSpeed * Time.deltaTime;
                transform.position = nextPosition;
                break;
            case DasherState.Recovery:
                if (Time.time - recoveryStartTime >= recoveryTime)
                    nextState = DasherState.Rewinding;
                break;
            case DasherState.Rewinding:
                nextPosition = transform.position + (origPosition - transform.position).normalized * rewindSpeed * Time.deltaTime;
                // Check if getting ahead of target
                if ((origPosition - nextPosition).normalized != (origPosition - transform.position).normalized)
                {
                    nextPosition = origPosition;
                    nextState = DasherState.Idle;
                }
                transform.position = nextPosition;
                break;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currState == DasherState.Dashing)
        {
            nextState = DasherState.Recovery;
            recoveryStartTime = Time.time;
        }
    }

    private enum DasherState
    {
        Idle,
        Dashing,
        Recovery,
        Rewinding
    }
}
