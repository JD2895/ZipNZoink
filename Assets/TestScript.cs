using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerExit2D(Collider2D other)
    {
        Vector2 playerPosition = transform.InverseTransformPoint(other.transform.position);

        if(playerPosition.x > 0)
        {
            Debug.Log("Out");
        }
        else
        {
            Debug.Log("In");
        }
    }
}
