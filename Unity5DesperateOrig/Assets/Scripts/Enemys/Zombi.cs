//using UnityEditor;
using UnityEngine;
using System.Collections;

public class Zombi : Enemy
{
    
    public AnimationClip ide;
    public AnimationClip walk;
    public AnimationClip attack;

    public float delayTimeAttack;
    public float speed;
    public float speedConstant;
    float curentTimeAttack;
    float lenghtAttack;
    float coolDownAttack;
    Vector2 posMe;
    Vector2 posPlayer;
    public float distantionToPlayer;
    public bool isAttack;
    public bool isPosibleAttack;
    public bool isMove;


    void Awake()
    {
        heals += ((GameMeneger.entity._mulHealth / 100) * heals);
        Create();
        isAttack = true;
        distantionAttack *= distantionAttack;

    }
    void Start()
    {
        GetComponent<Animation>()[walk.name].layer = 1;
        GetComponent<Animation>()[attack.name].layer = 2;
        GetComponent<Animation>()[death.name].layer = 3;
        GetComponent<Animation>()[impulceDeath.name].layer = 4;
        lenghtAttack = GetComponent<Animation>()[attack.name].length - 0.1f;
    }
    void Update()
    {
        if (!isDeath)
        {

            //speed = navigator.agent.velocity.sqrMagnitude;
            //distantionToPlayer = Vector3.Distance(actorTransform.position, navigator.target);
            distantionToPlayer = (Target.transform.position - transform.position).sqrMagnitude;
           // Debug.Log(distantionToPlayer);
            curentTimeAttack = actorAnimation[attack.name].time;
            coolDownAttack -= Time.deltaTime;
            navigator.isMove = isMove;
            _hand.attack = isPosibleAttack;
            if (coolDownAttack < 0 && distantionAttack > distantionToPlayer)
            {
                isPosibleAttack = true;
                _hand.attack = isPosibleAttack;
            }
            else
            {
                isMove = true;

                if (curentTimeAttack >= lenghtAttack)
                {
                    isMove = true;

                }
                actorAnimation.CrossFade(walk.name);
            }
            if (isPosibleAttack)
            {

                isMove = false;

                actorAnimation.CrossFade(attack.name);
                coolDownAttack = delayTimeAttack;
                isPosibleAttack = false;
            }
            else
            {
                isMove = true;
            }
        }
        else
        {

            GetComponent<Animation>()[walk.name].enabled = false;
            GetComponent<Animation>()[attack.name].enabled = false;

        }
    }
    void LateUpdate()
    {

    }


    void OnParticleCollision(GameObject other)
    {

        DamageOffParticle(other.tag);

    }




}

