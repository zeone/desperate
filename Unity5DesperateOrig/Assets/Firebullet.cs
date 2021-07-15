using UnityEngine;
using System.Collections;

public class Firebullet : MonoBehaviour
{
    public int Force = 1000;
    public float LiveTime = 3;
    public Transform Parent;
    [Header("Параметры для дробовика")]
    public bool IsShotgunBullet;

    public Vector3 PositionOnParent;
    public Quaternion RotationOnParent;
    private Rigidbody _body;
    private float _timer;
    // Use this for initialization
    void OnEnable()
    {
        _timer = LiveTime;
        _body = this.gameObject.GetComponent<Rigidbody>();
        _body.isKinematic = false;
        if (!IsShotgunBullet) transform.parent = null;
        _body.AddForce(transform.forward * Force);
    }
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0) Disable();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            col.SendMessage("GetDamage", Weapon.entity.damage);

        }

    }

    void Disable()
    {
        _body.isKinematic = true;
        if (!IsShotgunBullet)
        {
            transform.parent = Parent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            // transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localPosition = PositionOnParent;
            transform.localRotation = RotationOnParent;
        }
        gameObject.SetActive(false);

    }
}
