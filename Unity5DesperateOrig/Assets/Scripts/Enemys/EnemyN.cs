using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
//using UnityEditor;
//using UnityEditor;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class EnemyN : MonoBehaviour
{
    [Header("Основные параметры")]
    public float Helth;

    public int Experience;
    public int Score;
    public int Damage;
    [Tooltip("На этом растоянии нападем на зомби")]
    public float AgrRadius;
    [Tooltip("На этом растоянии будем бить")]
    public float DistToAtk;
    [Tooltip("Задержка для синхронизации анимации и нанесония урона")]
    public float DelayAttackAnim;
    [Tooltip("Скорость атаки, больше - медленей")]
    public float AttackSpeed;

    public float RotationSpeed = 5;

    private float _timmer = 0;




    [Header("Дополнительные объекты")]
    [Tooltip("Время в секундах за которое будет проверятся сколько аргесивных мобов на сцене чтоб было более плавно, время для каждого моба будет выбиратся случайно")]
    public float TimeChk = 5;
    [Tooltip("Для коректировки движения и незастревания в текстурах, надо указать землю по которой сможем бродить")]
    public GameObject Ground;

    [Tooltip("Лед")]
    public GameObject Ice;

    [Header("Горение персонажа")]
    [Tooltip("Кормим огонь на персонаже")]
    public ParticleSystem Fire;
    [Tooltip("Время сколько будет гореть")]
    public float BurningTime = 4;
    public bool isBurning;
    [Tooltip("Урон при горении")]
    public int DamageWhenBurn = 10;
    [Tooltip("Наносить урон каждые")]
    public int DamageEvery = 1;
    private float _damageBurnTimer;
    private float _currentburningTime;
    [Header("Шанс дропа бафа")]
    [Tooltip("Вероятность 1 из ...")]
    public int Chance = 10;

    private float _timer;
    [Header("случайное брожение")]
    [Tooltip("Радиус для случайного перемещения в пределах префаба")]
    private float _parrentRadius;

    private Transform _startPosition;
    private Transform _stopPosition;
    [Tooltip("Растояние на котором бедем менять место дял передвижения, чтоб не стопились")]
    public float DistToChangeDirection = 6;
    private Vector3 _randPositions;
    private float _randPosDist;
    public Transform Target;
    private AutoTarget _autoTarget;
    private NavMeshAgent _agent;
    private float _currentDist;
    private Player _player;
    private Rigidbody _rigibody;
    [Tooltip("Меш со скриптом")]
    public EnemyVFX _bloodVFX;
    [Tooltip("Вешаем обэект с кровью")]
    public VFXManeger _VfxManeger;



    [Header("Тип анимации")]
    public bool Meckanim;

    public string WalkAnimMec = "Run";
    public string AttackAnimMec = "Attack";
    public string DeathAnimMec = "Death";
    public string HitAnimMac = "Hit";
    public string ImpulseDeathMec = "ImpulseDeath";


    [Header("Анимация")]
    public AnimationClip DeathAnim;

    public float DeathAnimSpeed = 1;
    public AnimationClip Hit;
    public float HitAnimSpeed = 1;
    public AnimationClip ImpulseDeath;
    public float ImpulseDeathAnimSpeed = 1;
    [Tooltip("Типы передвижения")]
    public AnimationClip WalkAnim1;
    public float WalkAnim1Speed = 1;
    public AnimationClip WalkAnim2;
    public float WalkAnim2Speed = 1;
    [Tooltip("Основной тип передвижения")]
    public AnimationClip WalkAnim;
    public AnimationClip AttackAnim1;
    public float AttackAnim1Speed = 1;
    public AnimationClip AttackAnim2;
    public float AttackAnim2Speed = 1;
    private Animation _animation;
    private Animator _animator;
    Ray _ray;
    RaycastHit _hit;

    [Space(10)]
    [Header("Статусы дебафов")]
    public bool IsFrize;

    public bool Turret;

    [Header("Действия")]
    [Tooltip("Если активно то нападаем на цель")]
    public bool IsAgr;
    [Tooltip("активирируется режим выживания, мобы агрятся не зависимо от того насколько билизко к персонажу")]
    public bool isSurvival;

    private bool _haveRandPos;
    private bool _isAttack;
    private bool _isDead;
    private bool _isMove;
    public bool _wasAdd = false;
    [Header("Таймер проверки ")]
    public float StopTimmer = 3;
    private float _stopTimmer;

    private bool crit;



    //вызываем при старте
    private void Start()
    {
        _startPosition = GameObject.FindGameObjectWithTag("StartNavigations").transform;
        _stopPosition = GameObject.FindGameObjectWithTag("StopNavigations").transform;


        _currentburningTime = BurningTime;
        if (!Meckanim && Random.Range(1, 3) == 1)
        {
            WalkAnim = WalkAnim1;
        }
        else
        {
            WalkAnim = WalkAnim2;
        }
        Ground = GameObject.FindGameObjectWithTag("Ground");
        TimeChk = Random.Range(1, TimeChk);
        _parrentRadius = gameObject.transform.parent.GetComponent<PointSpawn>().Range;
        Target = GameObject.FindGameObjectWithTag("PlayerMesh").transform;
        // Target = GameObject.FindGameObjectWithTag("Player").transform;
        _autoTarget = Target.GetComponentInChildren<AutoTarget>();
        _bloodVFX = gameObject.GetComponentInChildren<EnemyVFX>();
        _VfxManeger = GameObject.FindGameObjectWithTag("VFX").GetComponent<VFXManeger>();
        //растояния перемножаются, так как будем вычитывать растояние до цели без вычитания корня(быстрее)
        AgrRadius *= AgrRadius;
        DistToAtk *= DistToAtk;
        _agent = gameObject.GetComponent<NavMeshAgent>();
        if (Target == null) Debug.Log("Объекту нужно указать цель");
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!Meckanim)
        {
            _animation = gameObject.GetComponent<Animation>();
        }
        else
        {
            _animator = gameObject.GetComponent<Animator>();
        }
        _timmer = AttackSpeed;
        _timer = TimeChk;

        if (!Meckanim)
        {
            if (WalkAnim1) _animation[WalkAnim1.name].speed = WalkAnim1Speed;
            if (WalkAnim2) _animation[WalkAnim2.name].speed = WalkAnim2Speed;
            if (DeathAnim) _animation[DeathAnim.name].speed = DeathAnimSpeed;
            if (AttackAnim1) _animation[AttackAnim1.name].speed = AttackAnim1Speed;
            if (AttackAnim2) _animation[AttackAnim2.name].speed = AttackAnim2Speed;
            if (ImpulseDeath) _animation[ImpulseDeath.name].speed = ImpulseDeathAnimSpeed;
            if (Hit) _animation[Hit.name].speed = HitAnimSpeed;
        }
    }

    void Update()
    {
        //Периодически мобы застряют на местах при этом ничего им не мешает передвигатся, сжелаем проверку по времени чтоб если это случилось то мы просто сменим направление
        if (_agent.desiredVelocity.sqrMagnitude < 0.1)
        {

            _stopTimmer += Time.deltaTime;
            if (_stopTimmer > StopTimmer) _haveRandPos = false;
        }
        else if (_stopTimmer != StopTimmer)
        {
            _stopTimmer = 0;
        }

        Debug.DrawLine(transform.position, _randPositions);
        if (isBurning) Burning();

        if (isSurvival)
        {
            _timer -= Time.deltaTime;
            if (_timer < 0 && GameMeneger.entity.CountAgrMobs > GameMeneger.entity._currentAgrMobs)
            {
                // Debug.Log("Нападу " + GameMeneger.entity.CountAgrMobs + "  " + GameMeneger.entity._currentAgrMobs);
                IsAgr = true;
                AddAgrMobs();
            }
            else if (_timer < 0) _timer = TimeChk;
        }
        if (_haveRandPos) _randPosDist = (_randPositions - transform.position).sqrMagnitude;
        if (_timmer > 0) _timmer -= Time.deltaTime;
        _currentDist = (Target.position - transform.position).sqrMagnitude;
        Check();

        if (GameMeneger.entity.Frieze)
        {
            IsFrize = true;

        }
        else
        {
            if (!IsInvoking("UnfriezeWalk")) Invoke("UnfriezeWalk", 3);
        }


        if (IsFrize)
        {
            _isMove = false;
            _isAttack = false;
            Frize();
        }
        /* else
         {
             UnFrize();
         }*/
        if (_isMove) Move();
        if (_isAttack) Attack();
        // if (_isDead) DeathEnemy();
    }



    //метод управляет переменными которые будут решать что делать нашему мобу
    void Check()
    {
        //меняем положение моба
        if (_haveRandPos && _randPosDist < DistToChangeDirection)
        {
            _haveRandPos = false;
        }
        //если не хардкорный режим, то останемся в живых если убежим
        if (_currentDist > AgrRadius)
        {
            // if (!isSurvival)
            // {
            // if (IsAgr) GameMeneger.entity.CurrentAgrMobs--;
            if (GameMeneger.entity.CurrentAgrMobs > GameMeneger.entity.CountAgrMobs)
            {
                IsAgr = false;
                AddAgrMobs();
            }
            //  }
        }
        //если мы на дистанции для атаки то даем добро атаковать
        if (_currentDist < DistToAtk && !Player.entity.isDeath)
        {
            TurnToTarget();
            _isMove = false;
            if (_timmer < 0)
            {
                _timmer = AttackSpeed;
                _isAttack = true;

            }
            else
            {
                _isAttack = false;
            }

        }
        else if (_currentDist < AgrRadius && !Player.entity.isDeath)
        {
            _isAttack = false;
            _isMove = true;
            _haveRandPos = false;
            // if (!IsAgr) GameMeneger.entity.CurrentAgrMobs++;
            IsAgr = true;
            AddAgrMobs();
        }
        else
        {
            _isMove = true;
        }

    }
    //Задаем случайные координаты для брожения
    void GetRandPlace()
    {

        bool getPosition = false;
        do
        {
            //Debug.Log("параметр вначале" + getPosition);
            //   _randPositions = new Vector3(transform.parent.position.x + Random.Range(-_parrentRadius, _parrentRadius), transform.parent.position.y,
            //  transform.parent.position.x + Random.Range(-_parrentRadius, _parrentRadius));
            _randPositions = new Vector3(Random.Range(_startPosition.position.x, _stopPosition.position.x), transform.parent.position.y, Random.Range(_startPosition.position.z, _stopPosition.position.z));
            _ray = new Ray(_randPositions, -Vector3.up);
            if (Physics.Raycast(_ray, out _hit, 100))
            {
                if (_hit.transform.tag == Ground.tag)
                {
                    getPosition = true;
                    _randPositions = _hit.point;
                }
                //  Debug.Log("наша земля " + Ground.name + " куда попали " + _hit.transform.name);
            }

            // Debug.Log("параметр в конце" + getPosition);
        } while (!getPosition);

        _haveRandPos = true;
    }

    void Move()
    {
        if (!_isDead && _isMove && !_isAttack && IsAgr && !Player.entity.isDeath)
        {
            if (!Meckanim)
            {
                _animation.CrossFade(WalkAnim.name);
            }
            else
            {
                _animator.SetBool(WalkAnimMec, true);
            }
            _agent.SetDestination(Target.position);
        }
        else if (!_isDead && _isMove && !_isAttack && !IsAgr || Player.entity.isDeath)
        {
            if (!_haveRandPos) GetRandPlace();
            if (!Meckanim)
            {
                _animation.CrossFade(WalkAnim.name);
            }
            else
            {
                _animator.SetBool(WalkAnimMec, true);
            }


            _agent.SetDestination(_randPositions);
        }
        else
        {
            if (Meckanim) _animator.SetBool(WalkAnimMec, false);
            if (!_isDead) _agent.Stop();
        }
    }

    void Attack()
    {
        if (_isAttack && !_isMove && !_isDead)
        {
            if (Meckanim) _animator.SetBool(WalkAnimMec, false);
            if (!Meckanim)
            {
                int r_anim;
                r_anim = AttackAnim2 ? Random.Range(1, 3) : 1;
                if (r_anim == 2)
                {
                    _animation.CrossFade(AttackAnim2.name);
                }
                else
                {
                    _animation.CrossFade(AttackAnim1.name);
                }
            }
            else
            {
                _animator.SetBool(AttackAnimMec, true);
            }

            if (!IsInvoking("DelayAttack")) Invoke("DelayAttack", DelayAttackAnim);
        }
    }


    void DelayAttack()
    {
        if (Meckanim) _animator.SetBool(AttackAnimMec, false);
        _player.Damage(Damage);
    }

    //Поскольку урон наносится у нас через вызов, то бывают ситуации что моб смотрит в сторону но лупит нас, по этому мы повернем его "лицом" к цели
    void TurnToTarget()
    {
        if (!IsFrize && !_isDead) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Target.transform.position - transform.position),
                RotationSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.LookRotation(Target.position - transform.position);
    }

    public void DamageOffParticle(GameObject obj)
    {
        int r = Random.Range(0, VFXManeger.Blood.bloodEmiters.Length);
        VFXManeger.Blood.transformEmiter.position = transform.position + Vector3.up;
        VFXManeger.Blood.bloodEmiters[r].Play();

        //Blood.transformParticles.position = actorTransform.position+Vector3.up;
        //Blood.transformParticles.rotation = actorTransform.rotation;
        //отниаем урон от жизни павна
        if (obj.tag == "Weapon")
        {
            // Helth -= Weapon.entity.damage;
            Helth -= GetDamage();
            GameMeneger.entity.AddHit();
        }
        //Debug.Log(Weapon.entity.damage);
        if (obj.tag == "Turet")
        {
            Helth -= Turet.entity.damage;
            //    GameMeneger.entity.AddHit();
        }
        if (obj.tag == "Plazma")
        {
            GameMeneger.entity.AddHit();
            Helth -= 1000;
        }
        if (obj.tag == "FlameThrower") DamageFromFlamethrower();
        // if (filter == "PlazmaBullet") Helth -= Weapon.entity.damage;
        TakeHit();
        //проверка на наличие жизни
        if (Helth <= 0)
        {
            _isDead = true;
            DeathEnemy();
        }

    }

    void DamageFromFlamethrower()
    {
        GameMeneger.entity.AddHit();
        // Helth -= Weapon.entity.damage;
        Helth -= GetDamage();
        isBurning = true;
        _currentburningTime = BurningTime;
        TakeHit();
    }

    void Burning()
    {
        if (!Fire.isPlaying) Fire.Play();
        _currentburningTime -= Time.deltaTime;
        if (_currentburningTime > 0)
        {
            // Fire.Play();

            _damageBurnTimer += Time.deltaTime;
            if (_damageBurnTimer > DamageEvery)
            {
                _damageBurnTimer = 0;
                Helth -= DamageWhenBurn;
            }

        }
        else
        {
            isBurning = false;
        }
    }
    public void DamageFromRpg(int damage)
    {
        GameMeneger.entity.AddHit();
        int r = Random.Range(0, VFXManeger.Blood.bloodEmiters.Length);
        VFXManeger.Blood.transformEmiter.position = transform.position + Vector3.up;
        VFXManeger.Blood.bloodEmiters[r].Play();
        Helth -= damage;
        if (Helth <= 0)
        {
            _isDead = true;
            DeathEnemy();
        }

    }

    void AddAgrMobs()
    {
        //if (_isDead)
        //{
        //    GameMeneger.entity._currentAgrMobs--;
        //    return;
        //}
        if (_wasAdd != IsAgr && !_isDead)
        {
            _wasAdd = IsAgr;
            if (IsAgr) GameMeneger.entity._currentAgrMobs++;
            if (!IsAgr) GameMeneger.entity._currentAgrMobs--;
        }

    }

    void OnParticleCollision(GameObject other)
    {
        // Debug.Log(other.tag);
        DamageOffParticle(other);

    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "AutoTarget" && !_isDead && !Weapon.entity.Target)
        {
            //  Debug.Log("Назначена цель");
            Weapon.entity.Target = gameObject;
        }
    }

    void OnTriggerExit(Collider col)
    {


        if (col.tag == "AutoTarget" && Weapon.entity.Target != null &&
            Weapon.entity.Target.GetInstanceID() == gameObject.GetInstanceID())
        {
            //  Debug.Log("Убрана цель");
            Weapon.entity.Target = null;
        }
    }
    void OnTriggerEnter(Collider col)
    {

        //  Debug.Log(col.name);
        if (col.tag == "PlazmaBullet")
        {
            // GameMeneger.entity.AddHit();
            // Helth -= Weapon.entity.damage;
            Helth -= GetDamage();
            TakeHit();
        }
        if (col.tag == "Fireblast")
        {
            // GameMeneger.entity.AddHit();
            Helth -= col.GetComponent<PlazmaBulletCtrl>().Damage;
        }
        if (Helth <= 0)
        {
            _isDead = true;
            DeathEnemy();
        }
    }
    public void DeathEnemy()
    {

        if (!Meckanim)
        {
            // if (crit) Debug.Log("Impusle death");
            if (crit)
            {
                _animation.CrossFade(ImpulseDeath.name);
            }
            else
            {
                _animation.CrossFade(DeathAnim.name);
            }
        }
        else
        {
            if (crit)
            {
                _animator.SetBool(ImpulseDeathMec, true);
            }
            else
            {
                _animator.SetBool(DeathAnimMec, true);
            }
        }
        // TakeABonus();
        transform.GetComponent<Collider>().enabled = false;
        _agent.enabled = false;
        //animationActor.animation.CrossFade(animationActor.death.name);
        //audioSource.clip = sound.death[Random.Range(0, sound.death.Length)];
        //audioSource.Play();
        //GameMeneger.entity.countEnemy--;

        _bloodVFX.isDeath = true;
       // TakeABonus();
        if (IsFrize) Ice.GetComponent<IceSC>().DisableOnDeath();
        if (!IsInvoking("Death"))
        {
            GameMeneger.entity.curentEnemy--;
            GameMeneger.entity.score += Score;
            GameMeneger.entity.xp += Experience * GameMeneger.entity.mulXp;
            if (Weapon.entity.Target != null &&
           Weapon.entity.Target.GetInstanceID() == gameObject.GetInstanceID())
            {
                //  Debug.Log("Убрана цель");
                Weapon.entity.Target = null;
            }
            //  _autoTarget.DeleteFromTargetList(gameObject);
            if (GameMeneger.entity.Turret) Turet.entity.target = null;
            Invoke("Death", 5);
        }
        //  if (!_isDead) GameMeneger.entity._currentAgrMobs--;
        _isDead = true;

    }

    void TakeHit()
    {
        if (crit)
        {
            if (Helth > 0 && !_isDead)
            {
                crit = false;
                if (Meckanim)
                {
                    _animator.SetBool(HitAnimMac, true);
                    if (!IsInvoking("StopHit")) Invoke("StopHit", 0.5f);
                }
                else
                {
                    _animation.CrossFade(Hit.name);
                }
            }

        }
    }
    //Меняет значение для анимации удара
    void StopHit()
    {
        _animator.SetBool(HitAnimMac, false);
    }

    //Случайно выпадает бонус
    void TakeABonus()
    {

        if (Random.Range(0, Chance) == 1)
        {
            var _bonus = GameMeneger.entity._BonusList;
            GameObject v = _bonus[Random.Range(0, _bonus.Count())];

            v.GetComponent<EquipmentItems>().ActivateBuff(transform.position);
            //v.SendMessage("ActivateBuff", transform.position);
        }
    }


    //заморозка мобов
    public void Frize()
    {
        if (Meckanim)
        {
            _animator.SetBool(WalkAnimMec, false);
            _animator.SetBool(AttackAnimMec, false);
        }
        else
        {
            _animation.Stop();
        }
        Ice.SetActive(true);
        if (_agent.velocity.sqrMagnitude != 0) _agent.Stop();
    }

    public void UnFrize()
    {

        //IsFrize = false;

    }

    void UnfriezeWalk()
    {
        IsFrize = false;
        if (_agent.enabled) _agent.Resume();
    }


    public void GetDamage(int dam)
    {
        Helth -= dam;
        if (IsFrize) Helth -= 50;
        if (Helth <= 0)
        {
            _isDead = true;
            DeathEnemy();
        }
    }
    void Death()
    {

        GameMeneger.entity.AddKilledMobs();
        GameMeneger.entity._currentAgrMobs--;
        Destroy(gameObject);
    }


    //Поскольку у оружия есть критический урон то его будем прощитывать сдесь
    float GetDamage()
    {

        if (Random.Range(1, 100) <= Weapon.entity.ChancheCritDamage)
        {
            float damage = Weapon.entity.damage * (Weapon.entity.CritDamage / 100);
            crit = true;
            return damage;
        }

        return Weapon.entity.damage;

    }

}
