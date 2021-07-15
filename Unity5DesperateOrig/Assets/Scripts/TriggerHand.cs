using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class TriggerHand : MonoBehaviour
{
    public int damage;
    public bool attack;
    public static TriggerHand entity { get; set; }
    private bool Type = false;

    private void Awake()
    {
        entity = this;
        Type = gameObject.transform.root.GetComponent<Spider>();
    }

    /*void Update()
    {
        if (Type && attack && !IsInvoking("SpiderDamage")) Invoke("SpiderDamage", 1);
        if(Type)Debug.Log(attack);
    }*/
    // Use this for initialization
    void OnTriggerEnter(Collider entity)
    {

        // if (entity.GetComponent<Pawn>() && attack && entity.tag == "Player") entity.SendMessage("Damage", damage);
        if (entity.tag == "Player" && attack && !Type) entity.SendMessage("Damage", damage);
    }

    //void SpiderDamage()
    //{
    //    Player.entity.SendMessage("Damage", damage);
    //}
}