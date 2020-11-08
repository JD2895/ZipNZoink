using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugUIController : MonoBehaviour
{
    public static DebugUIController instance;

    public HookFireVariant hookFireVar = HookFireVariant.Hold;
    public bool hookJump = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }

        DebugOptions.hookFireVarient = hookFireVar;
        DebugOptions.hookJump = hookJump;
    }

    public void UpdateDebugOptions()
    {
        DebugOptions.hookFireVarient = hookFireVar;
        DebugOptions.hookJump = hookJump;
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion
}
