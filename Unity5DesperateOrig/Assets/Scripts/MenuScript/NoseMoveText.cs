using UnityEngine;
using System.Collections;

public class NoseMoveText : MonoBehaviour 
{
    Transform parent;
    Transform child;
    public float noise;
    public float speed;
    public Vector3 startPos;
	// Use this for initialization

	void Start () 
    {
        
        child = transform;
        parent = child.parent.transform;
        startPos = child.localPosition;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector2 random = new Vector2(startPos.x + Random.Range(-noise, noise), startPos.y + Random.Range(-noise, noise));
        child.localPosition = Vector3.Lerp(child.localPosition, new Vector3(random.x, random.y, startPos.z), speed * Time.deltaTime);
        
	}
}
