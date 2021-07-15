using UnityEngine;
using System.Collections;

public class Turet : Bonus
{
    public Transform gun;
    public Transform target;
    public AudioClip soundFire;
    public float damage;
    public float speedFire;
    public bool fire;
    public ParticleSystem pointParticle;
    public GameObject meshEffect;
    public AnimationClip HideAnimaion;
    Animation animation;
    private float _stopTimer;
    
    public static Turet entity { get; set; }
    // Use this for initialization
    void Start()
    {
        _stopTimer = time;
        GameMeneger.entity.Turret = true;
        audio = gameObject.GetComponent<AudioSource>();
        entity = this;
        //AddBonus();
        animation = GetComponent<Animation>();
        ///BonusInfo.turet = this;

    }
    void Update()
    {

        if (pointParticle)
        {
            TuretFire();
        }
        _stopTimer -= Time.deltaTime;
        if (_stopTimer < 0)
        {

            if (!IsInvoking("EventRemove"))
            {
                animation.clip = HideAnimaion;
                animation[HideAnimaion.name].speed = -1f;
                animation[HideAnimaion.name].wrapMode = WrapMode.ClampForever;
                animation[HideAnimaion.name].time = HideAnimaion.length;// animation[HideAnimaion.name].length;
                animation.CrossFade(HideAnimaion.name);

                Invoke("EventRemove", HideAnimaion.length);
            }
            // EventRemove();
        }
    }
    void TuretFire()
    {
        if (target)
        {
            gun.LookAt(target.position + new Vector3(0, 1, 0));
            fire = true;
        }
        else
        {
            fire = false;
        }
        if (fire)
        {
            if (!IsInvoking("Fire"))
            {
                Invoke("Fire", speedFire);
                pointParticle.Play();
            }
            meshEffect.transform.localScale = Random.insideUnitSphere * 5;
        }
        else
        {
            meshEffect.transform.localScale = Vector3.zero;
        }
    }
    void Fire()
    {
        audio.clip = soundFire;
        audio.Play();
    }
    void EventRemove()
    {
        GameMeneger.entity.Turret = false;
        gameObject.SetActive(false);

        //RemoveBonus();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!target)
            {
                if (!animation.isPlaying)
                {
                    target = other.transform;
                }
            }
        }
    }
}
