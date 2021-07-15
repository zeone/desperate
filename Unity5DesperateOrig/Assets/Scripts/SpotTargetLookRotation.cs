using System.Net;
using UnityEngine;
using System.Collections;

public class SpotTargetLookRotation : MonoBehaviour
{
    public Transform LightPoint;
    public GameObject AutotargetObj;
    [Tooltip("Позиция обьекта автокаста")]
    public Vector3 AutoPosition;
    [Tooltip("Correct position under Ground")]
    public float YCorrection;
    [Tooltip("Correct angle")]
    public Quaternion AutoRotation;
    //public Transform WeaponSlot;
    //public WeaponCtrl weap;
    //private GameObject RotationWeapon;

    void Start()
    {

    }
    // Update is called once per frame
    private void Update()
    {
        AutoPosition = AutotargetObj.transform.position;
        AutoPosition.y = YCorrection;
        AutotargetObj.transform.position = AutoPosition;
        AutotargetObj.transform.rotation = AutoRotation;
        transform.LookAt(LightPoint);
        //if (weap.ActiveWeapon != null && RotationWeapon != weap.ActiveWeapon) RotationWeapon = weap.ActiveWeapon;
        //if (weap.ActiveWeapon)
        //{
        //    if (weap.ActiveWeapon.tag != "Minigun")
        //    {

        //        transform.LookAt(weap.transform);
        //    }
        //    else
        //    {
        //        transform.localRotation = new Quaternion(0, -0.01922031f, 0, 0.9998153f);
        //    }

        //}
        //else
        //{
        //    // transform.rotation = WeaponSlot.rotation;
        //    transform.LookAt(WeaponSlot.transform);
        //}
    }
}
