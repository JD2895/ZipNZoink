using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    public GameObject previous;
    public GameObject next;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   /* void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("GameObject1 collided with " + col.name);
        previous.SetActive(false);
        next.SetActive(true);
        gameObject.SetActive(false);
    }*/

    void OnTriggerExit2D(Collider2D other)
    {
        Vector2 playerPosition = transform.InverseTransformPoint(other.transform.position);

        if (playerPosition.x > 0)
        {
            previous.SetActive(false);
            next.SetActive(true);
        }
        else
        {
            previous.SetActive(true);
            next.SetActive(false);
        }
    }
}
