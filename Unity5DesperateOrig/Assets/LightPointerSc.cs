using UnityEngine;
using System.Collections;

public class LightPointerSc : MonoBehaviour
{


    public Vector3 FlameThrower;
    public Vector3 Minigun;
    public Vector3 M202;
    public Vector3 Rifle;
    public Vector3 Shotgun;
    public Vector3 PlazmaGun;
    public Vector3 Pistol;
    public Vector3 Uzi;


    // Update is called once per frame
    void Update()
    {
        if (!Weapon.entity) return;
        if (Weapon.entity.tag == "FlameThrower") transform.localPosition = FlameThrower;
        if (Weapon.entity.tag == "Minigun") transform.localPosition = Minigun;
        if (Weapon.entity.tag == "m202") transform.localPosition = M202;
        if (Weapon.entity.tag == "Rifle") transform.localPosition = Rifle;
        if (Weapon.entity.tag == "Shotgun") transform.localPosition = Shotgun;
        if (Weapon.entity.tag == "PlazmaGun") transform.localPosition = PlazmaGun;
        if (Weapon.entity.tag == "Uzi") transform.localPosition = Uzi;
        if (Weapon.entity.tag == "Pistol") transform.localPosition = Pistol;

    }
}
