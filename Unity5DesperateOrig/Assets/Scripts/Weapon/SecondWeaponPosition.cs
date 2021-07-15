using UnityEngine;
using System.Collections;

public class SecondWeaponPosition : MonoBehaviour
{
    public Vector3 PositionOnParrent;
    public Quaternion RotationOnparrent;
    public Vector3 ScaleOnParrent;


    void OnEnable()
    {
        transform.localPosition = PositionOnParrent;
        transform.localRotation = RotationOnparrent;
        transform.localScale = ScaleOnParrent;
    }
}
