using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCustomScript2 : MonoBehaviour
{
    public TutorialCustomScript1 custTut;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            custTut.SetNoneState(false);
        }
    }
}
