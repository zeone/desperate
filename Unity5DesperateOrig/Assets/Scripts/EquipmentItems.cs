using RootMotion.FinalIK.Demos;
using UnityEngine;
using System.Collections;

public class EquipmentItems : MonoBehaviour
{
    [Tooltip("иконка")]
    public Texture2D icon;
    [Tooltip("Затемненная иконка")]
    public Texture2D glow;
    [Tooltip("Время до ищезновения")]
    public float TimeToGone = 5;
    //контрольная переменая для таймера
    private float _timeLeft;
    Material material;
    MeshRenderer render;
    public float emision;
    [Tooltip("Размер иконки")]
    public float size = 1.5f;
    public float distToPlayer;
    public Transform transform;
    [Header("Оружие которое применяем")]
    [Tooltip("Скрипт который находится на руке персонажа")]
    public WeaponCtrl WepCtrl;
    [Tooltip("Тег оружия которое должно быть активным")]
    public string TagName;

    [Header("Тип бафа")]
    public bool Fireblast;

    public bool Turret;

    public bool ShockChain;
    public bool Nuke;

    public GameObject weapon;
    void Start()
    {
        _timeLeft = TimeToGone;
        render = GetComponent<MeshRenderer>();
        transform = gameObject.GetComponent<Transform>();
        material = GetComponent<Renderer>().material;
        material.SetTexture("_MainTex", icon);
        if (glow != null) material.SetTexture("_OtherTex", glow);
        material.SetFloat("_Size", size);
        WepCtrl = GameObject.FindGameObjectWithTag("WeaponSlot").GetComponent<WeaponCtrl>();

    }
    public void Update()
    {
        Deactivate();
        // distToPlayer = Vector3.Distance(transform.position, Player.entity.transform.position);
        distToPlayer = (Player.entity.transform.position - gameObject.transform.position).sqrMagnitude;
        if (distToPlayer < 6)
        {
            render.enabled = false;
            //Inventory.entity.AddToInventory(gameObject);
            if (gameObject.tag == "Bonus")
            {
                if (Fireblast)
                {
                    if (Plazma.EnableFireblast()) gameObject.SetActive(false);
                }
                else if (Turret)
                {
                    Vector3 pos = transform.position;
                    pos.y = 5.849272f;
                    weapon.transform.position = pos;
                    weapon.SetActive(true);
                    gameObject.SetActive(false);
                }
                else if (ShockChain)
                {
                    Vector3 pos = transform.position;
                    pos.y = 6.99f;
                    weapon.transform.position = pos;
                    weapon.SetActive(true);
                    gameObject.SetActive(false);
                }
                else if (Nuke)
                {
                    Vector3 pos = transform.position;
                    pos.y = 7.68f;
                    weapon.transform.position = pos;
                    weapon.SetActive(true);
                    gameObject.SetActive(false);
                }
                else
                {
                    // gameObject.SendMessage("Add");
                    if (weapon) Inventory.entity.AddToInventory(weapon);
                    gameObject.SetActive(false);
                }
            }
            else if (gameObject.tag == "WeaponBonus" && TagName != null)
            {
                WepCtrl.ActivateWeapon(TagName);

                /*  transform.localPosition = Vector3.zero;
                 // Debug.Log(weapon.name + " отправлен");
                  //transform.GetChild(0).gameObject.active = true;
                  Inventory.entity.AddToInventory(weapon);
                  gameObject.SetActive(false);
              */
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Нужно задать имя тега для оружия");
            }

        }
        emision = Mathf.PingPong(Time.time * 5, 6);
        material.SetFloat("_Emision", emision);
    }

    void Deactivate()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft < 0) gameObject.SetActive(false);
    }

    public void ActivateBuff(Vector3 position)
    {
        transform.position = position;
        _timeLeft = TimeToGone;
        gameObject.SetActive(true);
    }




}
