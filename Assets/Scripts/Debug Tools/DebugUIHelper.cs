using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DebugUIHelper : MonoBehaviour
{
    public GameObject debugMenu;

    public Slider hookVariantSlider;
    public Toggle hookJumpToggle;
    public Toggle debugTextToggle;
    public Button restartButton;
    public List<string> listOfLevels;

    void Start()
    {
        // Initialisation
        DebugUIController.instance.UpdateDebugOptions();
        EventSystem.current.SetSelectedGameObject(restartButton.gameObject);

        // Set UI based on current session's values
        hookVariantSlider.normalizedValue = (float)DebugUIController.instance.hookFireVar - 0.5f;   // the 0.5 is needed otherwise it just rounds to the next highest/lowest value???
        hookJumpToggle.isOn = DebugUIController.instance.hookJump;
        debugTextToggle.isOn = DebugUIController.instance.debugText;

        debugMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Debug Reset"))
        {
            debugMenu.SetActive(!debugMenu.activeSelf);
            Time.timeScale = debugMenu.activeSelf ? 0f : 1f;

            // Highlight/Select when paused
            if (debugMenu.activeSelf)
            {
                GameObject tempSel = EventSystem.current.currentSelectedGameObject;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(tempSel);
            }
        }
    }

    #region UI FUNCTIONS

    public void ChangeHookFireVariant(float variantNumber)
    {
        DebugUIController.instance.hookFireVar = (HookFireVariant)variantNumber;

        if (variantNumber == 2f)
        {
            DebugUIController.instance.hookJump = true;
            hookJumpToggle.isOn = true;
        }
    }

    public void ChangeHookJumpEnabled(bool hookJumpNewState)
    {
        DebugUIController.instance.hookJump = hookJumpNewState;
    }

    public void ChangeDebugTextEnabled(bool debugTextNewState)
    {
        DebugUIController.instance.debugText = debugTextNewState;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        int nextIndex = listOfLevels.IndexOf(SceneManager.GetActiveScene().name) + 1;
        if (nextIndex >= listOfLevels.Count - 1)
            nextIndex = listOfLevels.Count - 1;
        SceneManager.LoadScene(listOfLevels[nextIndex]);
    }

    public void PrevLevel()
    {
        Time.timeScale = 1f;
        int prevIndex = listOfLevels.IndexOf(SceneManager.GetActiveScene().name) - 1;
        if (prevIndex <= 0)
            prevIndex = 0;
        SceneManager.LoadScene(listOfLevels[prevIndex]);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    #endregion
}
