using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSM : MonoBehaviour
{
    private PlayerState _currentState = PlayerState.OnGround;

    void Update()
    {
        switch(_currentState)
        {
            case PlayerState.OnGround:
                {
                    // check if on ground, if yes then stay on ground
                    // -- if not, check wall, if yes then change to ground
                    // -- -- if not, check right hook
                    // -- -- also check left hook, change to relevant hook state
                    // -- -- -- if not, much just be in air
                    break;
                }
            case PlayerState.OnWall:
                {
                    break;
                }
        }
    }

}

public enum PlayerState
{
    OnGround = 0,
    InAir = 1,
    RightHook = 2,
    LeftHook = 3,
    BothHook = 4,
    OnWall = 5
}