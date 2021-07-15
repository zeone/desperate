using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;
using System.Collections;
//класс вооружения
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SecondWeapon : Actor
{
#pragma strict
    public Sprite icon;
    [Header("Confing")]
    public Sprite WeaponIcon;
    public float damage;
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
    [Space(10)]

    public Transform Laser;
    public static SecondWeapon entity
    {
        get;
        set;
    }

    private Quaternion _rotationParticles;
    public float RotationPartickleX;

    private bool IsShoot;
    //private void Awake()
    //{

    //}

    /* void OnEnable()
     {

         // UIsc.entity.ChangeWeapIcon(WeaponIcon);
     }*/
    private void Start()
    {

        entity = this;

        /* if (_SecondWeapon && _SecondWeapon.activeInHierarchy) _secondWeapon = _SecondWeapon.GetComponent<Weapon>();
         //Инициализируем масив для гранат
         if (RPG) GranadeList = new RpgGrenade[ammo];
         //Получаем аниматор персонажа
         //PlayerAnim = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
         PlayerAnim = GameMeneger.entity.PlayerAnimator;
         //задаем положение персонажа в зависимотси от типа оружия
         //  SetTypeWeaponToAnim();
         */
        /*  transform = gameObject.GetComponent<Transform>();
          if (!RPG)
          {
              // curentAmmo = ammo;

              var an = gameObject.GetComponent<Animation>();
              if (an != null) animation = an;
              audio = gameObject.GetComponent<AudioSource>();
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
          }*/
        YCorrector = transform.position.y;
        // SetTypeWeaponToAnim();
        // GetLaser();
        if (Laser) Laser.gameObject.SetActive(false);
    }

    //private void On_JoystickMove(MovingJoystick move)
    //{
    //    isFire = true;


    //}

    //private void On_JoystickMoveEnd(MovingJoystick move)
    //{

    //    isFire = false;
    //    _shootTimer = 0;
    //}

    private void Update()
    {
        /*     //перепроверяем чтоб наши параметры были заполнеными
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
     #endif*/
        // Debug.Log(isFire);
        FireBullets = GameMeneger.entity.FireBullets;
        _infiniteAmmo = GameMeneger.entity.SlowTime;
        //  if (_SecondWeapon && _SecondWeapon.activeInHierarchy) _secondWeapon.isFire = isFire;
        /*  if (!Player.entity.isDeath)
          {

              ChoseFire();
          }*/

        /* if (buffer != null)
         {
             handAtach.ConfingHand();
             buffer.handAtach.posLeftHand = handAtach.posLeftHand;
             buffer.handAtach.posRightHand = handAtach.posRightHand;
             buffer.handAtach.rotLeftHand = handAtach.rotLeftHand;
             buffer.handAtach.rotRightHand = handAtach.rotRightHand;
             buffer.posOnParent = transform.localPosition;

         }*/

        //Оружие будет смотреть на цель если она в досягаемости
        if (Target && !isReload)
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

 //  public void ChoseFire()
 //   {
 //       if (curentAmmo > 0)
 //       {
 //           /* if (PlazmaGun)
 //            {

 //                PlazmaGunFire(isFire);
 //            }
 //            else
 //            {*/

 //           MainFire(isFire);
 //           //}
 //       }
 //       else if (curentAmmo <= 0)
 //       {
 //           /*    if (RPG)
 //               {
 //                   ReloadRPG();
 //               }
 //               else
 //               {*/
 //           //Reload();
 //           if (meshEffect != null)
 //           {
 //               meshEffect.transform.localScale = Vector3.zero;
 //               if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
 //           }
 //           isReload = true;
 //           if (FlameThrower || Minigun) pointParticle.Stop();
 //           PlayerAnim.SetBool("IsShoot", false);
 //           PlayerAnim.SetBool("Reload", true);
 //           //  }

 //       }
 //       else
 //       {
 //           if (!RPG && meshEffect != null)
 //           {
 //               meshEffect.transform.localScale = Vector3.zero;
 //               if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
 //           }
 //       }

 //   }

    public void FireSecond()
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

    public void StopFire()
    {
        if (meshEffect != null)
        {
            meshEffect.transform.localScale = Vector3.zero;
            if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
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
            if (FlameThrower || Minigun) pointParticle.Stop();
            if (meshEffect != null)
            {
                meshEffect.transform.localScale = Vector3.zero;
                if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
            }
            PlayerAnim.SetBool("IsShoot", false);
        }
    }

    public void Fire()
    {
        if (isFire)
        {
            GameMeneger.entity.AddShoot();
            if (RPG)
            {
                Debug.Log("RPG Launch");
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


            if (!_infiniteAmmo) curentAmmo -= 1;

            if (meshEffect != null)
            {
                meshEffect.transform.localScale = Vector3.zero;
                if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
            }
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

    public void FireFinish()
    {

        IsShoot = false;
        if (meshEffect != null)
        {
            meshEffect.transform.localScale = Vector3.zero;
            if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
        }

    }



    public void Reload()
    {

        // isReload = false;
        //if (meshEffect != null)
        //{
        //    meshEffect.transform.localScale = Vector3.zero;
        //    if (meshOtherEffect) meshOtherEffect.transform.localScale = Vector3.zero;
        //}
        //isReload = false;
        //curentTimeRecharge = rechargeTime;
        //curentAmmo = ammo;
        //audio.clip = sound.rechargeClip;
        //audio.Play();
        //PlayerAnim.SetBool("Reload", false);

        //if (audio) audio.Play();
        //PlayerAnim.SetBool("Reload", false);
        // PlayerAnim.SetBool("IsShoot", false);
        // if (FlameThrower || Minigun) pointParticle.Stop();
        // if (!RPG && audio.isPlaying && audio.clip.name == sound.fireClip.name) audio.Stop();
        /*if (RPG)
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
        */
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

        // curentAmmo = ammo;
        //if (sound.rechargeClip != null) audio.clip = sound.rechargeClip;



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
    /* void PlazmaGunFire(bool fire)
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
         GameMeneger.entity.PlayerAnimator.SetBool("Pistol", Pistol);
         GameMeneger.entity.PlayerAnimator.SetBool("UZI", UZI);
         GameMeneger.entity.PlayerAnimator.SetBool("ShootGun", ShootGun);
         GameMeneger.entity.PlayerAnimator.SetBool("AK", AK);
         GameMeneger.entity.PlayerAnimator.SetBool("RPG", RPG);
         GameMeneger.entity.PlayerAnimator.SetBool("PlazmaGun", PlazmaGun);
         GameMeneger.entity.PlayerAnimator.SetBool("Flamethrower", FlameThrower);
         GameMeneger.entity.PlayerAnimator.SetBool("Minigun", Minigun);
     }*/

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
    /*
        public void GetLaser()
        {
            foreach (Transform o in gameObject.transform)
            {
                if (o.tag == "Laser") Laser = o;

            }
        }*/

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




