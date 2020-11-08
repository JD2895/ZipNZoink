using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOptions
{
    public static HookFireVariant hookFireVarient;
    public static bool hookJump;

}

public enum HookFireVariant
{
    TwoPress = 0,
    Hold = 1,
    OnePress = 2
}
