using UnityEngine;
using System.Collections;

public class ChangeCamera : MonoBehaviour
{

    public GameObject CurrentCam;
    public GameObject TargetCam;

    void Start()
    {
        CurrentCam = this.gameObject;
    }

    public void SwithCams()
    {
        if (TargetCam != null)
        {
            TargetCam.active = true;
            CurrentCam.active = false;
        }
    }
}
