using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
public class Inventory : MonoBehaviour
{
    public GameObject weapon;
    Weapon w;
    Player human;
    public List<Bonus> bonus;
    public EasyJoystick joyFire;
    public Transform instatnceWeapon;
    public Transform backWeapon;
    //  public LimbIK leftRootArm;
    //  public LimbIK rightRootArm;
    private static Inventory _entity;
    public static Inventory entity
    {
        get { return _entity; }
        set { _entity = value; }
    }
    void Awake()
    {
        entity = this;
        human = Player.entity;
        // leftRootArm = GameObject.FindGameObjectWithTag("LeftHand").GetComponent<LimbIK>();
        // rightRootArm = GameObject.FindGameObjectWithTag("RightHand").GetComponent<LimbIK>();
    }
    // Update is called once per frame
    void Update()
    {
        if (weapon)
        {
            //   AttachHand(leftRootArm, rightRootArm, Weapon.entity);
        }
        else
        {
            // leftRootArm.solver.IKPositionWeight = 0;
            // rightRootArm.solver.IKPositionWeight = 0;
        }
    }
    public void AddToInventory(GameObject item)
    {
        if (item.GetComponent<Weapon>() != null)
        {
           // Weapon wep = item.GetComponent<Weapon>();
            Weapon wep = Weapon.entity;
            //GameObject.Destroy(weapon);
            if (weapon)
            {
                weapon.transform.parent = backWeapon;
                //weapon.active = false;
                // weapon.transform.parent = Weapon.entity.parent;
            }
            weapon = item;
            // item.transform.parent = instatnceWeapon;
            // Debug.Log(wep.PositionOnBody + " " + wep.RotationOnBody);
            weapon.transform.parent = instatnceWeapon;
            weapon.transform.localPosition = wep.PositionOnBody;
            weapon.transform.localRotation = wep.RotationOnBody;
            weapon.transform.localScale = wep.ScaleOnBody;
          //  wep.SwitchWeappon();
            wep.SetTypeWeaponToAnim();
            joyFire.receiverGameObject = weapon;

            /*     weapon = GameObject.Instantiate(item, instatnceWeapon.position, instatnceWeapon.rotation) as GameObject;
            // weapon = item;   
            weapon.transform.parent = instatnceWeapon;
            weapon.active = true;
            // weapon.transform.localPosition = Weapon.entity.posOnParent;
            weapon.transform.localPosition = item.transform.localPosition;
            // weapon.transform.rotation = instatnceWeapon.rotation;
            weapon.transform.localRotation = item.transform.localRotation;
            weapon.transform.localScale = item.transform.localScale;
            joyFire.receiverGameObject = weapon;
            */

        }
        else
        {
            // bonus.Add(item.GetComponent<Bonus>());

            GameObject go = Instantiate(item, Player.entity.transform.position, Angles.zero) as GameObject;
            bonus.Add(go.GetComponent<Bonus>());

        }
    }

    public void DestroyWeapon()
    {
        var wep = gameObject.GetComponentInChildren<Weapon>();
        if (wep) wep.gameObject.SetActive(false);
    }
   /* public void AttachHand(LimbIK left, LimbIK right, Weapon weapon)
    {

        Transform leftHand = weapon.handAtach.pivotHandLeft;
        Transform rightHand = weapon.handAtach.pivotHandRight;

        left.solver.IKPosition = leftHand.position;
        right.solver.IKPosition = rightHand.position;
        left.solver.IKRotation = leftHand.rotation;
        right.solver.IKRotation = rightHand.rotation;
        left.solver.IKPositionWeight = 1;
        right.solver.IKPositionWeight = 1;
        left.solver.IKRotationWeight = 1;
        right.solver.IKRotationWeight = 1;
    }*/
}
