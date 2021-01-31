using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private int reelTutorialSatus = 0;

    public TextMeshProUGUI instructionText;
    public Transform animationPosition;

    private bool controllerVersion = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 1
        if (moveTutorialSatus == 0)
        {
            moveTutorialSatus = 1;
            StartCoroutine(MoveTutorial());
        }

        // 2
        if (moveTutorialSatus == 2 && jumpTutorialSatus == 0)
        {
            jumpTutorialSatus = 1;
            StartCoroutine(JumpTutorial());
        }

        // 3
        if (jumpTutorialSatus == 2 && aimTutorialSatus == 0)
        {
            aimTutorialSatus = 1;
            StartCoroutine(AimTutorial());
        }

        // 4
        if (aimTutorialSatus == 2 && fireTutorialSatus == 0)
        {
            fireTutorialSatus = 1;
            StartCoroutine(FireTutorial());
        }

        // 5
        if (fireTutorialSatus == 2 && reelTutorialSatus == 0)
        {
            reelTutorialSatus = 1;
            StartCoroutine(ReelTutorial());
        }
    }

    [System.Serializable]
    public struct UIoptions
    {
        public GameObject controllerVersion;
        public GameObject keyboardVersion;
    }

    private IEnumerator MoveTutorial()
    {
        instructionText.text = "MOVE";
        GameObject animationObject = Instantiate(movementButton.controllerVersion, animationPosition.parent);
        animationObject.transform.position = animationPosition.position;

        while (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.7f)
        {
            yield return null;
        }

        moveTutorialSatus = 2;
        Destroy(animationObject);
    }

    private IEnumerator JumpTutorial()
    {
        instructionText.text = "JUMP";
        GameObject animationObject = Instantiate(jumpButton.controllerVersion, animationPosition.parent);
        animationObject.transform.position = animationPosition.position;

        while (!Input.GetButtonDown("Jump"))
        {
            yield return null;
        }

        jumpTutorialSatus = 2;
        Destroy(animationObject);
    }

    private IEnumerator AimTutorial()
    {
        instructionText.text = "AIM";
        GameObject animationObject = Instantiate(aimButton.controllerVersion, animationPosition.parent);
        animationObject.transform.position = animationPosition.position;

        bool bothPressed = false;

        while (!bothPressed)
        {
            if ((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.4f) && (Mathf.Abs(Input.GetAxis("Vertical")) > 0.4f))
                bothPressed = true;
            yield return null;
        }

        aimTutorialSatus = 2;
        Destroy(animationObject);
    }

    private IEnumerator FireTutorial()
    {
        instructionText.text = "AIM + FIRE";
        GameObject animationObject = Instantiate(fireButton.controllerVersion, animationPosition.parent);
        animationObject.transform.position = animationPosition.position;

        while (!Input.GetButtonDown("Right Hook Fire"))
        {
            yield return null;
        }

        fireTutorialSatus = 2;
        Destroy(animationObject);
    }

    private IEnumerator ReelTutorial()
    {
        instructionText.text = "REEL";
        GameObject animationObject = Instantiate(reelButton.controllerVersion, animationPosition.parent);
        animationObject.transform.position = animationPosition.position;

        while (!Input.GetButtonDown("Right Hook Reel")) // Change to get axis?
        {
            yield return null;
        }

        reelTutorialSatus = 2;
        Destroy(animationObject);
    }
}
