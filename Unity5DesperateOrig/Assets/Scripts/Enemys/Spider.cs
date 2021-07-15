using UnityEngine;
using System.Collections;

public class Spider : Enemy
{


    public AnimationClip ide;
    public AnimationClip walk;
    public AnimationClip attack;

    public float damage;

    public float delayTimeAttack;
    public float speed;
    public float speedConstant;
    float curentTimeAttack;
    float lenghtAttack;
    float coolDownAttack;
    Vector2 posMe;
    Vector2 posPlayer;
    public float distantionToPlayer;
    public bool isPosibleAttack;
    public bool isMove;


    void Awake()
    {
        heals += ((GameMeneger.entity._mulHealth / 100) * heals);
        Create();
        distantionAttack *= distantionAttack;
    }
    // Use this for initialization
    void Start()
    {
        GetComponent<Animation>()[walk.name].layer = 1;
        GetComponent<Animation>()[attack.name].layer = 2;
        GetComponent<Animation>()[death.name].layer = 3;
        GetComponent<Animation>()[impulceDeath.name].layer = 4;
        lenghtAttack = GetComponent<Animation>()[attack.name].length - 0.1f;
    }

    // Update is called once per frame
    void Update()
    {


        if (!isDeath)
        {

            //speed = navigator.agent.velocity.sqrMagnitude;
            // distantionToPlayer = (Target.transform.position - transform.position).sqrMagnitude;
            distantionToPlayer = (Target.transform.position - transform.position).sqrMagnitude;
            // Debug.Log(distantionToPlayer + "  " + distantionAttack);
            curentTimeAttack = actorAnimation[attack.name].time;
            coolDownAttack -= Time.deltaTime;
            navigator.isMove = isMove;

            if (distantionAttack > distantionToPlayer)
            {
                isMove = false;
                isPosibleAttack = true;
            }
            else
            {
                isMove = true;
                isPosibleAttack = false;
            }

            if (isMove) actorAnimation.CrossFade(walk.name);


            //if (coolDownAttack < 0 && distantionAttack > distantionToPlayer)
            //{
            //    isMove = false;
            //    isPosibleAttack = true;
            //}
            //else
            //{
            //    //if (curentTimeAttack >= lenghtAttack)
            //    //{
            //    //    isMove = true;

            //    //}
            //   // actorAnimation.CrossFade(walk.name);
            //}
            if (isPosibleAttack && coolDownAttack < 0)
            {

                isMove = false;

                actorAnimation.CrossFade(attack.name);
                coolDownAttack = delayTimeAttack;
                if (!IsInvoking("SpiderDamage")) Invoke("SpiderDamage", 1);

                isPosibleAttack = false;
            }
        }
        else
        {

            GetComponent<Animation>()[walk.name].enabled = false;
            GetComponent<Animation>()[attack.name].enabled = false;

        }
    }
    void SpiderDamage()
    {
        Player.entity.SendMessage("Damage", damage);
    }

    void OnParticleCollision(GameObject other)
    {

        DamageOffParticle(other.tag);

    }
}
