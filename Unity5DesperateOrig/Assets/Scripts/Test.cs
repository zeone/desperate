using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
    [Header("Enter value")]
    public float k;
    [Header("Out value")]
    public float b;
	// Use this for initialization
	void Start () 
    {
	
	}
	void Update () 
    {
        float arctg = Mathf.Atan(k);
        float piSubSix = Mathf.PI/6;
        float modulArctg = Mathf.Abs(arctg);
        if (modulArctg >= 1)
        {
            float drob=k / Mathf.Sqrt(2);
            b = Mathf.Exp(0.6f * arctg - drob);
        }
        else if (piSubSix <= modulArctg && modulArctg < 1)
        {
            b = Mathf.Tan(modulArctg);
        }
        else if (modulArctg < piSubSix)
        {
            b = Mathf.Pow(-10, -5);
        }
        else
        {

        }
	}
}
