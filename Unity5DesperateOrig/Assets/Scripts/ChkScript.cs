using System;
using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public class ChkScript : MonoBehaviour
{


    [Header("Выведем фпс")]
    public bool ShowFps;

    public bool OnlyAvareFps;
    public float updateInterval = 0.5F;
    [Tooltip("Количество кадров которые будут сумироватся для среднего ФПС")]
    public int ControlCount = 50;
    private float lastInterval;
    private int frames = 0;
    private float fps;
    private float minFps = 0;
    private float maxFps = 20;
    private float midFps = 0;
    private float _midFPSCalc = 0;
    private int i = 1;
    private bool canAtach = false;

    [Header("Контроль освещения")]
    public GameObject FlashLight;

    [Header("Переключение работы тригерров")]
    public EasyJoystick LeftTrigger;
    public EasyJoystick RightTrigger;
    private int b;

    public bool WaweInfo;



    void Awake()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer) Application.targetFrameRate = 30;
    }
    // Use this for initialization
    void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        StartCoroutine("Delay");
    }


    IEnumerator Delay()
    {

        yield return new WaitForSeconds(3.0f);
        canAtach = true;
        minFps = fps;
    }
    // Update is called once per frame
    private void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            fps = frames / (timeNow - lastInterval);
            frames = 0;
            lastInterval = timeNow;
            if (canAtach) CalcMiddle(fps);
        }
        if (fps > maxFps) maxFps = fps;
        if (canAtach && fps < minFps) minFps = fps;

    }

    void OnGUI()
    {
        if (ShowFps)
        {
            GUILayout.Label("Current FPS: " + fps.ToString("f2") + "   ХП: " + Player.entity.heals + " EX: " +
                            GameMeneger.entity.xp);
            GUILayout.Label("Min FPS: " + minFps.ToString("f2") + " Score: " + GameMeneger.entity.score + " Ammo: " +
                            Weapon.entity.curentAmmo);
            GUILayout.Label("Max FPS: " + maxFps.ToString("f2") + " Shield:" + GameMeneger.entity.Shield);
            if (midFps < 1)
            {

                GUILayout.Label("Average FPS: calculating wait " + (ControlCount - i));
            }
            else
            {

                GUILayout.Label("Average FPS: " + midFps.ToString("f2"));
            }
        }
        if (OnlyAvareFps)
        {
           // GUILayout.Label( midFps.ToString("f2"));
            GUI.Box(new Rect(0, 100, 50, 20), midFps.ToString("f2"));
        }
        if (WaweInfo)
        {
            GUILayout.Label("Номер волны: " + (GameMeneger.entity.numWave + 1));
            GUILayout.Label("Мобов в волне: " + GameMeneger.entity.countEnemyInWawe);
            GUILayout.Label("Мобов на сцене: " + GameMeneger.entity.curentEnemy);
        } /*
        if (GUI.Button(new Rect(10, 170, 130, 50), "Переключение\n целеуказателя"))
        {
            Weapon.entity.GetLaser();

            Debug.Log(b);
            //горит все
            if (b == 0)
            {
                ++b;
                Debug.Log("Горит все");
                FlashLight.SetActive(true);
                Weapon.entity.Laser.gameObject.SetActive(true);
                return;
            }
            //горит фонарь
            if (b == 1)
            {
                ++b;
                Debug.Log("Фонарь");
                FlashLight.SetActive(true);
                Weapon.entity.Laser.gameObject.SetActive(false);
                return;
            }
            //горит лазер
            if (b == 2)
            {
                Debug.Log("Лазер");
                b = 0;
                FlashLight.SetActive(false);
                Weapon.entity.Laser.gameObject.SetActive(true);
            }

            if (b > 2) b = 0;
        }
        /* if (GUI.Button(new Rect(10, 210, 130, 50), "Тип джойстиков"))
         {
          //   LeftTrigger.DynamicJoystick = !LeftTrigger.DynamicJoystick;
             LeftTrigger.DynamicJoystick =!LeftTrigger.DynamicJoystick;
             RightTrigger.DynamicJoystick = !RightTrigger.DynamicJoystick;
             Debug.Log(LeftTrigger.DynamicJoystick +" " + RightTrigger.DynamicJoystick);

         }*/

    }

    void CalcMiddle(float _fps)
    {
        ++i;

        if (_midFPSCalc < 1) _midFPSCalc = _fps;
        _midFPSCalc += _fps;


        if (i == ControlCount)
        {
            midFps = _midFPSCalc / i;

            i = 1;
            _midFPSCalc = 0;
        }
    }

}

