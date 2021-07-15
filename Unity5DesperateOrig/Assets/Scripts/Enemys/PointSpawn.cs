using UnityEngine;
using System.Collections;

public class PointSpawn : MonoBehaviour
{

    public float Range = 10;
    public Transform Player;
    public bool CanSpawn;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        var distToPlayer = (transform.position - Player.transform.position).sqrMagnitude;
        if (distToPlayer < Range)
        {
            CanSpawn = false;
            Debug.DrawLine(transform.position, Player.position, Color.red);
        }
        else
        {
            CanSpawn = true;
            Debug.DrawLine(transform.position, Player.position, Color.green);
        }
    }
}
