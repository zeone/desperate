using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public class TimeBuffs : MonoBehaviour
{


    public float TimeToGone = 5f;
    public bool Shield;
    public bool FireBullets;
    void Start()
    {
        if (Shield) GameMeneger.entity.Shield = true;
        if (FireBullets) GameMeneger.entity.FireBullets = true;
    }

    void Update()
    {
        TimeToGone -= Time.deltaTime;
        if (TimeToGone < 0)
        {
            if (Shield) GameMeneger.entity.Shield = false;
            if (FireBullets) GameMeneger.entity.FireBullets = false;
            Destroy(gameObject);
        }
    }


}
