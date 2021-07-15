using UnityEngine;
using System.Collections;



public class RpgGrenade : MonoBehaviour
{
    private Rigidbody _body;
    public float Force = -1000;
    public ParticleSystem Flame;
    public GameObject ExplosionObj;
    public float RangeDmg = 5;
    private bool isShoot = false;
    public int Damage = 100;
    public float DestroyTime = 5;
    private float _timer;
    public AudioClip FlyClip;
    public AudioClip ExplosionClip;
    private AudioSource _audio;
    private MeshRenderer _mesh;


    [Tooltip("Если скрипт используется для брнуса")]
    public bool Nuke;

    public GameObject[] Bullets;
    // Use this for initialization
    private void Start()
    {
        RangeDmg *= RangeDmg;
        _body = gameObject.GetComponent<Rigidbody>();
        _audio = gameObject.GetComponent<AudioSource>();
        _mesh = gameObject.GetComponent<MeshRenderer>();
        if (Nuke)
        {
            Explosion();

        }

    }

    void OnEnable()
    {
        foreach (GameObject bullet in Bullets)
        {
            bullet.SetActive(true);
        }

    }

    void Update()
    {
        if (isShoot || Nuke)
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                if (Nuke)
                {
                    gameObject.SetActive(false);
                    //   if (Weapon.entity.Target != null) transform.LookAt(Weapon.entity.Target.transform.position);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (isShoot && col.tag != "Weapon" && col.transform.root.tag != "Weapon"
            && col.tag != "Player" && col.transform.root.tag != "Player" && col.name != "B_Turet") Explosion();
    }




    public void Launch()
    {
        _timer = DestroyTime;
        isShoot = true;
        if (Flame) Flame.Play();
        transform.parent = null;
        _body.isKinematic = false;
        _body.useGravity = true;
        _body.AddForce(transform.forward * Force);
        // _body.AddForce(new Vector3(0,0,-100));
        _audio.clip = FlyClip;
        _audio.Play();
    }

    private void Explosion()
    {
        Instantiate(ExplosionObj, transform.position, Quaternion.identity);
        // ExplosionObj.SetActive(true);
        //ExplosionObj.transform.parent = null;
        if (!Nuke)
        {
            _body.isKinematic = true;
            _mesh.enabled = false;
        }
        _audio.clip = ExplosionClip;
        if (!_audio.isPlaying) _audio.Play();
        SendDamage();
        if (!Nuke)
        {
            Destroy(gameObject, 1);
        }

    }

    private void SendDamage()
    {
        var enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject o in enemys)
        {
            if ((o.transform.position - transform.position).sqrMagnitude < RangeDmg)
            {
                o.GetComponent<EnemyN>().DamageFromRpg(Damage);
            }
        }
    }
}
