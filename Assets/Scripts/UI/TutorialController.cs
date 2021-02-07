using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialController : MonoBehaviour
{
    public UIoptions movementButton;
    public UIoptions jumpButton;
    public UIoptions aimButton;
    public UIoptions fireButton;
    public UIoptions reelButton;

    private int moveTutorialSatus = 0;
    private int jumpTutorialSatus = 0;
    private int aimTutorialSatus = 0;
    private int fireTutorialSatus = 0;
    private int unhookTutorialSatus = 0;
    private int reelTutorialSatus = 0;
    private int hookJumpTutorialSatus = 0;

    public TextMeshProUGUI instructionText;
    public Transform animationPosition;

    private bool controllerVersion = true;

    public PlayerMovement_OneHook playermovementController;

    PlayerControls controls;
    float horizontalInput;
    float verticalInput;

    private void Awake()
    {
        // New input system
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.OneHook.HoriztonalAxis.performed += HandleHorizontalAxis;
        controls.OneHook.VerticalAxis.performed += HandleVerticalaxis;
        controls.OneHook.Jump.performed += HandleJump;
        controls.OneHook.Jump.canceled += HandleJump;
        controls.OneHook.Fire.performed += HandleFire;
        controls.OneHook.Reel.performed += HandleReel;

        controls.OneHook.HoriztonalAxis.Enable();
        controls.OneHook.VerticalAxis.Enable();
        controls.OneHook.Jump.Enable();
        controls.OneHook.Fire.Enable();
        controls.OneHook.Reel.Enable();
    }

    private void OnDisable()
    {
        controls.OneHook.HoriztonalAxis.performed -= HandleHorizontalAxis;
        controls.OneHook.VerticalAxis.performed -= HandleVerticalaxis;
        controls.OneHook.Jump.performed -= HandleJump;
        controls.OneHook.Jump.canceled -= HandleJump;
        controls.OneHook.Fire.performed -= HandleFire;
        controls.OneHook.Reel.performed -= HandleReel;

        controls.OneHook.HoriztonalAxis.Disable();
        controls.OneHook.VerticalAxis.Disable();
        controls.OneHook.Jump.Disable();
        controls.OneHook.Fire.Disable();
        controls.OneHook.Reel.Disable();
    }

    private void HandleHorizontalAxis(InputAction.CallbackContext obj)
    {
        horizontalInput = obj.ReadValue<float>();
        if (moveTutorialSatus == 1 && Mathf.Abs(horizontalInput) > 0.5f)
            moveTutorialSatus = 2;
        if (aimTutorialSatus == 1 && obj.ReadValue<float>() > 0.5f)
            aimTutorialSatus = 2;
    }

    private void HandleVerticalaxis(InputAction.CallbackContext obj)
    {
        verticalInput = obj.ReadValue<float>();
        if (aimTutorialSatus == 2 && Mathf.Abs(verticalInput) > 0.5f)
            aimTutorialSatus = 3;
    }

    private void HandleReel(InputAction.CallbackContext obj)
    {
        if (reelTutorialSatus == 1 && playermovementController.EvaluateHookState() == 1)
            reelTutorialSatus = 2;
    }

    private void HandleFire(InputAction.CallbackContext obj)
    {
        if (Mathf.Abs(horizontalInput) > 0.2f || Mathf.Abs(verticalInput) > 0.2f)
        {
            if (fireTutorialSatus == 1)
                fireTutorialSatus = 2;
        }
        if (unhookTutorialSatus == 1 && playermovementController.EvaluateHookState() == 1)
            unhookTutorialSatus = 2;
    }

    private void HandleJump(InputAction.CallbackContext obj)
    {
        if (jumpTutorialSatus == 1)
            jumpTutorialSatus = 2;
        if (hookJumpTutorialSatus == 1 && playermovementController.EvaluateHookState() == 1)
            hookJumpTutorialSatus = 2;
    }

    private void Start()
    {
        StartCoroutine(StartTutorial());
    }

    [System.Serializable]
    public struct UIoptions
    {
        public GameObject controllerVersion;
        public GameObject keyboardVersion;
    }

    private IEnumerator StartTutorial()
    {
        moveTutorialSatus = 1;
        yield return StartCoroutine(MoveTutorial());

        jumpTutorialSatus = 1;
        yield return StartCoroutine(JumpTutorial());

        aimTutorialSatus = 1;
        yield return StartCoroutine(AimTutorial());

        fireTutorialSatus = 1;
        yield return StartCoroutine(FireTutorial());

        unhookTutorialSatus = 1;
        yield return StartCoroutine(UnhookTutorial());

        reelTutorialSatus = 1;
        yield return StartCoroutine(ReelTutorial());

        hookJumpTutorialSatus = 1;
        yield return StartCoroutine(HookJumpTutorial());
    }

    private IEnumerator MoveTutorial()
    {
        instructionText.text = "MOVE";
        GameObject animationObject = Instantiate(movementButton.controllerVersion, animationPosition.parent);
        animationObject.transform.position = animationPosition.position;

        while (moveTutorialSatus == 1)
        {
            yield return null;
        }

        Destroy(animationObject);
    }

    private IEnumerator JumpTutorial()
    {
        instructionText.text = "JUMP";
        GameObject animationObject = Instantiate(jumpButton.controllerVersion, animationPosition.parent);
        animationObject.transform.position = animationPosition.position;

        while (jumpTutorialSatus == 1)
        {
            yield return null;
        }

        Destroy(animationObject);
    }

    private IEnumerator AimTutorial()
    {
        instructionText.text = "AIM";
        GameObject animationObject = Instantiate(aimButton.controllerVersion, animationPosition.parent);
        animationObject.transform.position = animationPosition.position;

        while (aimTutorialSatus < 3)
        {
            yield return null;
        }

        Destroy(animationObject);
    }

    private IEnumerator FireTutorial()
    {
        instructionText.text = "AIM + FIRE";
        GameObject animationObject = Instantiate(fireButton.controllerVersion, animationPosition.parent);
        animationObject.transform.position = animationPosition.position;

        while (fireTutorialSatus == 1)
        {
            yield return null;
        }

        Destroy(animationObject);
    }

    private IEnumerator UnhookTutorial()
    {
        instructionText.text = "UNHOOK";
        GameObject animationObject = Instantiate(fireButton.controllerVersion, animationPosition.parent);
        animationObject.transform.position = animationPosition.position;

        while (unhookTutorialSatus == 1)
        {
            yield return null;
        }

        Destroy(animationObject);
    }

    private IEnumerator ReelTutorial()
    {
        instructionText.text = "REEL";
        GameObject animationObject = Instantiate(reelButton.controllerVersion, animationPosition.parent);
        animationObject.transform.position = animationPosition.position;

        while (reelTutorialSatus == 1)
        {
            yield return null;
        }

        Destroy(animationObject);
    }

    private IEnumerator HookJumpTutorial()
    {
        instructionText.text = "HOOK JUMP";
        GameObject animationObject = Instantiate(jumpButton.controllerVersion, animationPosition.parent);
        animationObject.transform.position = animationPosition.position;

        while (hookJumpTutorialSatus == 1)
        {
            yield return null;
        }

        Destroy(animationObject);
    }
}
