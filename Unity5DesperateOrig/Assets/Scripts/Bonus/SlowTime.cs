using UnityEngine;

public class SlowTime : Bonus
{
    void Start()
    {
        //Add();
        GameMeneger.entity.SlowTime = true;
        BonusInfo.slowTime = 0.5f;

    }
    void Update()
    {
        TimeOff();
        // BonusInfo.slowTime = Mathf.Lerp(BonusInfo.slowTime, 0.5f, Time.deltaTime);


        // time -= Time.deltaTime;
        if (_willDestroed)
        {
            GameMeneger.entity.SlowTime = false;
            BonusInfo.slowTime = 1;
        }
    }

}

