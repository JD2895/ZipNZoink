using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    PlayerControls controls;
    float horizontalInput;
    float verticalInput;
    Vector2 aimingDirection;

    [Header("Aim Assist")]
    public LayerMask castingLayers;
    public float castDistance;
    public List<float> circleSizes = new List<float>();

    // Debug Visuals
    Vector3 debugCircleOrigin = new Vector3();
    float debugCircleSize = 0;
    bool drawDebugCircle = false;

    private void Awake()
    {
        // New input system
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.OneHook.HoriztonalAxis.performed += HandleHorizontalAxis;
        controls.OneHook.VerticalAxis.performed += HandleVerticalaxis;

        controls.OneHook.HoriztonalAxis.Enable();
        controls.OneHook.VerticalAxis.Enable();
    }

    private void OnDisable()
    {
        controls.OneHook.HoriztonalAxis.performed -= HandleHorizontalAxis;
        controls.OneHook.VerticalAxis.performed -= HandleVerticalaxis;

        controls.OneHook.HoriztonalAxis.performed -= HandleHorizontalAxis;
        controls.OneHook.VerticalAxis.performed -= HandleVerticalaxis;
    }

    private void HandleHorizontalAxis(InputAction.CallbackContext obj)
    {
        horizontalInput = obj.ReadValue<float>();
    }

    private void HandleVerticalaxis(InputAction.CallbackContext obj)
    {
        verticalInput = obj.ReadValue<float>();
    }

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
            leftLine.enabled = false;

            aimingDirection = PlayerMovement_v3.SnapOctDirection(horizontalInput, verticalInput);
            if (aimingDirection != Vector2.zero)
            {
                rightLine.enabled = true;
                rightLine.SetPosition(0, this.transform.position);
                rightLine.SetPosition(1, this.transform.position + ((Vector3)aimingDirection * fadeDistance));

                AimingData targetAimData = GetTargetPosition();
                if (targetAimData.validTarget)
                {
                    leftLine.enabled = true;
                    leftLine.SetPosition(0, targetAimData.targetPosition);
                    leftLine.SetPosition(1, targetAimData.targetPosition + ((Vector3)aimingDirection * fadeDistance * -0.5f));
                }
            }
        }
        else if (DebugOptions.hookFireVarient != HookFireVariant.None)
        {
            rightLine.enabled = true;
            rightLine.SetPosition(0, this.transform.position);
            rightLine.SetPosition(1, this.transform.position + (new Vector3(1f, 1f, 0f) * fadeDistance));

            leftLine.SetPosition(0, this.transform.position);
            leftLine.SetPosition(1, this.transform.position + (new Vector3(-1f, 1f, 0f) * fadeDistance));
        }
    }

    public AimingData GetTargetPosition()
    {
        Vector3 targetPosition = new Vector3(0,0,0);
        bool validTarget = false;

        foreach(float castSize in circleSizes)
        {
            Vector3 aimStartOffset = this.transform.position + (0.5f * (Vector3)aimingDirection);
            RaycastHit2D castHit = Physics2D.CircleCast(aimStartOffset, castSize, aimingDirection, castDistance, castingLayers);
            
            /*// Debug drawing circles
            debugCircleOrigin = aimStartOffset;
            debugCircleSize = castSize;
            drawDebugCircle = true; //*/
            
            if (castHit.collider != null)
            {
                // The following if statement makes sure that the first thing the cast hits is on the gorund layer
                // If the thing we hit isn't the ground layer, we try again in the next cast first before breaking out
                if (castHit.collider.CompareTag("CameraEdge") || castHit.collider.CompareTag("Hazard"))
                {
                    validTarget = false;
                }
                else
                {
                    targetPosition = castHit.point;
                    validTarget = true;
                    break;
                }
            }
        }

        return new AimingData(targetPosition, validTarget);
    }

    private void OnDrawGizmos()
    {
        if (drawDebugCircle)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(debugCircleOrigin, debugCircleSize);
        }
    }

    public struct AimingData
    {
        public AimingData(Vector3 newTargetPosition, bool newValidTarget)
        {
            targetPosition = newTargetPosition;
            validTarget = newValidTarget;
        }

        public Vector3 targetPosition;
        public bool validTarget;
    }
}
