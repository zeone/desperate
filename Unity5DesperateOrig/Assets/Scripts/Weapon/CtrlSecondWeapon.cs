using UnityEngine;
using System.Collections;

public class CtrlSecondWeapon : MonoBehaviour
{

    public GameObject SecondWeapon;

    void OnEnable()
    {
        SecondWeapon.SetActive(true);
    }

    void OnDisable()
    {
        SecondWeapon.SetActive(false);
    }
}
