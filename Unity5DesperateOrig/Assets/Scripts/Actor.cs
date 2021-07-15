using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
  
    
}


[System.Serializable]
public class GameComponent
{
    public GameObject actorEntity;
    public Transform actorTransform;
    public Rigidbody actorRigbody;
    public Animation actorAnimation;
    public AudioSource actorAudio;
    
    public void CreateComponent(GameObject entity)
    {
        actorEntity = entity;
        actorTransform = entity.transform;
        if (entity.GetComponent<Rigidbody>()) actorRigbody = entity.GetComponent<Rigidbody>();
        if (entity.GetComponent<Animation>()) actorAnimation = entity.GetComponent<Animation>();
        if (entity.GetComponent<AudioSource>()) actorAudio = entity.GetComponent<AudioSource>();

    }
}

