using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
    public GameObject debugMenu;

    public HookFireVariant hookFireVar = HookFireVariant.Hold;
    public bool hookJump = true;

    private void Awake()
    {
        DebugOptions.hookFireVarient = hookFireVar;
        DebugOptions.hookJump = hookJump;
    }

    void Start()
    {
        debugMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Debug Reset"))
        {
            debugMenu.SetActive(!debugMenu.activeSelf);
        }
    }

    #region UI FUNCTIONS

    public void ChangeHookFireVariant(float variantNumber)
    {
        hookFireVar = (HookFireVariant)variantNumber;
        DebugOptions.hookFireVarient = hookFireVar;
    }

    public void ChangeHookJumpEnabled(bool hookJumpNewState)
    {
        hookJump = hookJumpNewState;
        DebugOptions.hookJump = hookJump;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion
}
