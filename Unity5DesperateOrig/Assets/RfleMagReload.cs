using UnityEngine;
using System.Collections;

public class RfleMagReload : MonoBehaviour
{

    public GameObject Magazine;

    // Update is called once per frame
    void Update()
    {
        if (Weapon.entity.isReload)
        {
            if (Magazine.active) Magazine.SetActive(false);
        }
        else
        {
            if (!Magazine.active) Magazine.SetActive(true);
        }

    }
}
