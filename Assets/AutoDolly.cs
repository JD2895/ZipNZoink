using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//just a quick script to test dolly movement
public class AutoDolly : MonoBehaviour
{
     public Cinemachine.CinemachineVirtualCamera vCam;
    public float speed = 0.00025f;
    Cinemachine.CinemachineTrackedDolly dolly;
    float pathLength;
    // Start is called before the first frame update
    void Start()
    {
        dolly = vCam.GetCinemachineComponent<Cinemachine.CinemachineTrackedDolly>();
        pathLength = dolly.m_Path.PathLength;
    }

    // Update is called once per frame
    void Update()
    {
        dolly.m_PathPosition += speed;
    }
}
