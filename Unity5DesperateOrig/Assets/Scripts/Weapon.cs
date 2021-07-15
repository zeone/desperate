using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
//класс вооружения
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Weapon : Actor
{
#pragma strict
    public Sprite icon;
    [Header("Confing")]
    public Sprite WeaponIcon;
    public float damage;
    public float CritDamage;
    public float ChancheCritDamage;
    public float speedFire;
    public int ammo;
    public float rechargeTime;
    public bool isImpulse;
    public bool isPowerfull;
    [Header("Info")]
    public float curentShoot;
    public int curentAmmo;
    public float curentTimeRecharge;
    public bool isFire;
    public bool isReload;
    public Animator PlayerAnim;
    private bool _infiniteAmmo;
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

    [Tooltip("Используется для коррекции положения левой руки")]
    public Transform HandCorrector;
    [Tooltip("Используется для задания минимального веса при перезарядке, например когда нужно ограничить движения")]
    public float ReloadCorrectionWeight = 0;
    private Vector3 TargetPositions;
    [Header("Effect")]
    //   public Vector3 posOnParent;
    //  public Vector3 angleOnParent;
    public float fireAnimSpeed;
    public Vector3 randomSizeFlash;
    private Vector3 sizeEffect;
    public GameObject meshEffect;
    public GameObject meshOtherEffect;
    public ParticleSystem pointParticle;
    public Transform transform;
    public Animation animation;
    public AnimationClip FireAnim;
    public AudioSource audio;
    public Weapon buffer;
    public SoundWeapon sound;
    //public HandAtach handAtach;
    [Tooltip("тип стрельбы, если включено то стреляет независемо от анимации")]
    public bool AlwaysFire;
    [Header("Позиция на персонаже")]
    public Vector3 PositionOnBody;

    public Quaternion RotationOnBody;
    public Vector3 ScaleOnBody;
    [Tooltip("Поворачивает пушку при перезарядке, для того чтоб рука не гуляла сама по себе")]
    public Quaternion ReloadRotation = Quaternion.identity;
    [Header("Другое оружие")]
    [Tooltip("Если оружие парное то сюда кормим пару в левой руке")]
    public GameObject _SecondWeapon;

    public SecondHandWeap SecWeap;

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
    [Space(10)]

    public Transform Laser;
    //Таймер и значение для того чтоб прятать вспышку после выстрела
    private float _flashTimmer;
    [Tooltip("Через сколько прятать вспышку от выстрела")]
    public float FlashTime = 0.1f;

    public static Weapon entity
    {
        get;
        set;
    }

    private Quaternion _rotationParticles;
    public float RotationPartickleX;


    public WeaponHandCorrection handCor;
    private bool IsShoot;
    //private void Awake()
    //{

    //}

    void OnEnable()
    {
        /* if (!SecWeap)
         {*/
        Weapon.entity = null;
        entity = this;
        isReload = true;
        //  Debug.Log("Weapon chnget to " +  name);
        //}
        // UIsc.entity.ChangeWeapIcon(WeaponIcon);
    }


    private void Start()
    {

        //  entity = this;

        if (_SecondWeapon && _SecondWeapon.activeInHierarchy) _secondWeapon = _SecondWeapon.GetComponent<Weapon>();
        //Инициализируем масив для гранат
        if (RPG) GranadeList = new RpgGrenade[ammo];
        //Получаем аниматор персонажа
        //PlayerAnim = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
        PlayerAnim = GameMeneger.entity.PlayerAnimator;
        //задаем положение персонажа в зависимотси от типа оружия
        //  SetTypeWeaponToAnim();
        audio = gameObject.GetComponent<AudioSource>();
        transform = gameObject.GetComponent<Transform>();
        if (!RPG)
        {
            // curentAmmo = ammo;

            var an = gameObject.GetComponent<Animation>();
            if (an != null) animation = an;

            curentTimeRecharge = rechargeTime;
            // handAtach.LoadConfingHands();
            if (animation != null) animation["Fire1"].speed = fireAnimSpeed;

            if (icon)
            {
                // UIManager.entity.SetWeapon(icon);
            }
            curentShoot = speedFire;
        }
        else
        {


            curentAmmo = 0;
            PlayerAnim.SetBool("Reload", true);
        }
        //Засуну костыль но до будет разобратся
        //YCorrector = !Pistol ? transform.position.y : 7;
        //YCorrector = transform.position.y;
       // YCorrector = Minigun ? 7.5f : transform.position.y;
        if (Minigun || FlameThrower)
        {
            YCorrector = 7.5f;
        }
        else
        {
            YCorrector = transform.position.y;
        }
        SetTypeWeaponToAnim();
        GetLaser();
        if (Laser) Laser.gameObject.SetActive(false);

    }

    private void On_JoystickMove(MovingJoystick move)
    {
        isFire = true;


    }

    private void On_JoystickMoveEnd(MovingJoystick move)
    {

        isFire = false;
        _shootTimer = 0;
    }

    private void Update()
    {

        //проверяем чтоб второе оружие было активным, если оно есть
        if (SecWeap != null && !SecWeap.gameObject.active) SecWeap.gameObject.SetActive(true);
        if (Weapon.entity.tag != tag) Debug.LogError("Different weapon");
        //перепроверяем чтоб наши параметры были заполнеными
        if (PlayerAnim == null) PlayerAnim = GameMeneger.entity.PlayerAnimator;

        //-------------
#if UNITY_EDITOR || (UNITY_EDITOR_WIN ||UNITY_STANDALONE_WIN)


        if (Input.GetAxis("Fire1") > 0)
        {
            isFire = true;
        }
        else
        {
            isFire = false;
            _shootTimer = 0;
        }
#endif
        // Debug.Log(isFire);
        FireBullets = GameMeneger.entity.FireBullets;
        _infiniteAmmo = GameMeneger.entity.SlowTime;
        if (_SecondWeapon && _SecondWeapon.activeInHierarchy) _secondWeapon.isFire = isFire;
        if (!Player.entity.isDeath)
        {

            ChoseFire();
        }

        /* if (buffer != null)
         {
             handAtach.ConfingHand();
             buffer.handAtach.posLeftHand = handAtach.posLeftHand;
             buffer.handAtach.posRightHand = handAtach.posRightHand;
             buffer.handAtach.rotLeftHand = handAtach.rotLeftHand;
             buffer.handAtach.rotRightHand = handAtach.rotRightHand;
             buffer.posOnParent = transform.localPosition;

         }*/
        if (SecondWeapon.entity && SecondWeapon.entity.Target != Target) SecondWeapon.entity.Target = Target;

        /*  if (!isReload && curentAmmo > 0 && handCor != null && handCor.WeightPosition < 1)
          {
              handCor.WeightPosition += 0.02f;
          }*/


        //Оружие будет смотреть на цель если она в досягаемости
        if (Target && !isReload && !UZI)
        {
            TargetPositions = Target.transform.position;
            TargetPositions.y = YCorrector;
            transform.LookAt(TargetPositions);
        }
        else if (isReload && ReloadRotation != Quaternion.identity)
        {
            //  gameObject.transform.localRotation = ReloadRotation;
            // gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, ReloadRotation, Time.deltaTime * 3);
        }
        else
        {
            gameObject.transform.localRotation = RotationOnBody;
        }

        //Прячем вспышку после выстрела
        if (!AlwaysFire)
        {
            _flashTimmer += Time.deltaTime;
            if (_flashTimmer > FlashTime)
            {
                if (meshEffect != null)
                {
                    meshEffect.transform.localScale = Vector3.zero;
                    if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
                }
            }
        }
    }

    public void ChoseFire()
    {
        if (curentAmmo > 0)
        {
            /* if (PlazmaGun)
             {

                 PlazmaGunFire(isFire);
             }
             else
             {*/

            MainFire(isFire);
            //}
        }
        else if (curentAmmo <= 0)
        {
            /*    if (RPG)
                {
                    ReloadRPG();
                }
                else
                {*/
            //Reload();
            /*  if (handCor != null && handCor.WeightPosition > 0.1f)
               {
                   handCor.WeightPosition -= 0.02f;
               }
               else
               {*/

            StopFire();
            ReloadBegin();
            //  } //  }

        }
        else
        {
            if (!RPG && meshEffect != null)
            {
                meshEffect.transform.localScale = Vector3.zero;
                if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
            }
        }

    }


    public void MainFire(bool fire)
    {
        if (fire)
        {
            if (AlwaysFire)
            {
                PlayAlways();
            }
            else
            {
                PlayerAnim.SetBool("IsShoot", true);
            }
        }
        else
        {
            StopFire();
        }
    }

    public void Fire()
    {
        if (isFire)
        {
            GameMeneger.entity.AddShoot();
            if (RPG)
            {
                GranadeList[curentAmmo - 1].Launch();
                curentAmmo -= 1;
                return;
            }
            if (PlazmaGun)
            {
                foreach (Transform o in transform)
                {
                    if (o.tag == "PlazmaBullet")
                    {

                        // o.SetActive(true);
                        PlayerAnim.SetBool("IsShoot", true);
                        o.gameObject.SetActive(true);
                        curentAmmo -= 1;
                        // Debug.Log(o.name);

                        break;
                    }
                }

                return;
            }
            if (meshEffect)
            {

                sizeEffect.x = Random.Range(1, randomSizeFlash.x);
                sizeEffect.y = Random.Range(1, randomSizeFlash.y);
                sizeEffect.z = Random.Range(1, randomSizeFlash.z);

                meshEffect.transform.localScale = sizeEffect;
                if (meshOtherEffect) meshOtherEffect.transform.localScale = sizeEffect;

            }
            audio.clip = sound.fireClip;
            if (AK || FlameThrower)
            {
                if (!audio.isPlaying) audio.Play();
            }
            else
            {
                audio.Play();
            }
            _flashTimmer = 0;

            if (!_infiniteAmmo) curentAmmo -= 1;

            /*    if (meshEffect != null)
                {
                    meshEffect.transform.localScale = Vector3.zero;
                    if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
                }*/
            //используется для коректировки пули чтоб она летела всегда прямо не зависимо от положение пушки
            if (Pistol || AK)
            {
                _rotationParticles = transform.rotation;
                _rotationParticles.x = RotationPartickleX;
                pointParticle.transform.rotation = _rotationParticles;
            }
            if (FireBullets)
            {
                LaunchFirebullets();
            }
            else
            {

                pointParticle.Play();
            }
            // if (!IsInvoking("Partickl") && curentAmmo != 0) Invoke("Partickl", 0.1f);


        }
    }

    //Для оружия вистрел которого не вызывается анимацией
    public void PlayAlways()
    {
        if (isFire && !isReload)
        {
            if (Minigun) RotationMinigun.Rotate(0, 10, 0);
            if (_shootTimer == 0) curentShoot = 0.3f;
            _shootTimer += Time.deltaTime;
            curentShoot -= Time.deltaTime;


            if (curentShoot < 0)
            {
                GameMeneger.entity.AddShoot();
                _shootTimer += Time.deltaTime;
                if (meshEffect)
                {

                    sizeEffect.x = Random.Range(1, randomSizeFlash.x);
                    sizeEffect.y = Random.Range(1, randomSizeFlash.y);
                    sizeEffect.z = Random.Range(1, randomSizeFlash.z);

                    meshEffect.transform.localScale = sizeEffect;
                    if (meshOtherEffect) meshOtherEffect.transform.localScale = sizeEffect;

                }
                curentShoot = speedFire;
                // if (animation != null) animation.CrossFade("Fire1");
                PlayerAnim.SetBool("IsShoot", true);
                //animation.CrossFade(FireAnim.name);
                audio.clip = sound.fireClip;
                //if (AK || FlameThrower)
                if (FlameThrower)
                {
                    if (!audio.isPlaying) audio.Play();
                }
                else
                {
                    audio.Play();
                }
                if (FlameThrower)
                {
                    if (!pointParticle.isPlaying) pointParticle.Play();

                }
                else
                {
                    if (FireBullets && FireBulletsCase)
                    {
                        LaunchFirebullets();
                        if (SecWeap != null) SecWeap.FireSecond();
                        // if (SecWeap != null && SecondHandWeap.entity.FireBulletsCase != null) SecWeap.LaunchFirebullets();
                    }
                    else
                    {
                        if (SecWeap != null) SecWeap.FireSecond();
                        pointParticle.Play();
                    }
                }

                if (!_infiniteAmmo) curentAmmo -= 1;
            }
            else if (curentShoot > curentShoot / 2 && !RPG && !FlameThrower && !Minigun)
            {
                //PlayerAnim.SetBool("IsShoot", false);
                // if (FlameThrower) pointParticle.Stop();
                //if ((AK || Minigun) && audio.clip.name == sound.fireClip.name) audio.Stop();
                //PlayerAnim.SetBool("IsShoot", false);
                if (meshEffect != null)
                {
                    meshEffect.transform.localScale = Vector3.zero;
                    if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
                }
                if (SecondHandWeap.entity != null) SecondHandWeap.entity.StopFire();
            }

        }

        else
        {
            PlayerAnim.SetBool("IsShoot", false);
            if (FlameThrower || Minigun) pointParticle.Stop();
            //if (audio.clip && (AK || Minigun) && audio.clip.name == sound.fireClip.name) audio.Stop();
            if (audio.clip && (Minigun) && audio.clip.name == sound.fireClip.name) audio.Stop();
            // PlayerAnim.SetBool("IsShoot", false);
            if (meshEffect != null)
            {
                meshEffect.transform.localScale = Vector3.zero;
                meshOtherEffect.transform.localScale = Vector3.zero;
            }
            if (SecWeap != null) SecWeap.StopFire();
        }
        //  PlayerAnim.SetBool("IsShoot", false);
    }
    void Partickl()
    {

        if (!_infiniteAmmo) curentAmmo -= 1;

        if (meshEffect != null)
        {
            meshEffect.transform.localScale = Vector3.zero;
            if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
        }
        _rotationParticles = transform.rotation;
        _rotationParticles.x = RotationPartickleX;
        pointParticle.transform.rotation = _rotationParticles;
        pointParticle.Play();
    }
    //Окончание выстрела, для оружия выстрел которого производится через анимацию
    public void FireFinish()
    {

        IsShoot = false;
        if (meshEffect != null)
        {
            meshEffect.transform.localScale = Vector3.zero;
            if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
        }

    }
    //Выполняется если мы перестали стрелять
    void StopFire()
    {
        if (FlameThrower || Minigun) pointParticle.Stop();
        //if (FlameThrower || Minigun || AK && audio.isPlaying)
        if (FlameThrower || Minigun && audio.isPlaying)
        {
            Debug.Log("Stop Audio");
            audio.Stop();
        }
        if (meshEffect != null)
        {
            meshEffect.transform.localScale = Vector3.zero;
            if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
        }
        if (SecWeap != null) SecWeap.StopFire();
        PlayerAnim.SetBool("IsShoot", false);
    }

    public void FireRpg()
    {

    }
    //вызывается в начале перезарядки
    void ReloadBegin()
    {
        if (meshEffect != null)
        {
            meshEffect.transform.localScale = Vector3.zero;
            if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
        }
        isReload = true;
        if (FlameThrower || Minigun) pointParticle.Stop();
        PlayerAnim.SetBool("IsShoot", false);
        if (SecWeap != null) SecWeap.StopFire();
        PlayerAnim.SetBool("Reload", true);
    }


    //Вызывается в конце анимации перезарядки
    public void Reload()
    {
        if (!_SecondWeapon)
        {
            if (SecondWeapon.entity) SecondWeapon.entity.Reload();
            isReload = false;
            //if (meshEffect != null)
            //{
            //    meshEffect.transform.localScale = Vector3.zero;
            //    if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
            //}
            //isReload = false;
            //curentTimeRecharge = rechargeTime;
            //curentAmmo = ammo;
            if (sound.rechargeClip != null) audio.clip = sound.rechargeClip;
            //audio.Play();
            //PlayerAnim.SetBool("Reload", false);

            if (audio) audio.Play();
            PlayerAnim.SetBool("Reload", false);
            // PlayerAnim.SetBool("IsShoot", false);
            // if (FlameThrower || Minigun) pointParticle.Stop();
            // if (!RPG && audio.isPlaying && audio.clip.name == sound.fireClip.name) audio.Stop();
            if (RPG)
            {
                for (int i = 0; i < ammo; i++)
                {
                    var t = Instantiate(RpgGranate, parent.position, Quaternion.identity) as GameObject;
                    t.transform.parent = parent;
                    t.transform.localPosition = PositionRocket[i];
                    t.transform.localRotation = RotationRocket[i];
                    t.transform.localScale = ScaleRocet[i];
                    GranadeList[i] = t.GetComponent<RpgGrenade>();
                    curentAmmo++;
                }
                return;
            }

            if (meshEffect)
            {

                meshEffect.transform.localScale = Vector3.zero;
                meshOtherEffect.transform.localScale = Vector3.zero;
            }
            //  if (audio.isPlaying) audio.Stop();
            //  curentTimeRecharge -= Time.deltaTime;
            // if (animation != null) animation.CrossFade("Reload");
            // PlayerAnim.SetBool("Reload", true);
            //  if (curentTimeRecharge < 0)
            //  {

            //  animation.CrossFade("Reload");
            //curentTimeRecharge = rechargeTime;

            curentAmmo = ammo;
            if (sound.rechargeClip != null) audio.clip = sound.rechargeClip;


        }
    }

    /* public void MainFire(bool fire)
     {
         if (fire)
         {
             if (Minigun) RotationMinigun.Rotate(0, 10, 0);
             if (_shootTimer == 0) curentShoot = 0.3f;
             _shootTimer += Time.deltaTime;
             curentShoot -= Time.deltaTime;


             if (curentShoot < 0)
             {
                 GameMeneger.entity.AddShoot();
                 _shootTimer += Time.deltaTime;
                 if (meshEffect)
                 {

                     sizeEffect.x = Random.Range(1, randomSizeFlash.x);
                     sizeEffect.y = Random.Range(1, randomSizeFlash.y);
                     sizeEffect.z = Random.Range(1, randomSizeFlash.z);

                     meshEffect.transform.localScale = sizeEffect;
                     if (meshOtherEffect) meshOtherEffect.transform.localScale = sizeEffect;

                 }
                 curentShoot = speedFire;
                 // if (animation != null) animation.CrossFade("Fire1");
                 PlayerAnim.SetBool("IsShoot", true);
                 //animation.CrossFade(FireAnim.name);
                 audio.clip = sound.fireClip;
                 if (AK || FlameThrower)
                 {
                     if (!audio.isPlaying) audio.Play();
                 }
                 else
                 {
                     audio.Play();
                 }
                 if (FlameThrower)
                 {
                     if (!pointParticle.isPlaying) pointParticle.Play();

                 }
                 else
                 {
                     if (FireBullets && FireBulletsCase)
                     {
                         LaunchFirebullets();
                     }
                     else
                     {
                         pointParticle.Play();
                     }
                 }

                 if (!_infiniteAmmo) curentAmmo -= 1;
             }
             else if (curentShoot > curentShoot / 2 && !RPG && !FlameThrower && !Minigun)
             {
                 PlayerAnim.SetBool("IsShoot", false);
                 // if (FlameThrower) pointParticle.Stop();
                 //if ((AK || Minigun) && audio.clip.name == sound.fireClip.name) audio.Stop();
                 //PlayerAnim.SetBool("IsShoot", false);
                 if (meshEffect != null)
                 {
                     meshEffect.transform.localScale = Vector3.zero;
                     if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
                 }
             }

         }

         else
         {
             PlayerAnim.SetBool("IsShoot", false);
             if (FlameThrower || Minigun) pointParticle.Stop();
             if (audio.clip && (AK || Minigun) && audio.clip.name == sound.fireClip.name) audio.Stop();
             // PlayerAnim.SetBool("IsShoot", false);
             if (meshEffect != null)
             {
                 meshEffect.transform.localScale = Vector3.zero;
                 meshOtherEffect.transform.localScale = Vector3.zero;
             }
         }
         //  PlayerAnim.SetBool("IsShoot", false);

     }



     */

    //public void Reload()
    //{
    //    PlayerAnim.SetBool("IsShoot", false);
    //    if (FlameThrower || Minigun) pointParticle.Stop();
    //    if (!RPG && audio.isPlaying && audio.clip.name == sound.fireClip.name) audio.Stop();
    //    if (meshEffect)
    //    {

    //        meshEffect.transform.localScale = Vector3.zero;
    //        meshOtherEffect.transform.localScale = Vector3.zero;
    //    }
    //    //  if (audio.isPlaying) audio.Stop();
    //    curentTimeRecharge -= Time.deltaTime;
    //    // if (animation != null) animation.CrossFade("Reload");
    //    PlayerAnim.SetBool("Reload", true);
    //    if (curentTimeRecharge < 0)
    //    {

    //        //  animation.CrossFade("Reload");
    //        curentTimeRecharge = rechargeTime;
    //        curentAmmo = ammo;
    //        audio.clip = sound.rechargeClip;
    //        audio.Play();
    //        PlayerAnim.SetBool("Reload", false);


    //    }
    //}

    //____________RPG________________
    //public void RpgFire(bool fire)
    //{

    //    if (fire)
    //    {
    //        curentShoot -= Time.deltaTime;

    //        if (curentShoot < 0)
    //        {
    //            GameMeneger.entity.AddShoot();
    //            PlayerAnim.SetBool("IsShoot", true);
    //            --curentAmmo;
    //            /* gameObject.GetComponentInChildren<RpgGrenade>().Launch();*/
    //            GranadeList[curentAmmo].Launch();

    //            //Debug.Log("ракета " + curentAmmo + "Пошла");
    //            curentShoot = speedFire;

    //        }
    //        else if (curentShoot > curentShoot - 0.3)
    //        {

    //            PlayerAnim.SetBool("IsShoot", false);
    //        }
    //        //  PlayerAnim.SetBool("IsShoot", false);
    //    }
    //    else
    //    {
    //        PlayerAnim.SetBool("IsShoot", false);
    //    }
    //    //  PlayerAnim.SetBool("IsShoot", false);
    //}

    //private void ReloadRPG()
    //{
    //    curentTimeRecharge -= Time.deltaTime;
    //    PlayerAnim.SetBool("IsShoot", false);
    //    PlayerAnim.SetBool("Reload", true);
    //    if (curentTimeRecharge < 0)
    //    {
    //        PlayerAnim.SetBool("Reload", false);

    //        for (int i = 0; i < ammo; i++)
    //        {
    //            var t = Instantiate(RpgGranate, parent.position, Quaternion.identity) as GameObject;
    //            t.transform.parent = parent;
    //            t.transform.localPosition = PositionRocket[i];
    //            t.transform.localRotation = RotationRocket[i];
    //            t.transform.localScale = ScaleRocet[i];
    //            GranadeList[i] = t.GetComponent<RpgGrenade>();
    //            curentAmmo++;
    //            curentTimeRecharge = rechargeTime;
    //        }
    //    }
    //}

    //______________PlazmaGun_______________
    void PlazmaGunFire(bool fire)
    {
        if (fire)
        {
            curentShoot -= Time.deltaTime;
            //if (curentShoot < 0.10)
            //{
            //    PlayerAnim.SetBool("IsShoot", true);
            //}
            if (curentShoot < 0)
            {
                GameMeneger.entity.AddShoot();
                var i = 0;
                // PlayerAnim.SetBool("IsShoot", true);
                curentShoot = speedFire;
                audio.clip = sound.fireClip;
                foreach (Transform o in transform)
                {
                    if (o.tag == "PlazmaBullet")
                    {
                        ++i;
                        // o.SetActive(true);
                        PlayerAnim.SetBool("IsShoot", true);
                        o.gameObject.SetActive(true);
                        curentAmmo--;
                        // Debug.Log(o.name);
                    }
                    if (i > 0) break;

                }

            }
            else if (curentShoot > curentShoot - 0.3)
            {

                PlayerAnim.SetBool("IsShoot", false);
            }
        }
        else
        {
            PlayerAnim.SetBool("IsShoot", false);
        }
        //  PlayerAnim.SetBool("IsShoot", false);
    }
    public void SetTypeWeaponToAnim()
    {
        GameMeneger.entity.PlayerAnimator.SetBool("IsShoot", false);
        GameMeneger.entity.PlayerAnimator.SetBool("Reload", false);
        GameMeneger.entity.PlayerAnimator.SetBool("Pistol", Pistol);
        GameMeneger.entity.PlayerAnimator.SetBool("UZI", UZI);
        GameMeneger.entity.PlayerAnimator.SetBool("ShootGun", ShootGun);
        GameMeneger.entity.PlayerAnimator.SetBool("AK", AK);
        GameMeneger.entity.PlayerAnimator.SetBool("RPG", RPG);
        GameMeneger.entity.PlayerAnimator.SetBool("PlazmaGun", PlazmaGun);
        GameMeneger.entity.PlayerAnimator.SetBool("Flamethrower", FlameThrower);
        GameMeneger.entity.PlayerAnimator.SetBool("Minigun", Minigun);
    }

    /* public void SwitchWeappon()
     {
         Debug.Log("Смена оружия");
         PlayerAnim.SetBool("IsShoot", false);
     }*/
    //public void SetTypeWeaponToAnim()
    //{

    //    PlayerAnim.SetBool("Pistol", Pistol);
    //    PlayerAnim.SetBool("UZI", UZI);
    //    PlayerAnim.SetBool("ShootGun", ShootGun);
    //    PlayerAnim.SetBool("AK", AK);
    //    PlayerAnim.SetBool("RPG", RPG);
    //    PlayerAnim.SetBool("PlazmaGun", PlazmaGun);
    //    PlayerAnim.SetBool("Flamethrower", FlameThrower);
    //    PlayerAnim.SetBool("Minigun", Minigun);
    //}

    public void GetLaser()
    {
        foreach (Transform o in gameObject.transform)
        {
            if (o.tag == "Laser") Laser = o;

        }
    }

    //_________________Firebullets______________
    void LaunchFirebullets()
    {
        foreach (Transform fire in FireBulletsCase)
        {
            fire.gameObject.SetActive(true);
            break;
        }
    }

}





[System.Serializable]
public class SoundWeapon
{
    public AudioClip fireClip;
    public AudioClip fireCicleClip;
    public AudioClip rechargeClip;
    public AudioClip finisRechargeClip;
    public AudioClip empty;

}
[System.Serializable]
public class HandAtach
{
    public Vector3 posLeftHand;
    public Vector3 rotLeftHand;
    public Vector3 posRightHand;
    public Vector3 rotRightHand;
    public Transform pivotHandLeft;
    public Transform pivotHandRight;

    public void ConfingHand()
    {
        posLeftHand = pivotHandLeft.localPosition;
        posRightHand = pivotHandRight.localPosition;
        rotLeftHand = pivotHandLeft.localEulerAngles;
        rotRightHand = pivotHandRight.localEulerAngles;

    }
    public void LoadConfingHands()
    {
        pivotHandLeft.localPosition = posLeftHand;
        pivotHandRight.localPosition = posRightHand;
        pivotHandLeft.localRotation = Quaternion.Euler(rotLeftHand);
        pivotHandRight.localRotation = Quaternion.Euler(rotRightHand);

    }

}
