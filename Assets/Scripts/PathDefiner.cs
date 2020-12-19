using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDefiner : MonoBehaviour
{
    public GameObject pathFollower;
    public GameObject positionListObject;
    public GameObject positionPrefab;

    public float speed;

    private int targetIndex = 0;
    private List<Transform> positionList = new List<Transform>();
    private Vector3 targetPosition;

    void Start()
    {
        if (positionList.Count < 2)
        {
            Debug.Break();
            throw new System.Exception("PathDefiner in '" + this.gameObject.name + "'position list needs at least 2 positions");
        }

        foreach (Transform child in positionListObject.transform)
        {
            positionList.Add(child);
        }

        pathFollower.transform.position = positionList[0].position;
        targetPosition = positionList[1].position;
        targetIndex = 1;
    }

    void Update()
    {
        Vector3 targetDirection = targetPosition - pathFollower.transform.position;

        // Figure out the next position
        Vector3 nextPosition = pathFollower.transform.position + (targetDirection.normalized * speed * Time.deltaTime);
        
        if ((targetPosition - nextPosition).normalized != targetDirection.normalized)
        {
            // The next position will put us ahead of the target, so set the next position as the target position instead
            nextPosition = targetPosition;
        }
        pathFollower.transform.position = nextPosition;

        if (nextPosition == targetPosition)
        {
            //start moving to next target
        }
    }
}
