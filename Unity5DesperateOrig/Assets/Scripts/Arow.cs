using UnityEngine;
using System.Collections;

public class Arow : MonoBehaviour
{
    public EasyJoystick joyMove;
    Vector3 joyVecMove;
    Transform transform;
    MeshRenderer arow;
    public Material normalMatirial;
    public Material boostMatirial;
    Quaternion rotation;
    // Use this for initialization
    void Start()
    {
        joyMove = GameObject.Find("JoystickMove").GetComponent<EasyJoystick>();
        transform = gameObject.GetComponent<Transform>();
        arow = transform.FindChild("Arow").GetComponent<MeshRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (joyMove)
        {
            transform.rotation = rotation;
            joyVecMove.Set(joyMove.JoystickAxis.x, 0, joyMove.JoystickAxis.y);
            if (joyVecMove.x != 0 || joyVecMove.z != 0)
            {
                arow.enabled = true;
                rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(joyVecMove),5*Time.deltaTime);
                if(Boost.boost>1)
                {
                    if (boostMatirial.name != arow.material.name)
                    {
                        arow.material = boostMatirial;
                    }
                    
                }
                else
                {
                    if (normalMatirial.name != arow.material.name)
                    {
                        arow.material = normalMatirial;
                    }
                }
            }
            else
            {
                
                arow.enabled = false;
            }
            
        }
    }
}
