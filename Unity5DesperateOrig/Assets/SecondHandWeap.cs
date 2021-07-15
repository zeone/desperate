using UnityEngine;
using System.Collections;

public class SecondHandWeap : MonoBehaviour
{


    public float damage;


    [Header("Тип оружия")]
    public bool Pistol;
    public bool ShootGun;
    public bool RPG;
    public bool AK;
    public bool UZI;
    public bool FlameThrower;
    public bool PlazmaGun;
    public bool Minigun;
    [Header("Автоприцел")]
    public GameObject Target;
    [Tooltip("Используется для корекции направления, чтоб не стреляло в пол, значение получается автоматически")]
    public float YCorrector;
    public Vector3 randomSizeFlash;
    private Vector3 sizeEffect;
    private Vector3 TargetPositions;
    public GameObject meshEffect;
    public GameObject meshOtherEffect;
    public ParticleSystem pointParticle;
    [Tooltip("тип стрельбы, если включено то стреляет независемо от анимации")]
    public bool AlwaysFire;
    [Header("Позиция на персонаже")]
    public Vector3 PositionOnBody;

    public Quaternion RotationOnBody;
    public Vector3 ScaleOnBody;

    [Header("Другое оружие")]
    [Tooltip("Если оружие парное то сюда кормим пару в левой руке")]
    public GameObject _SecondWeapon;

    private Weapon _secondWeapon;

    [Tooltip("Префаб гранаты для м202")]
    public GameObject RpgGranate;

    public Transform parent;

    [Header("Поворотная часть минигана")]
    public Transform RotationMinigun;
    private float _shootTimer = 0;

    [Header("RocketPositions")]
    public Vector3[] PositionRocket;

    public Quaternion[] RotationRocket;
    public Vector3[] ScaleRocet;

    public RpgGrenade[] GranadeList;


    [Header("Огненные пули")]
    public bool FireBullets;

    [Tooltip("Кейс с огненными пулями")]
    public Transform FireBulletsCase;


    public static SecondHandWeap entity { get; set; }
    // Use this for initialization
    void Start()
    {
        YCorrector = transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
      
        FireBullets = GameMeneger.entity.FireBullets;
        if (Target && !Weapon.entity.isReload)
        {
            TargetPositions = Target.transform.position;
            TargetPositions.y = YCorrector;
            transform.LookAt(TargetPositions);
        }
        else
        {
            gameObject.transform.localRotation = RotationOnBody;
        }
    }

    public void FireSecond()
    {
        if (!FireBullets)
        {

            if (meshEffect)
            {

                sizeEffect.x = Random.Range(1, randomSizeFlash.x);
                sizeEffect.y = Random.Range(1, randomSizeFlash.y);
                sizeEffect.z = Random.Range(1, randomSizeFlash.z);

                meshEffect.transform.localScale = sizeEffect;
                if (meshOtherEffect) meshOtherEffect.transform.localScale = sizeEffect;
            }
            pointParticle.Play();
        }
        else
        {

            LaunchFirebullets();
        }
        //        Debug.Log("Start fire");

    }
    public void LaunchFirebullets()
    {
        foreach (Transform fire in FireBulletsCase)
        {

            fire.gameObject.SetActive(true);
            break;
        }
    }

    public void StopFire()
    {
        //  Debug.Log("Stop fire");
        if (meshEffect != null)
        {
            meshEffect.transform.localScale = Vector3.zero;
            if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
        }
    }
}
