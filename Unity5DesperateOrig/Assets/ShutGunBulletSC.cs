
using UnityEngine;
using System.Collections;

public class ShutGunBulletSC : MonoBehaviour
{
    public GameObject Parent;
    public float Timer = 3;
    private float _timer;


    void OnEnable()
    {
        _timer = Timer;
        transform.parent = null;
        foreach (Transform o in transform)
        {
            o.gameObject.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0) Disable();
    }

    void Disable()
    {
        transform.parent = Parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
}
