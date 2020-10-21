using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class PlatformPathing : MonoBehaviour
{
    public PathCreator path;

    [SerializeField]
    private float pathSpeed;
    private float distanceTravelled;

    // Start is called before the first frame update
    void Start()
    {
        //pathSpeed = 1f;
        distanceTravelled = 0f;

        transform.position = path.path.GetPoint(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowPath();

    }

    private void FollowPath()
    {
        distanceTravelled += pathSpeed * Time.fixedDeltaTime;
        transform.position = path.path.GetPointAtDistance(distanceTravelled);
    }

}