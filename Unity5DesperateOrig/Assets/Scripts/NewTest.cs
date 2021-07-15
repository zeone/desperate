using UnityEngine;
using System.Collections;

public class NewTest : MonoBehaviour
{
    public string Test = "Hello word";
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnParticleCollision(GameObject other)
    {


        Debug.Log("Попал в капсулу как частица" + other.name);
        // DamageOffParticle(other.tag);

    }

    void OnTriggerStay(Collider col)
    {
        Debug.Log("Stay " + col.name);
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Попал в капсулу как тригер" + col.name);
        
    }

 

}
