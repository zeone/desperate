
using JetBrains.Annotations;
using RootMotion.FinalIK.Demos;
using UnityEngine;
using System.Collections;

public class PlazmaBulletCtrl : MonoBehaviour
{
    [Header("Основные компоненты")]
    public GameObject Parent;
    public Vector3 PositionOnParrent;
    public Quaternion RotationOnParrent;
    public GameObject MainBall;
    public GameObject[] OtherBalls;

    public bool Fireblast;

    [Space(10)]
    [Header("Параметры")]
    public int Speed;
    public float TimeToDisable = 5;
    public bool IsMainBullet;
    private Collider _colider;
    private Rigidbody _body;
    private float _timer;
    private bool _isCollider = false;

    [Space(10)]
    [Header("Звуки")]
    public AudioClip FlySound;

    public AudioClip ExplosionSound;


    [Header("Если это у нас Fireblast то он будет демажить")]
    public int Damage = 150;
    private AudioSource _audio;

    void Start()
    {
        var a = gameObject.GetComponent<AudioSource>();
        if (a) _audio = a;

    }
    void OnEnable()
    {
        if (Fireblast && IsMainBullet)
        {
            // _colider.enabled = false;
            transform.parent = null;

        }
        var col = gameObject.GetComponent<SphereCollider>();
        if (col) _colider = col;
        if (_colider) _colider.enabled = true;
        _timer = TimeToDisable;
        if (IsMainBullet)
        {

            transform.parent = null;
            if (MainBall) MainBall.SetActive(true);
        }

    }

    private void OnDisable()
    {
        if (Fireblast)
        {
            if (IsMainBullet && Parent) transform.parent = Parent.transform;
            transform.localPosition = PositionOnParrent;
            transform.localRotation = RotationOnParrent;
        }
        if (IsMainBullet)
        {
            if (!IsInvoking("AsignToParrent")) Invoke("AsignToParrent", 1);
            _isCollider = false;


        }
        else
        {
            _isCollider = false;

            transform.localPosition = PositionOnParrent;
            transform.localRotation = RotationOnParrent;
        }
    }

    void AsignToParrent()
    {
        if (Parent) transform.parent = Parent.transform;
        transform.localPosition = PositionOnParrent;
        transform.localRotation = RotationOnParrent;
    }


    private void OnTriggerEnter(Collider col)
    {

        if (col.tag != "Weapon" && col.tag != "Player" && col.name != "B_Turet" && col.transform.root.tag != "Player")
        {

            if (_audio) _audio.clip = ExplosionSound;
            if (_audio.enabled && !_audio.isPlaying) _audio.Play();
            if (_colider) _colider.enabled = false;
            if (!_isCollider)
            {

                if (IsMainBullet)
                {
                    if (col.tag == ("Enemy")) GameMeneger.entity.AddHit();
                    transform.rotation = new Quaternion(0, 0, 0, 1);
                    if (MainBall) MainBall.GetComponentInChildren<ProjectileCollisionBehaviour>().CollisionEnter();
                }
                else
                {
                    var colObj = gameObject.GetComponentInChildren<ProjectileCollisionBehaviour>();
                    if (colObj) colObj.CollisionEnter();
                    if (!IsInvoking("DeactivateChild")) Invoke("DeactivateChild", 1);
                }

                _isCollider = true;
                if (IsMainBullet) EnableOthersBalls();
            }
        }
    }

    void DeactivateMain()
    {
        if (MainBall) MainBall.SetActive(false);
    }
    void DeactivateChild()
    {

        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0) DisableChilds();
        if (!_isCollider)
        {
            if (_audio && _audio.enabled)
            {
                _audio.clip = FlySound;
                if (_audio.enabled && !_audio.isPlaying) _audio.Play();
            }
            //if (IsMainBullet || gameObject.tag == "PlazbaBullet") transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            if (!Fireblast)
            {
                transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            }
        }


    }

    void EnableOthersBalls()
    {
        foreach (GameObject Tr in OtherBalls)
        {
            Tr.SetActive(true);
        }
    }

    void DisableChilds()
    {
        if (IsMainBullet)
        {
            foreach (GameObject Tr in OtherBalls)
            {
                Tr.SetActive(false);
            }
            if (MainBall) MainBall.SetActive(false);
        }
        gameObject.SetActive(false);
    }


}
