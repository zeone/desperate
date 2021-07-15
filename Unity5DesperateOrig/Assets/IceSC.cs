
using UnityEngine;
using System.Collections;

public class IceSC : MonoBehaviour
{

    public AnimationClip Create;
    public AnimationClip DestroyIce;
    //нужны чтоб анимация не проигпывалась несколько раз
    private bool isPlayngDestroy;
    private bool isPlayngCreate = false;
    public bool Unfrize;

    public static IceSC entyti { get; set; }
    private Animation _animation;

    void Start()
    {
        // isPlayngDestroy = false;
        entyti = this;
    }
    void OnEnable()
    {
        isPlayngDestroy = false;
        _animation = gameObject.GetComponent<Animation>();
        if (!isPlayngCreate)
        {
            isPlayngCreate = true;
            _animation.CrossFade(Create.name);
        }
    }

    void Update()
    {
        if (!GameMeneger.entity.Frieze)
        {
            if (!isPlayngDestroy)
            {
                isPlayngDestroy = true;
                _animation.CrossFade(DestroyIce.name);
            }

            if (!IsInvoking("DisableIce")) Invoke("DisableIce", DestroyIce.length);

        }
    }

    void DisableIce()
    {

        //Unfrize = false;
        gameObject.SetActive(false);
    }

    public void DisableOnDeath()
    {
        if (!isPlayngDestroy)
        {
            isPlayngDestroy = true;
            _animation.CrossFade(DestroyIce.name);
        }
        if (!IsInvoking("DisableIce")) Invoke("DisableIce", DestroyIce.length);
    }

    void OnDisable()
    {
        isPlayngCreate = false;
        isPlayngDestroy = false;
    }



}
