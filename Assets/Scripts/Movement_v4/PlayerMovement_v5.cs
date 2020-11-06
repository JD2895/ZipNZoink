using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_v5 : MonoBehaviour
{

    public CharacterController2D charController;

    float horMove = 0f;
    float runSpeed = 40f;

    bool jump = false;

    void Start()
    {
        
    }

    void Update()
    {
        horMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("JUMP");
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        charController.Move(horMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
