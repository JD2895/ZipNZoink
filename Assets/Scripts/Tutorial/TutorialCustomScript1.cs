using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCustomScript1 : MonoBehaviour
{
    private bool setToNone = true;

    private void Update()
    {
        if (setToNone == true)
        {
            DebugOptions.hookFireVarient = HookFireVariant.None;
        }
    }
}
