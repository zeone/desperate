using UnityEngine;
using System.Collections;

public class ConfingBuffer : MonoBehaviour {

    public WeaponBuffer weaponBuffer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
[System.Serializable]
public class WeaponBuffer
{
    public Vector3 posLeftHand;
    public Vector3 rotLeftHand;
    public Vector3 posRightHand;
    public Vector3 rotRightHand;
    public Vector3 posOnParent;
}
