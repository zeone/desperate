using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour
{
    [Header("Info")]
    public NavMeshAgent agent;
    public Transform player;
    public Vector3 target;
    public float timeUpdate;
    [Header("Confing speed")]
    public float speedWalk;
    public float speedAngryMin;
    public float speedAngryMax;
    public bool isMove;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = gameObject.GetComponent<NavMeshAgent>();
        InvokeRepeating("LimitedUpdate", timeUpdate, timeUpdate);
    }

    // Update is called once per frame
    void LimitedUpdate()
    {
        if(agent.enabled)
        {
            SetTarget();
        }

       
        
    }
    void SetTarget()
    {
        if (player && !Player.entity.isDeath && isMove)
        {
            target = player.position;
            agent.SetDestination(target);
            agent.Resume();
        }
        else
        {
            agent.Stop();
        }
    }
}
