using UnityEngine;
using System.Collections;

public class CameraControll : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform target;
    public Vector3 cameraOffsets;
    public float lerphTime;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        cameraTransform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY
        Vector3 pos = new Vector3(target.position.x, cameraOffsets.y, target.position.z - cameraOffsets.x);
        Vector3 lerph = (target.forward * cameraOffsets.z) + pos;
        GetComponent<Camera>().transform.position = Vector3.Lerp(cameraTransform.position, lerph, lerphTime * Time.deltaTime);
//#endif
        }
 
}
