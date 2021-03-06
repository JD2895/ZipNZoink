using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebugUIController : MonoBehaviour
{
    public static DebugUIController instance;
    public GameObject debugCanvasPrefab;
    public GameObject eventSystemPrefab;

    [Header("Default Values")]
    // Hook Fire variations
    [SerializeField] private HookFireVariant _hookFireVar = HookFireVariant.Hold;
    public HookFireVariant hookFireVar
    {
        get { return _hookFireVar; }
        set 
        { 
            _hookFireVar = value;
            DebugOptions.hookFireVarient = value;
        }
    }

    // Hook Jump availability
    [SerializeField] private bool _hookJump = true;
    public bool hookJump
    {
        get { return _hookJump; }
        set
        {
            _hookJump = value;
            DebugOptions.hookJump = value;
        }
    }

    // Show Debug Text
    [SerializeField] private bool _debugText = true;
    public bool debugText
    {
        get { return _debugText; }
        set
        {
            _debugText = value;
            DebugOptions.debugText = value;
        }
    }

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

        DebugOptions.hookFireVarient = _hookFireVar;
        DebugOptions.hookJump = _hookJump;
        DebugOptions.debugText = _debugText;

        //debugCanvasPrefab = GameObject.Find("Debug Canvas");
        if (GameObject.Find("EventSystem") == null)
        {
            GameObject.Instantiate(eventSystemPrefab);
        }
        if (GameObject.Find("Debug Canvas") == null)
        {
            GameObject.Instantiate(debugCanvasPrefab);
        }
    }

    public void UpdateDebugOptions()
    {
        DebugOptions.hookFireVarient = _hookFireVar;
        DebugOptions.hookJump = _hookJump;
        DebugOptions.debugText = _debugText;
    }
}
