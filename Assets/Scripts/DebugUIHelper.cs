using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DebugUIHelper : MonoBehaviour
{
    public GameObject debugMenu;

    public Slider hookVariantSlider;
    public Toggle hookJumpToggle;
    public Button restartButton;

    void Start()
    {
        restartButton.onClick.AddListener(DebugUIController.instance.RestartLevel);
        hookJumpToggle.onValueChanged.AddListener(DebugUIController.instance.ChangeHookJumpEnabled);
        hookVariantSlider.onValueChanged.AddListener(DebugUIController.instance.ChangeHookFireVariant);
        DebugUIController.instance.UpdateDebugOptions();

        hookJumpToggle.isOn = DebugUIController.instance.hookJump;
        hookVariantSlider.normalizedValue = (float)DebugUIController.instance.hookFireVar - 0.5f;   // the 0.5 is needed otherwise it just rounds to the next highest/lowest value???

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
}
