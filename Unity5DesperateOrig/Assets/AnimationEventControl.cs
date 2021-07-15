using UnityEngine;
using System.Collections;

public class AnimationEventControl : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Fire()
    {

        Weapon.entity.Fire();
        //Debug.Log("EventController");

    }

    public void FireFinish()
    {
        Weapon.entity.FireFinish();
    }

    public void ReloadDone()
    {
        Weapon.entity.Reload();
    }
}
