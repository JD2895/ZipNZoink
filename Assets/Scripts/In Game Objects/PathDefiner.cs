using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDefiner : MonoBehaviour
{
    public GameObject pathFollower;
    public GameObject positionListObject;
    public GameObject positionPrefab;       // Only needed if we make this script fancy enough to include buttons that add these in the game

    public float speed = 0f;
    public bool reverse = false;
    private bool reverseHelper = false;

    private int targetIndex = 0;
    private List<Transform> positionList = new List<Transform>();
    private Vector3 targetPosition;

    void Start()
    {
        foreach (Transform child in positionListObject.transform)
        {
            positionList.Add(child);
            //child.gameObject.SetActive(false);
        }

        if (positionList.Count < 2)
        {
            Debug.Break();
            throw new System.Exception("PathDefiner in '" + this.gameObject.name + "'position list needs at least 2 positions");
        }

        pathFollower.transform.position = positionList[0].position;
        targetPosition = positionList[1].position;
        targetIndex = 1;
    }

    void Update()
    {
        // Get target direction
        targetPosition = positionList[targetIndex % positionList.Count].position;
        Vector3 targetDirection = targetPosition - pathFollower.transform.position;

        // Calculate next position
        Vector3 nextPosition = pathFollower.transform.position + (targetDirection.normalized * speed * Time.deltaTime);
        
        // Check if getting ahead of target
        if ((targetPosition - nextPosition).normalized != targetDirection.normalized)
        {
            // Set the next position as the target position instead
            nextPosition = targetPosition;
        }

        // Set position
        pathFollower.transform.position = nextPosition;

        // Get next target
        if (nextPosition == targetPosition)
        {
            if (reverse)
            {
                if (targetIndex == positionList.Count - 1)
                    reverseHelper = true;
                if (reverseHelper && targetIndex == 0)
                    reverseHelper = false;

                targetIndex = reverseHelper ? targetIndex - 1 : targetIndex + 1;
            }
            else
            {
                targetIndex++;
            }
        }
    }
}
