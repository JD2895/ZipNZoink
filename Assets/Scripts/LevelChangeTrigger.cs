using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelChangeTrigger : MonoBehaviour
{
    public string nextLevel;

    void Start()
    {
        this.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.LoadLevel(nextLevel);
        }
    }
}
