using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    public GameObject actualTarget;
    public GameObject camTarget;

    public float aheadAmount;   // how far to move
    public float aheadSpeed;    // how quickly to move

    private float curHorInput;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(actualTarget.transform.position.x, actualTarget.transform.position.y, this.transform.position.z);
        camTarget.transform.position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        curHorInput = Input.GetAxis("Horizontal");
        if (curHorInput != 0)
        {
            camTarget.transform.position = new Vector3(actualTarget.transform.position.x, actualTarget.transform.position.y, this.transform.position.z);
            camTarget.transform.localPosition = new Vector3(Mathf.Lerp(camTarget.transform.localPosition.x, aheadAmount * curHorInput, aheadSpeed * Time.deltaTime), camTarget.transform.localPosition.y, this.transform.position.z);
        }

        this.transform.position = camTarget.transform.position;
    }
}
