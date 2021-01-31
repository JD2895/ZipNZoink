using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookGuides : MonoBehaviour
{
    #region Public Variables

    public LayerMask ground;
    
    public GameObject HookR;
    public GameObject HookL;

    public Material LaserMat;
    #endregion

    #region Private Variables

    private GameObject laserR;
    private GameObject laserL;

    private LineRenderer guideR;
    private LineRenderer guideL;
    #endregion

    #region Variable Initialization & LineRenderer Setup

    private void Awake()
    {        
        laserR = new GameObject("LaserR");
        laserL = new GameObject("LaserL");

        laserR.transform.SetParent(transform);
        laserL.transform.SetParent(transform);

        guideR = laserR.AddComponent<LineRenderer>();
        guideL = laserL.AddComponent<LineRenderer>();

    }

    private void Start()
    {
        laserR.transform.localPosition = Vector3.zero;
        laserL.transform.localPosition = Vector3.zero;

        GuideSetup(guideR);
        GuideSetup(guideL);
    }
    
    private void GuideSetup(LineRenderer guide)
    {
        guide.startWidth = 0.075f;
        guide.endWidth = 0.075f;

        guide.material = LaserMat;

        guide.enabled = false;
    }
    #endregion

    private void Update()
    {
        DrawHookGuide(HookR.activeSelf, HookL.activeSelf);
    }
    
    private void DrawHookGuide(bool hookR_Out, bool hookL_Out)
    {

        if (!hookR_Out)
        {
            if (!guideR.enabled) guideR.enabled = true;

            guideR.SetPosition(0, transform.position);
            guideR.SetPosition(1, DetectEndPoint(1) );

        }
        else if (hookR_Out) guideR.enabled = false;

        if(!hookL_Out)
        {
            if (!guideL.enabled) guideL.enabled = true;

            guideL.SetPosition(0, transform.position);
            guideL.SetPosition(1, DetectEndPoint(-1) );

        } else if (hookL_Out) guideL.enabled = false;
    }

    private Vector2 DetectEndPoint(int direction)
    {
        Vector2 defaultLine = transform.position + (transform.up + transform.right * direction) * 15f;

        RaycastHit2D ray = Physics2D.Raycast(transform.position, (transform.up + transform.right * direction), 100f, ground);

        if (ray.collider == null)
        {
            return defaultLine;

        }
        else if (ray.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            return new Vector2(transform.position.x + ray.distance * Mathf.Sin(Mathf.PI / 4) * direction, transform.position.y + ray.distance * Mathf.Cos(Mathf.PI / 4));
        }
        else return defaultLine;
    }
}
