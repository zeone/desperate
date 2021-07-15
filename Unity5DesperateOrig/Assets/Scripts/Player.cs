using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
public class Player : MonoBehaviour
{
    [Header("Main confing")]
    public float heals;
    public float maxHeals;
    public bool isGodmode;
    [Header("Sound confing")]
    public AudioSource stepSource;
    public AudioSource otherSource;
    public AudioSource noiseSource;
    public AudioClip death;
    public AudioClip lowHeals;
    public AudioClip[] steps;
    public AudioClip[] hit;
    public AudioClip noise;
    [Header("Components")]
    public NavMeshObstacle obstacle;
    public Transform transform;
    public Rigidbody rigidBody;
    public Animator _anim;
    public bool isDeath;
    private static Player _palyer;
    private GameObject _UI;
    public static Player entity
    {
        get { return _palyer; }
        set { _palyer = value; }
    }
    void Awake()
    {
        _UI = GameObject.FindGameObjectWithTag("UI");
        heals = maxHeals;
        transform = gameObject.GetComponent<Transform>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
        obstacle = gameObject.GetComponent<NavMeshObstacle>();
        entity = this;

    }
    void Start()
    {

    }

    void Update()
    {
        isGodmode = GameMeneger.entity.Shield;
        PlaySound();
    }

    public void PlaySound()
    {
        if (heals < 25 && !noiseSource.isPlaying)
        {
            noiseSource.clip = lowHeals;
            noiseSource.Play();
        }
    }
    public void Damage(int damage)
    {
        int r = Random.Range(0, VFXManeger.Blood.bloodEmiters.Length);
        VFXManeger.Blood.transformEmiter.position = transform.position + Vector3.up;
        VFXManeger.Blood.bloodEmiters[r].Play();
        otherSource.clip = hit[Random.Range(0, hit.Length)];
        otherSource.Play();
        if (!isGodmode) heals -= damage;
        if (heals <= 0) DeathHuman();
    }
    public void DeathHuman()
    {
        if (!isDeath)
        {
            isDeath = true;
            Inventory.entity.DestroyWeapon();
            _anim.SetBool("IsDead", isDeath);
        }
        noiseSource.clip = death;
        noiseSource.Play();
        Debug.Log("Human death :(");
        if (obstacle != null) obstacle.enabled = false;
        rigidBody.GetComponent<Collider>().enabled = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        noiseSource.Stop();
        if (!IsInvoking("Deadmenu")) Invoke("DeadMenu", 1);
    }


    void DeadMenu()
    {

        _UI.SendMessage("DeadMenu");
    }
    public void PlaySteps()
    {
        int lengSteps = steps.Length;
        int r = Random.Range(0, lengSteps);
        stepSource.pitch = Random.Range(0.98f, 1.02f);
        stepSource.PlayOneShot(steps[r], Random.Range(0.8f, 1.2f));
    }


    //Bonuses

    void AddHp(int hp)
    {
        heals += hp;
    }



}


