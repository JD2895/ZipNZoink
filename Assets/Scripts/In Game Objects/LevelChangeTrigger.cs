using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelChangeTrigger : MonoBehaviour
{
    public string nextLevel;
    private bool atDoor;
    public GameObject interactPrompt;
    public Animator doorAnimator;

    private PlayerControls controls;

    private void Awake()
    {
        // New input system
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.OneHook.Interact.performed += HandleInteract;


        controls.OneHook.Interact.Enable();
    }

    private void OnDisable()
    {
        controls.OneHook.Interact.performed -= HandleInteract;
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

    public void HandleInteract(InputAction.CallbackContext obj)
    {
        if (atDoor)
        {
            doorAnimator.SetBool("DoorOpened", true);
            //ChangeLevel();
        }
    }

    public void ChangeLevel()
    {
        GameManager.instance.LoadLevel(nextLevel);
    }
}
