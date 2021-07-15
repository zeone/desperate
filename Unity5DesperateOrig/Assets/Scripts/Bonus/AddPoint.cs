using UnityEngine;
using System.Collections;

public class AddPoint : Bonus
{
    public int HealsPoint, ExperiencePoint, HealPointsInPercents;
    private float maxHp;
    public ParticleSystem particle;

    void Start()
    {
        maxHp = Player.entity.maxHeals;
        Player.entity.SendMessage("AddHp", (maxHp / 100 * HealPointsInPercents));
        Player.entity.SendMessage("AddHp", HealsPoint);
        GameMeneger.entity.SendMessage("AddXp", ExperiencePoint);

        //gameObject.SendMessage("AddHp", HealsPoint);
    }

    void Update()
    {
        TimeOff();
        //if(isAdd)
        //{

        //    particle.Play();
        //    GameMeneger.entity.xp += ExperiencePoint;
        //    Player.entity.heals += HealsPoint;
        //    isTimeStart = true;
        //    isAdd = false;
        //}

        //TimeLive(ref time);

    }



}
