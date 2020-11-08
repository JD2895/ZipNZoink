using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    public HookFireVariant hookFireVar = HookFireVariant.Hold;
    public bool hookJump = true;

    private void Awake()
    {
        DebugOptions.hookFireVarient = hookFireVar;
        DebugOptions.hookJump = hookJump;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHookFireVariant(int variantNumber)
    {
        hookFireVar = (HookFireVariant)variantNumber;
        DebugOptions.hookFireVarient = hookFireVar;
    }
}
