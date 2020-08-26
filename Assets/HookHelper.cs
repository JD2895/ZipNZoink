using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookHelper : MonoBehaviour
{
    public static event Action OnHookHitGround;

    private bool hitGround = false;

    void Update()
    {
        if (Input.GetButton("Right Hook Fire") && !hitGround)
        {
            hitGround = true;
            OnHookHitGround?.Invoke();
        }
    }
}
