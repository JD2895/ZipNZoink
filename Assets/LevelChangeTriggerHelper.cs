using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChangeTriggerHelper : MonoBehaviour
{
    public LevelChangeTrigger lct;

    public void ChangeLevelHelper()
    {
        lct.ChangeLevel();
    }
}
