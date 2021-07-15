using System.Linq;
using System.Security;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
//клас враг раширен от павна
using UnityEngineInternal;

public class Enemy : MonoBehaviour
{
    [Header("Main confing")]
    public float heals;
    public int score;
    public int exp;
    public float distantionAttack;
    public GameObject Target;

    [Header("Случайный бонус")]
    [Tooltip("Вероятность 1 из ...")]
    public int Chance = 10;
    [Header("Sound confing")]
    public EnemySound sound;
    public AudioSource audioSource;
    [Header("Component confing")]
    public Collider collider;
    public Navigation navigator;
    public Transform actorTransform;
    public Animation actorAnimation;
    public EnemyVFX enemyVFX;
    protected TriggerHand _hand;
    public Weapon playerWepon;
    [Header("Animation confing")]
    public AnimationClip death;
    public AnimationClip impulceDeath;
    public bool isDeath;
    private static bool _isFrize;
    public static bool isFrize
    {
        get { return _isFrize; }
        set { _isFrize = value; }
    }




    public void Create()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
        if (gameObject.GetComponent<Navigation>() != null)
        {
            navigator = gameObject.GetComponent<Navigation>();
        }
        else
        {
            Debug.LogWarning("На объект " + gameObject.name + " надо повесить скрипт Navigation");
        }
        if (gameObject.GetComponent<CapsuleCollider>() != null)
        {
            collider = gameObject.GetComponent<CapsuleCollider>();
        }
        else
        {
            Debug.LogWarning("На объект " + gameObject.name + " надо повесить скрипт CapsuleCollider");
        }
        actorTransform = gameObject.GetComponent<Transform>();
        if (gameObject.GetComponent<Animation>())
        {
            actorAnimation = GetComponent<Animation>();
        }
        else
        {
            Debug.LogWarning("На объект " + gameObject.name + " надо повесить скрипт Animation");
        }
        if (actorTransform.GetComponentInChildren<EnemyVFX>())
        {
            enemyVFX = actorTransform.GetComponentInChildren<EnemyVFX>();
        }
        else
        {
            Debug.LogWarning("На объект " + gameObject.name + " надо повесить скрипт EnemyVFX");
        }
        if (actorTransform.GetComponentInChildren<TriggerHand>())
        {
            _hand = actorTransform.GetComponentInChildren<TriggerHand>();
        }
        else
        {
            Debug.Log("На мобе нету скрипта для урона");
        }

        // GameMeneger.entity.countEnemy++;
    }
    public void DamageOffParticle(string filter)
    {
        int r = Random.Range(0, VFXManeger.Blood.bloodEmiters.Length);
        VFXManeger.Blood.transformEmiter.position = actorTransform.position + Vector3.up;
        VFXManeger.Blood.bloodEmiters[r].Play();

        //Blood.transformParticles.position = actorTransform.position+Vector3.up;
        //Blood.transformParticles.rotation = actorTransform.rotation;
        //отниаем урон от жизни павна
        if (filter == "Weapon") heals -= Weapon.entity.damage;
        if (filter == "Turet") heals -= 10;
        if (filter == "Plazma") heals -= 1000;

        //проверка на наличие жизни
        if (heals <= 0)
        {
            DeathEnemy();
        }

    }
    public void DeathEnemy()
    {


        if (Weapon.entity.isImpulse)
        {
            actorAnimation.CrossFade(impulceDeath.name);
        }
        else
        {
            actorAnimation.CrossFade(death.name);
        }
        TakeABonus();
        collider.enabled = false;
        navigator.agent.enabled = false;
        //animationActor.animation.CrossFade(animationActor.death.name);
        audioSource.clip = sound.death[Random.Range(0, sound.death.Length)];
        audioSource.Play();
        //GameMeneger.entity.countEnemy--;
        GameMeneger.entity.curentEnemy--;
        GameMeneger.entity.score += score;
        GameMeneger.entity.xp += exp * GameMeneger.entity.mulXp;
        isDeath = true;
        enemyVFX.isDeath = true;
        Invoke("Death", 5);
    }
    void Death()
    {
        // Debug.Log("Death");
        Destroy(gameObject);
    }
    //Считаем давать бонус или нет
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

}
[System.Serializable]
public class EnemySound
{
    public AudioClip[] death;
    public AudioClip[] noise;
    public AudioClip[] attack;
}


