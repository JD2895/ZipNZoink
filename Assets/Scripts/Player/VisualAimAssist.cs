using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualAimAssist : MonoBehaviour
{
    public LayerMask stopAt;
    public Color rightColor;
    public Color leftColor;
    public Material lineMaterial;
    private float fadeDistance = 3.5f;
    private float lineWidth = 0.07f;
    private int layerOrder = -1;

    private LineRenderer rightLine;
    private LineRenderer leftLine;

    private Gradient rightGradient;
    private GradientColorKey[] rightColorKey;
    private Gradient leftGradient;
    private GradientColorKey[] leftColorKey;
    public GradientAlphaKey[] sharedAlphaKey;

    // Start is called before the first frame update
    void Start()
    {
        rightGradient = new Gradient();
        rightColorKey = new GradientColorKey[2];
        rightColorKey[0].color = rightColor;
        rightColorKey[0].time = 0f;
        rightColorKey[1].color = Color.clear;
        rightColorKey[1].time = fadeDistance;

        leftGradient = new Gradient();
        leftColorKey = new GradientColorKey[2];
        leftColorKey[0].color = leftColor;
        leftColorKey[0].time = 0f;
        leftColorKey[1].color = Color.clear;
        leftColorKey[1].time = fadeDistance;

        sharedAlphaKey = new GradientAlphaKey[2];
        sharedAlphaKey[0].alpha = 1f;
        sharedAlphaKey[0].time = 0f;
        sharedAlphaKey[1].alpha = 0f;
        sharedAlphaKey[1].time = fadeDistance;

        rightGradient.SetKeys(rightColorKey, sharedAlphaKey);
        leftGradient.SetKeys(leftColorKey, sharedAlphaKey);

        GameObject rightLineContainer = new GameObject();
        rightLineContainer.transform.SetParent(this.transform);
        rightLine = rightLineContainer.AddComponent<LineRenderer>();
        rightLine.material = lineMaterial;
        rightLine.colorGradient = rightGradient;
        rightLine.startWidth = rightLine.endWidth = lineWidth;
        rightLine.sortingOrder = layerOrder;

        GameObject leftLineContainer = new GameObject();
        leftLineContainer.transform.SetParent(this.transform);
        leftLine = leftLineContainer.AddComponent<LineRenderer>();
        leftLine.material = lineMaterial;
        leftLine.colorGradient = leftGradient;
        leftLine.startWidth = leftLine.endWidth = lineWidth;
        leftLine.sortingOrder = layerOrder;
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugOptions.hookFireVarient == HookFireVariant.OneHook)// && PlayerMovement_v3.SnapOctDirection(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) != Vector2.zero)
        {
            rightLine.enabled = false;
            Vector2 aimingDirection = PlayerMovement_v3.SnapOctDirection(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (aimingDirection != Vector2.zero)
            {
                rightLine.enabled = true;
                rightLine.SetPosition(0, this.transform.position);
                rightLine.SetPosition(1, this.transform.position + ((Vector3)aimingDirection * fadeDistance));
            }
        }
        else
        {
            rightLine.enabled = true;
            rightLine.SetPosition(0, this.transform.position);
            rightLine.SetPosition(1, this.transform.position + (new Vector3(1f, 1f, 0f) * fadeDistance));

            leftLine.SetPosition(0, this.transform.position);
            leftLine.SetPosition(1, this.transform.position + (new Vector3(-1f, 1f, 0f) * fadeDistance));
        }
    }
}
