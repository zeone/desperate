using UnityEngine;
using System.Collections;

public class WeaponCtrl : MonoBehaviour
{
    [Header("Тип оружия")]

    public GameObject ActiveWeapon;
    public GameObject[] Weapons;
    public EasyJoystick joyFire;


    void Update()
    {

    }

    public void ActivateWeapon(string wepTag)
    {
        DeactivateWeapon(wepTag);
        foreach (GameObject tr in Weapons)
        {
            if (tr.tag == wepTag && tr.active == false)
            {
                tr.SetActive(true);
                ActiveWeapon = tr;

            }

        }
        foreach (Transform obj in transform)
        {
            if (obj.gameObject.active == true)
            {
                Weapon wep = obj.GetComponent<Weapon>();

                obj.localPosition = wep.PositionOnBody;
                obj.localRotation = wep.RotationOnBody;
                obj.localScale = wep.ScaleOnBody;

                wep.SetTypeWeaponToAnim();

                joyFire.receiverGameObject = obj.gameObject;
            }
        }


    }

    void DeactivateWeapon(string wepTag)
    {
        ActiveWeapon = null;
        if (Weapon.entity) Weapon.entity.isReload = false;
        if (Weapon.entity) Weapon.entity.isFire = false;
        foreach (GameObject o in Weapons)
        {
            if (o.tag != wepTag) o.gameObject.SetActive(false);
        }
    }
}
