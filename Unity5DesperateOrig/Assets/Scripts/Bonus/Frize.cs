using UnityEngine;
public class Frize : Bonus
{
    public GameObject[] enemys;
    GameObject[] iceDoor;
    void Start()
    {

        /* iceDoor = GameObject.FindGameObjectsWithTag("IceDoor");
         for (int i = 0; i < iceDoor.Length; i++)
         {
             iceDoor[i].GetComponent<Animation>().Play("IceShow");
         }*/
        //Invoke("RemoveBuffs", time);
        /*    BonusInfo.frize = true;
            enemys = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemys.Length; i++)
            {
                enemys[i].SendMessage("Frize");
            }*/
        GameMeneger.entity.Frieze = true;

    }
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        if (time < 0)
        {
            EventRemove();
        }
    }
    void EventRemove()
    {
        GameMeneger.entity.Frieze = false;
        //BonusInfo.frize = false;
        //   enemys = GameObject.FindGameObjectsWithTag("Enemy");
        /* for (int i = 0; i < iceDoor.Length; i++)
         {
             iceDoor[i].GetComponent<Animation>().Play("IceDestroy");
         }*/
        //   for (int i = 0; i < enemys.Length; i++)
        //{
        //     enemys[i].SendMessage("UnFrize");
        //  }
        //iceDoor.animation.Play("IceDestroy");
        //Invoke("UnFrize",3);



    }
    void UnFrize()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].SendMessage("UnFrize");
        }

        BonusInfo.frize = false;
    }
}
