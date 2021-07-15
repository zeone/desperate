using UnityEngine;
using System.Collections;

public class DropShadow : MonoBehaviour {

	// Use this for initializatio
    public Transform light;
    public Transform startShadowPoint;
    public Transform endShadowPoint;
    private Transform shadow;
    public Transform heigh;
    public Transform foot;
    private Vector3 posEndPoint;
    private Vector3 posStartPoint;
    private Vector3 posLight;
    private Vector3 posMesh;
    private Vector3 posHeigh;
    public float h;
    public float a;
    public float lenght;
    public enum death { front, back, normal}
    public death deathType;
	void Start () 
    {
        
        shadow = transform;
        

	}
	
	// Update is called once per frame
	void Update () 
    {
        
        posLight = light.position;
        posMesh = shadow.position;
        posStartPoint = startShadowPoint.position;
        posHeigh.Set(posStartPoint.x, heigh.position.y, posStartPoint.z);
        
        a = Vector3.Distance(posLight, posStartPoint);
        posLight.y = posMesh.y;


        h = Vector3.Distance(posHeigh, posStartPoint);
        if(h<0.5f)
        {
            if (deathType.ToString() == "front")
            {

                startShadowPoint.position = heigh.position;
                endShadowPoint.position = foot.position;
            }
            if(deathType.ToString() == "back")
            {
                startShadowPoint.position = foot.position;
                endShadowPoint.position = heigh.position;
            }
        }
        else
        {
            shadow.LookAt(posLight);
            endShadowPoint.localPosition = new Vector3(0, 0, -lenght * h);
            //endShadowPoint.localScale = new Vector3(-a, a, a);
        }
        


        


	}
}
