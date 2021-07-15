using UnityEngine;
using System.Collections;

public class Bonus : Actor
{

    [Header("Buff confing")]
    [Tooltip("Время жизни")]
    public float time;
    public bool isAdd;
    public bool isTimeStart;
    protected float _timer;
    public AudioClip soundEqipment;
    public AudioSource audio;
    public MeshRenderer render;
    public EquipmentItems icon;
    public Transform transform;
    //контрольная переменная которая указывает что объект сейчас уничтожится, ипользуется для бафов действие которых надо отменить
    protected bool _willDestroed = false;
    // Use this for initialization
    /*  void Start()
      {
          //render.enabled = true;
        //  icon.enabled = true;
        
      }*/


    protected void TimeOff()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            _willDestroed = true;
            Destroy(gameObject);
        }
    }



    public void Add()
    {
        // isAdd = true;
        // icon.enabled = false;
        //render.enabled = false;
    }
    public bool TimeLive(ref float time, bool isOutTime = false)
    {
        if (isTimeStart)
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                isOutTime = true;
                isTimeStart = false;
                Remove();
            }
        }
        return isOutTime;

    }
    public void Remove()
    {
        icon.enabled = false;
        //render.enabled = false;
        Inventory.entity.bonus.Remove(this);
        transform.localPosition = Vector3.zero;
        gameObject.active = false;
    }
}
