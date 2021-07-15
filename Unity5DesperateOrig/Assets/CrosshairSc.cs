using UnityEngine;
using System.Collections;

public class CrosshairSc : MonoBehaviour
{


    public Camera Camera;
    public Transform character;


    // Cursor settings
    public float cursorPlaneHeight = 0;
    public float cursorFacingCamera = 1;
    public float cursorSmallerWithDistance = 0;
    public float cursorSmallerWhenClose = 1;

    public Vector3 Position;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Position;
        LookAtCamera();
    }


    void LookAtCamera()
    {

        Quaternion cursorWorldRotation = transform.rotation;
        /*if (motor.facingDirection != Vector3.zero)
            cursorWorldRotation = Quaternion.LookRotation (motor.facingDirection);
        */
        // Calculate cursor billboard rotation
        // Vector3 cursorScreenspaceDirection = Input.mousePosition - Camera.WorldToScreenPoint(transform.position + character.up * cursorPlaneHeight);
        Vector3 cursorScreenspaceDirection = Camera.WorldToScreenPoint(transform.position + character.up * cursorPlaneHeight);
         cursorScreenspaceDirection.z = 0;
        
        Quaternion cursorBillboardRotation = transform.rotation * Quaternion.LookRotation(cursorScreenspaceDirection, -Vector3.forward);
        // Quaternion cursorBillboardRotation = transform.rotation * Quaternion.LookRotation(Camera.transform.position, -Vector3.forward);
        // Set cursor rotation
        transform.rotation = Quaternion.Slerp(cursorWorldRotation, cursorBillboardRotation, cursorFacingCamera);

    }
}
