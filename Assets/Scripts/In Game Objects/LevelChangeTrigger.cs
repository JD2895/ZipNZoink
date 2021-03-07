﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelChangeTrigger : MonoBehaviour
{
    public string nextLevel;
    public bool atDoor;
    public GameObject interactPrompt;

    private PlayerControls controls;

    private void Awake()
    {
        // New input system
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.OneHook.Interact.performed += TryChangelevel;
    }

    private void OnDisable()
    {
        controls.OneHook.Interact.performed -= TryChangelevel;
    }

    void Start()
    {
        this.GetComponent<BoxCollider2D>().isTrigger = true;
        interactPrompt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            atDoor = true;
            interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            atDoor = false;
            interactPrompt.SetActive(false);
        }
    }

    public void TryChangelevel(InputAction.CallbackContext obj)
    {
        if (atDoor)
        {
            GameManager.instance.LoadLevel(nextLevel);
        }
    }
}
