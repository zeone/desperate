using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class GameMeneger : MonoBehaviour
{

    [Header("Параметры пользователя")]
    public Animator PlayerAnimator;
    [Space(10)]
    [Header("Info Raund")]
    [Tooltip("Номер волны начинается с 0")]
    public int numWave = 0;
    public int xp;
    [Tooltip("Количество опыта для окончания уровня, также нужно для заполнения полоски опыта")]
    public int TargetXp;
    [Tooltip("Количество врагов которые были в волне")]
    public int countEnemy;
    public int score;
    [Tooltip("Количество очков для окончания уровня, также нужно для заполнения полоски очков")]
    public int TargetScore;
    [Header("Bonus Pack")]
    public GameObject[] _BonusList;

    [Header("Таймер")]
    public int Minutes;

    public int Seconds;
    private float _startTime;

    [Header("Количество выстрелов и попаданий")]
    public int ShootCount;

    public float HitCount;

    public float KilledMobs;

    [Header("Волны")]
    [Tooltip("Задаем количество волн")]
    public Wawe[] Wawes;

    [Tooltip("Максимально количество врагов на сцене, для контроля нагрузки")]
    public int maxCountEnemyInScene = 15;

    [Tooltip("Количество боссов в сцене")]
    private int countBossEnemyInWawe;

    [Tooltip("Количество простых мобов на сцене")]
    private int countMainMobsInWawe;

    [Tooltip("Количество мобов которые будут бить персонажа")]
    public int CountAgrMobs = 10;

    public int _currentAgrMobs;

    public bool CreationDone = false;
    [Tooltip("Количество врагов которые появлятся в волне, задается автоматически")]
    public int countEnemyInWawe;
    [Tooltip("Сколько врагов на сцене")]
    public int curentEnemy;
    [Tooltip("Сколько будем отдыхать между волнами")]
    public float timeRelax;
    public float curentTimeRelax;
    [Header("Множители")]
    [Tooltip("Множители опыта")]
    public int mulXp = 1;
    [Tooltip("добавляет проценты к основному множителю характеристик мобов, нужен если базового мало")]
    public float addHealth;
    [Tooltip("Данное число будет умножатся на количество волн, и прибавлятся к количеству мобов")]
    public int addCount = 1;
    [Header("Задерка перед появлением")]
    [Tooltip("Задержка перед спавном следующего моба")]
    public float delaySpawn = 1;
    public float curentDallySpawn;
    [Header("Other Properety")]
    [Tooltip("Основные противники")]
    public GameObject enemy;
    [Tooltip("Минибоссы")]
    public GameObject bossEnemy;

    public TimeManeger time;

    public List<GameObject> enemyPool;
    public List<GameObject> enemyActive;
    public GameObject[] pointSpawn;
    public bool isCreate;
    public bool isSpawn;
    public bool _battlebegin = true;
    private bool _isFilled = false;
    public bool _isSurvival = false;
    public float _mulHealth = 0;

    [Header("Активируемые бонусы")]
    public bool Frieze;

    public bool Turret;
    public bool Shield;
    public bool SlowTime;
    public bool FireBullets;

    [Header("джойстики")]
    public GameObject RJoy;
    public GameObject LJoy;
    public GameObject Arrow;



    [Header("Temp")]
    public GameObject mobs;

    private GameObject _UI;
    public int[] count;
    /// <summary>
    /// Возращает количество мобов которые на данный момент атакуют персонажа, чтоб убрать надо отправить значение с -
    /// </summary>
    public int CurrentAgrMobs
    {
        get { return _currentAgrMobs; }
        set { _currentAgrMobs += value; }
    }


    //Управляет отдыхом
    public bool IsRelax = false;


    public static GameMeneger entity
    {
        get;
        set;
    }



    void Awake()
    {
        entity = this;
        _startTime = Time.time;
        _UI = GameObject.FindGameObjectWithTag("UI");

    }
    void Start()
    {



        pointSpawn = GameObject.FindGameObjectsWithTag("Spawner");
        //PlayerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
        FillBonusList();

#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY
        RJoy.SetActive(true);
        LJoy.SetActive(true); 
        Arrow.SetActive(true);
#else
        RJoy.SetActive(false);
        LJoy.SetActive(false);
        Arrow.SetActive(false);
#endif
    }
    void Update()
    {



        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Menu))
            {
                _UI.SendMessage("ShowMenu");
                return;
            }
        }

        //   if (CreationDone && _currentAgrMobs < CountAgrMobs && _isSurvival) SetAgresive();
        if (Wawes.Length != 0)
        {
            if (!_isFilled) FillParameters();
        }
        else
        {
            Debug.LogWarning("Не задана ни одна волна, мобы не появятся");
        }
        SpawnControll();


    }



    ////заполняем параметры в зависимости от параметров в волне
    //void FillParameters()
    //{
    //    if (numWave < Wawes.Length)
    //    {

    //        countMainMobsInWawe = Wawes[numWave].CountMainMobs;
    //        countBossEnemyInWawe = Wawes[numWave].CountBoss;
    //        enemy = Wawes[numWave].MainMob;
    //        if ((numWave + 1) < Wawes.Length)
    //        {
    //            bossEnemy = Wawes[numWave + 1].MainMob;
    //        }
    //        else
    //        {
    //            bossEnemy = Wawes[numWave].MainMob;
    //        }
    //        countEnemyInWawe = countBossEnemyInWawe + countMainMobsInWawe;

    //        _isFilled = true;
    //    }
    //    else if (_isSurvival)
    //    {
    //        Debug.Log("Начался режим выживания");
    //        _mulHealth = numWave * addHealth;
    //        var t = Wawes[UnityEngine.Random.Range(0, Wawes.Length)];
    //        countBossEnemyInWawe = t.CountBoss + numWave * addCount;
    //        countMainMobsInWawe = t.CountMainMobs + numWave * addCount;
    //        enemy = t.MainMob;
    //        var r = Wawes[UnityEngine.Random.Range(0, Wawes.Length)];
    //        bossEnemy = r.MainMob;
    //        countEnemyInWawe = countBossEnemyInWawe + countMainMobsInWawe;

    //        _isFilled = true;
    //    }
    //    else
    //    {

    //        _isFilled = true;
    //        Debug.Log("Волны закончились");
    //    }
    //}



    //заполняем параметры в зависимости от параметров в волне
    void FillParameters()
    {
        if (numWave < Wawes.Length)
        {

            countMainMobsInWawe = Wawes[numWave].CountMainMobs;
            countBossEnemyInWawe = Wawes[numWave].CountBoss;
            enemy = Wawes[numWave].MainMob;
            bossEnemy = Wawes[numWave].BossMob;
            /*if ((numWave + 1) < Wawes.Length)
            {
                bossEnemy = Wawes[numWave + 1].MainMob;
            }
            else
            {
                bossEnemy = Wawes[numWave].MainMob;
            }*/
            countEnemyInWawe = countBossEnemyInWawe + countMainMobsInWawe;

            _isFilled = true;
        }
        else if (_isSurvival)
        {
            Debug.Log("Начался режим выживания");
            _mulHealth = numWave * addHealth;
            var t = Wawes[UnityEngine.Random.Range(0, Wawes.Length)];
            countBossEnemyInWawe = t.CountBoss + numWave * addCount;
            countMainMobsInWawe = t.CountMainMobs + numWave * addCount;
            enemy = t.MainMob;
            var r = Wawes[UnityEngine.Random.Range(0, Wawes.Length)];
            bossEnemy = r.MainMob;
            countEnemyInWawe = countBossEnemyInWawe + countMainMobsInWawe;

            _isFilled = true;
        }
        else
        {

            _isFilled = true;
            Debug.Log("Волны закончились");
        }
    }

    void SpawnControll()
    {
        Time.timeScale = BonusInfo.slowTime;
        time.AddTime(Time.deltaTime);

        //В конце волны обновляем счетчики и запускаем перекур
        if (countEnemy == countEnemyInWawe && curentEnemy <= 0 && !IsRelax)
        {
            countEnemy = 0;
            curentEnemy = 0;
            // countEnemyInWawe += countEnemyInWawe / 2;
            numWave++;
            curentTimeRelax = timeRelax;
            IsRelax = true;
            _isFilled = false;
        }
        //Создаем мобов
        if (!IsRelax && curentTimeRelax <= 0 && maxCountEnemyInScene > curentEnemy)
        {
            SpawnEnemy();

            //  InstantiateEnemy();
        }
        //курим
        else if (IsRelax)
        {
            curentTimeRelax -= Time.deltaTime;
            if (curentTimeRelax <= 0) IsRelax = false;

        }
    }

    void SpawnEnemy()
    {
        if (!Frieze)
        {
            if (countEnemy < countEnemyInWawe && !IsInvoking("Spawn"))
            {

                Invoke("Spawn", delaySpawn);
            }
        }

    }



    private void Spawn()
    {
        GameObject spavn = pointSpawn[UnityEngine.Random.Range(0, pointSpawn.Length)];
        PointSpawn _spavnParam = spavn.GetComponent<PointSpawn>();
        if (_spavnParam.CanSpawn)
        {


            if (countMainMobsInWawe != 0)
            {

                countMainMobsInWawe--;
                countEnemy++;
                curentEnemy++;

                GameObject g = Instantiate(enemy, spavn.transform.position, Quaternion.identity) as GameObject;
                g.transform.parent = spavn.transform;
                //передаем тип игры мобам
                g.GetComponent<EnemyN>().isSurvival = _isSurvival;

            }
            if (countBossEnemyInWawe != 0 && countMainMobsInWawe == 0)
            {
                countBossEnemyInWawe--;
                countEnemy++;
                curentEnemy++;
                // GameObject spavn = pointSpawn[UnityEngine.Random.Range(0, pointSpawn.Length)];
                GameObject g = Instantiate(bossEnemy, spavn.transform.position, Quaternion.identity) as GameObject;
                g.transform.parent = spavn.transform;
                g.GetComponent<EnemyN>().isSurvival = _isSurvival;

            }
        }

        /* var t = Random.Range(0, 2);
         if ((t >= 1 && countMainMobsInWawe != 0) || countMainMobsInWawe != 0 && countBossEnemyInWawe == 0)
         {
             countMainMobsInWawe--;
             countEnemy++;
             curentEnemy++;
             GameObject spavn = pointSpawn[UnityEngine.Random.Range(0, pointSpawn.Length)];
             GameObject g = Instantiate(enemy, spavn.transform.position, Quaternion.identity) as GameObject;
             g.transform.parent = spavn.transform;
             //передаем тип игры мобам
             g.GetComponent<EnemyN>().isSurvival = _isSurvival;
             //первые мобы у нас будут агресивными
             //if (_isSurvival && _currentAgrMobs < CountAgrMobs)
             //{
             //    _currentAgrMobs++;
             //    g.GetComponent<EnemyN>().IsAgr = true;

             //    g.GetComponent<EnemyN>()._wasAdd = true;
             //}
             //else
             //{
             //    g.GetComponent<EnemyN>().IsAgr = false;

             //    g.GetComponent<EnemyN>()._wasAdd = false;
             //    CreationDone = true;

             //}
         }
         if ((t < 1 && countBossEnemyInWawe != 0) || countBossEnemyInWawe != 0 && countMainMobsInWawe == 0)
         {
             countBossEnemyInWawe--;
             countEnemy++;
             curentEnemy++;
             GameObject spavn = pointSpawn[UnityEngine.Random.Range(0, pointSpawn.Length)];
             GameObject g = Instantiate(bossEnemy, spavn.transform.position, Quaternion.identity) as GameObject;
             g.transform.parent = spavn.transform;
             g.GetComponent<EnemyN>().isSurvival = _isSurvival;
             //if (_isSurvival && _currentAgrMobs < CountAgrMobs)
             //{
             //    _currentAgrMobs++;
             //    g.GetComponent<EnemyN>().IsAgr = true;
             //    g.GetComponent<EnemyN>()._wasAdd = true;
             //}
             //else
             //{
             //    g.GetComponent<EnemyN>().IsAgr = false;

             //    g.GetComponent<EnemyN>()._wasAdd = false;
             //    CreationDone = true;
             //}
         }*/
    }

    void SetAgresive()
    {
        Debug.Log("Делаем агресивным");
        GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyN>().IsAgr = true;
        _currentAgrMobs++;
    }

    void GetRandomSpawn()
    {


    }





    //bonuses

    void FillBonusList()
    {
        var t = GameObject.FindGameObjectsWithTag("Bonus");
        var y = GameObject.FindGameObjectsWithTag("WeaponBonus");
      //GameObject[]  y = new GameObject[0];
        int count = t.Count() + y.Count();
        _BonusList = new GameObject[count];
        int i = 0;

        if (t != null)
        {

            foreach (GameObject o in t)
            {
                _BonusList[i] = o;
                i++;
            }
        }
        if (y != null)
        {
            foreach (GameObject r in y)
            {
                _BonusList[i] = r;
                i++;
            }

        }
    }



    public void AddXp(int _xp)
    {
        xp += _xp;
    }

    //используется чтоб показать в конце время игры
    public float PlayedTime()
    {
        return Time.time - _startTime;
    }


    //используется для суммирования количества выстрелов
    public void AddShoot()
    {
        ShootCount++;
    }

    //используется для сумирования количества попаданий
    public void AddHit()
    {
        HitCount++;
    }

    //Количество убитых мобов
    public void AddKilledMobs()
    {
        KilledMobs++;
    }

    public float GetAccuracy()
    {


        if (HitCount == 0 || ShootCount == 0) return 0;
        var acuur = HitCount / ShootCount * 100;
        return acuur;
    }
}

[System.Serializable]
public class TimeManeger
{
    public int milliseconds;
    public int seconds;
    public int minutes;
    public int hours;
    public DateTime dataTime = new DateTime();
    public void AddTime(float time)
    {

        dataTime = dataTime.AddSeconds(time);
        milliseconds = dataTime.Millisecond;
        seconds = dataTime.Second;
        minutes = dataTime.Minute;
        hours = dataTime.Hour;
    }


}

public class BonusList
{
    public int ID { get; set; }
    public GameObject Bonus { get; set; }
}
[System.Serializable]
public class Wawe
{
    [Tooltip("Префаб основных мобов которые будут в волне")]
    public GameObject MainMob;
    [Tooltip("Префаб боссов которые будут в волне")]
    public GameObject BossMob;
    [Tooltip("Количество мобов в волне")]
    public int CountMainMobs;
    [Tooltip("Сколько должно быть боссов, боссы берутся из следующей волны")]
    public int CountBoss;
}
/// <summary>
/// Класс для передачи множителей
/// </summary>
public class Mul
{
    public float Health { get; set; }
    public int Count { get; set; }
}