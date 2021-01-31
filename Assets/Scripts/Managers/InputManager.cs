using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InputManager : MonoBehaviour
{
    static Dictionary<string, KeyCode> keyboardMapping;
    static Dictionary<string, KeyCode> controllerMapping;

    static string[] keyMaps = new string[7]
    {
        // Movement
        "Left",
        "Right",
        "Up",
        "Down",
        // Actions
        "Jump",
        "Fire",
        "Reel"
    };

    static KeyCode[] defaultKeyboard = new KeyCode[7]
    {
        // Movement
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        // Actions
        KeyCode.Space,
        KeyCode.C,
        KeyCode.Z
    };

    static KeyCode[] defaultController = new KeyCode[7]
    {
        // Movement
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        // Actions
        KeyCode.JoystickButton0,
        KeyCode.JoystickButton5,
        KeyCode.JoystickButton4
    };

    static InputManager()
    {
        InitializeDictionary();
    }

    private static void InitializeDictionary()
    {
        // Keyboard
        keyboardMapping = new Dictionary<string, KeyCode>();
        for (int i = 0; i < keyMaps.Length; ++i)
        {
            keyboardMapping.Add(keyMaps[i], defaultKeyboard[i]);
        }

        // Controller
        controllerMapping = new Dictionary<string, KeyCode>();
        for (int i = 0; i < keyMaps.Length; ++i)
        {
            controllerMapping.Add(keyMaps[i], defaultKeyboard[i]);
        }
    }

    public static void SetKeyMap(string keyMap, KeyCode key)
    {
        if (!keyboardMapping.ContainsKey(keyMap))
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
        keyboardMapping[keyMap] = key;
    }

    public static bool GetKeyDown(string keyMap)
    {
        return Input.GetKeyDown(keyboardMapping[keyMap]);
    }
}
