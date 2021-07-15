using UnityEngine;
using System.Collections;

public class Ai : GameController {

    public GameComponent component;
    //public NavMeshAgent agent;
    public int random;
    public float randomLerph;
    public Transform target;
	// Use this for initialization
    void Awake ()
    {
        component.CreateComponent(gameObject);
    }
	void Start () 
    {
        target = Player.entity.transform;
	}

	// Update is called once per frame
    void FixedUpdate()
    {
        //agent.SetDestination(target.position);
        LookAtEnemy(target.transform.position);
    }
	void Update () 
    {
	    
	}
    void LookAtEnemy(Vector3 target)
    {

            aim = target;
            facingDirection = (aim - component.actorTransform.position);
            facingDirection.y = 0;
            Vector3 dir = component.actorTransform.forward;
            movementDirection = dir;

        //random;
        //random = Random.Range(0, 5);
        //randomLerph = Mathf.Lerp(randomLerph, random, 5*Time.deltaTime);
    }

}
