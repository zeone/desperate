using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;

public class Ricochet : MonoBehaviour
{
    /*[Tooltip("Пушка к которой будет возвражатся объект")]
    public Transform Parrent;
    */
    public int Damage = 100;
    [Tooltip("Система частиц внутри пули")]
    public GameObject Particle;
    [Tooltip("Скорость полета")]
    public float Speed = 15;
    //[Tooltip("")]
    //public float RotationSpeed = 800;
    [Tooltip("Количество рикошетов до уничтожения")]
    public int MaxTouch = 3;
    [Tooltip("Время до уничтожения")]
    public float LifeTime = 6;

    public float DistanceForChangeTarget = 1;
    public GameObject[] TargetList;

    public float maxRicochetDistance = 15;
    public Vector3 ParentPosition;
    public Quaternion ParentRotation;


    float distance = 0;
    float currentDistance;
    GameObject newTarget;

    public GameObject Target;
    private AudioSource audio;
    public AudioClip FlyNoise;
    private float _timmer;
    private List<GameObject> _alreadyHit = new List<GameObject>();
    private int _touchCount;

    void OnEnable()
    {
        _touchCount = MaxTouch;
        TargetList = new GameObject[_touchCount];

        FillTargetList();
        if (Particle) Particle.SetActive(true);
    }

    void Awake()
    {
        DistanceForChangeTarget *= DistanceForChangeTarget;

    }
    void Start()
    {


        // newTarget = new GameObject();
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!Target && _touchCount > 0) Target = TargetList[_touchCount - 1];


        _timmer += Time.deltaTime;
        if (_timmer < LifeTime && _touchCount > 0)
        {
            if (CheckTargetDist()) return;
            // if (transform.parent != null) transform.parent = null;
            if (!audio.isPlaying)
            {
                audio.clip = FlyNoise;
                audio.Play();
            }
            //Bullet.Rotate(Vector3.up);

            GoToTarget();

            /* if (Target)
             {
                 GoToTarget();
             }
             else
             {
                 SimpleRicochet();
             }*/
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
        else
        {
            Destroy();
        }
    }



    private void OnTriggerEnter(Collider col)
    {
        //     if (Target && col.gameObject.GetInstanceID() == Target.GetInstanceID()) Target = null;
        if (col.tag == "Enemy")
        {
            // if (Check(col.GetInstanceID()))
            //  {
            //  _alreadyHit.Add(col.gameObject);
            --_touchCount;
            Target = null;
            col.SendMessage("GetDamage", Damage);
            /*  }
              else
              {
                  --_touchCount;
                  Target = Target = TargetList[_touchCount - 1];
              }*/
        }
        //  Debug.Log("Ricochet " + col.name);
    }

    void SimpleRicochet()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Time.deltaTime * Speed + .1f))
        {
            Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
            float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rot, 0);

        }
    }

    void FindTarget(int ID)
    {
        if (!Target)
        {
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject o in enemys)
            {
                if (ID != o.GetInstanceID())
                {
                    currentDistance = (o.transform.position - transform.position).sqrMagnitude;
                    if (distance > currentDistance || distance == 0)
                    {
                        distance = currentDistance;
                        newTarget = o;
                    }
                }
                if (distance > maxRicochetDistance)
                {
                    Target = null;
                }
                else
                {


                    Target = newTarget;
                }
            }
        }
    }

    void FillTargetList()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int i = 0;
        if (enemies.Length < _touchCount) _touchCount = enemies.Length;
        foreach (GameObject enemy in enemies)
        {
            if (TargetList.Length > 0)
            {
                foreach (GameObject o in TargetList)
                {
                    if (o == null)
                    {
                        TargetList[i] = enemy;
                        break;
                    }
                    if (o.GetInstanceID() != enemy.GetInstanceID())
                    {
                        TargetList[i] = enemy;
                        break;
                    }

                }
            }
            else
            {
                TargetList[i] = enemy;
            }
            ++i;
            if (i == _touchCount) break;
        }
    }

    bool CheckTargetDist()
    {
        if ((transform.position - Target.transform.position).sqrMagnitude < DistanceForChangeTarget)
        {
            --_touchCount;
            Target.SendMessage("GetDamage", Damage);
            Target = null;
            return true;
        }
        return false;
    }

    void GoToTarget()
    {
        if (Target) transform.LookAt(Target.transform);
    }

    void Destroy()
    {
        _touchCount = MaxTouch;
        TargetList = null;
        audio.Stop();
        _timmer = 0;

        if (Particle) Particle.SetActive(false);
        // transform.parent = Parrent;
        // transform.localPosition = ParentPosition;
        //  transform.localRotation = ParentRotation;
        gameObject.SetActive(false);
    }

    bool Check(int Id)
    {
        foreach (GameObject o in _alreadyHit)
        {
            if (o.GetInstanceID() == Id) return false;
        }
        return true;
    }
}
