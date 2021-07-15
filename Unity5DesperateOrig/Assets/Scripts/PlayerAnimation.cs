using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    public float maxIdleSpeed = 0.5f;
    public float minWalkSpeed = 2.0f;
    public AnimationClip idle;
    public AnimationClip turn;
    public AnimationClip death;
    public AnimationClip altDeath;
    public AnimationClip shootAdditive;
    public DirectionAnimation[] moveAnimations;
    private Animation animation;
    public Transform rootBone;
    public Transform upperBodyBone;
    public Transform rigTransform;
    private Vector3 lastPosition = Vector3.zero;
    public  Vector3 velocity = Vector3.zero;
    public  Vector3 localVelocity = Vector3.zero;
    public float speed = 0;
    private float angle = 0;
    private float lowerBodyDeltaAngle = 0;
    private float idleWeight = 0;
    private Vector3 lowerBodyForwardTarget = Vector3.forward;
    private Vector3 lowerBodyForward = Vector3.forward;
    private DirectionAnimation bestAnimation = null;
    private float lastFootstepTime = 0;
    private float lastAnimTime = 0;
    public  float constantSmooth = 0;
    private static PlayerAnimation _entity;
    public static PlayerAnimation entity
    {
        get { return _entity; }
        set { _entity = value; }
    }
    
    //функция предачи компонентов в нуторь класса
    public void Awake()
    {
        entity = this;


        rigTransform = gameObject.GetComponent<Transform>(); 
        lastPosition = rigTransform.position;
        animation = gameObject.GetComponent<Animation>();
        foreach (DirectionAnimation moveAnimation in moveAnimations)
        {
            moveAnimation.Init();
            animation[moveAnimation.clip.name].layer = 1;
            animation[moveAnimation.clip.name].enabled = true;
        }
        animation.SyncLayer(1);
        animation[idle.name].layer = 2;
        animation[turn.name].layer = 3;
        animation[idle.name].enabled = true;
        animation[death.name].layer = 4;
        animation[death.name].weight = 1;
        animation[death.name].enabled = false;
        animation[death.name].wrapMode = WrapMode.Once;
        

        ////animationComponent[shootAdditive.name].layer = 4;
        ////animationComponent[shootAdditive.name].weight = 1;
        ////animationComponent[shootAdditive.name].speed = 0.1f;
        ////animationComponent[shootAdditive.name].blendMode = AnimationBlendMode.Additive;
        

    }
    public void FixedUpdate()
    {
        
        
        velocity = (rigTransform.position - lastPosition) / Time.deltaTime;
        localVelocity = rigTransform.InverseTransformDirection(velocity);
        localVelocity.y = 0;
        speed = localVelocity.magnitude;
        angle = Angles.HorizontalAngle(localVelocity);
        

        lastPosition = rigTransform.position;
    }
    //анимация
    public void Update()
    {
        if (!Player.entity.isDeath)
        {
            idleWeight = Mathf.Lerp(idleWeight, Mathf.InverseLerp(minWalkSpeed, maxIdleSpeed, speed), Time.deltaTime * 10);
            animation[idle.name].weight = idleWeight;

            if (speed > 0)
            {
                float smallestDiff = Mathf.Infinity;
                foreach (DirectionAnimation moveAnimation in moveAnimations)
                {
                    float angleDiff = Mathf.Abs(Mathf.DeltaAngle(angle, moveAnimation.angle));
                    float speedDiff = Mathf.Abs(speed - moveAnimation.speed);
                    animation[moveAnimation.clip.name].speed = speed * constantSmooth;
                    float diff = angleDiff + speedDiff;
                    if (moveAnimation == bestAnimation)
                        diff *= 0.9f;

                    if (diff < smallestDiff)
                    {
                        bestAnimation = moveAnimation;
                        smallestDiff = diff;
                    }
                }
                Debug.Log(bestAnimation.clip.name);
                animation.CrossFade(bestAnimation.clip.name);
            }
            else
            {
                bestAnimation = null;
            }

            if (lowerBodyForward != lowerBodyForwardTarget && idleWeight >= 0.9f)
                animation.CrossFade(turn.name, 0.05f);

            if (bestAnimation != null && idleWeight < 0.9f)
            {
                float newAnimTime = Mathf.Repeat(animation[bestAnimation.clip.name].normalizedTime * 2 + 0.1f, 1);
                if (newAnimTime < lastAnimTime)
                {
                    if (Time.time > lastFootstepTime + 0.1)
                    {
                        
                        lastFootstepTime = Time.time;
                    }
                }
                lastAnimTime = newAnimTime;
            }
        }
        else if (!Player.entity.isDeath)
        {
            animation[idle.name].enabled = false;
            animation[turn.name].enabled = false;
            foreach (DirectionAnimation moveAnimation in moveAnimations)
            {
                moveAnimation.Init();
                //animationComponent[moveAnimation.clip.name].layer = 1;
                animation[moveAnimation.clip.name].enabled = false;
            }
            animation[death.name].enabled = true;
           // animationComponent.CrossFade(death.name);
            Player.entity.isDeath = true;
        }
        
    }
    //после вращение костей
    public void LateUpdate()
    {
        if (!Player.entity.isDeath)
        {
            float idle = Mathf.InverseLerp(minWalkSpeed, maxIdleSpeed, speed);
            if (idle < 1)
            {

                Vector3 animatedLocalVelocity = Vector3.zero;
                foreach (DirectionAnimation moveAnimation in moveAnimations)
                {

                    if (animation[moveAnimation.clip.name].weight == 0)
                    {

                    
                        continue;
                    }
                    if (Vector3.Dot(moveAnimation.velocity, localVelocity) <= 0)
                        continue;

                    animatedLocalVelocity += moveAnimation.velocity * animation[moveAnimation.clip.name].weight;
                }


                float lowerBodyDeltaAngleTarget = Mathf.DeltaAngle(
                    Angles.HorizontalAngle(rigTransform.rotation * animatedLocalVelocity),
                    Angles.HorizontalAngle(velocity)
                    );


                lowerBodyDeltaAngle = Mathf.LerpAngle(lowerBodyDeltaAngle, lowerBodyDeltaAngleTarget, Time.deltaTime * 10);


                lowerBodyForwardTarget = rigTransform.forward;
                lowerBodyForward = Quaternion.Euler(0, lowerBodyDeltaAngle, 0) * lowerBodyForwardTarget;
            }
            else
            {

                lowerBodyForward = Vector3.RotateTowards(lowerBodyForward, lowerBodyForwardTarget, Time.deltaTime * 520 * Mathf.Deg2Rad, 1);

                lowerBodyDeltaAngle = Mathf.DeltaAngle(
                    Angles.HorizontalAngle(rigTransform.forward),
                    Angles.HorizontalAngle(lowerBodyForward)
                    );

                if (Mathf.Abs(lowerBodyDeltaAngle) > 80)
                    lowerBodyForwardTarget = rigTransform.forward;
            }

            Quaternion lowerBodyDeltaRotation = Quaternion.Euler(0, lowerBodyDeltaAngle, 0);

            rootBone.rotation = lowerBodyDeltaRotation * rootBone.rotation;

            upperBodyBone.rotation = Quaternion.Inverse(lowerBodyDeltaRotation) * upperBodyBone.rotation;


            // Quaternion rot =
            // upperBodyBone.rotation = Quaternion.LookRotation(aim);
        }
    }
    
}
[System.Serializable]
public class DirectionAnimation
{
    //клип анимации
    //скорость при котрой анимация проигравыеться
    //вес клипа на анимацию
    //скорость проигрывания анимации
    //угол проигрывания анимации
    public AnimationClip clip;
    public Vector3 velocity;
    public float weight;
    public bool currentBest = false;
    public float speed;
    public float angle;

    public void Init()
    {
        velocity.y = 0;
        speed = velocity.magnitude;
        angle = Angles.HorizontalAngle(velocity);
    }

}