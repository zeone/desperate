using UnityEngine;
using System.Collections;

public class RandomZombi : Actor
{
    public Material matirial;
    public float intencivity;

    public void Awake()
    {

        matirial = transform.GetComponentInChildren<SkinnedMeshRenderer>().material;
        
    }
    void Start()
    {
        matirial.SetColor("_Color1", RandomColor(Vector4.zero, new Vector4(1, 1, 1, intencivity)));
        matirial.SetColor("_Color2", RandomColor(Vector4.zero, new Vector4(1, 1, 1, intencivity)));
        matirial.SetColor("_Color3", RandomColor(Vector4.zero, new Vector4(1, 1, 1, intencivity)));
    }
    public Color RandomColor(Vector4 MinRGBA, Vector4 MaxRGBA)
    {
        Vector4 r = new Vector4(Random.Range(MinRGBA.x, MaxRGBA.x), Random.Range(MinRGBA.y, MaxRGBA.y),
            Random.Range(MinRGBA.z, MaxRGBA.z), Random.Range(MinRGBA.w, MaxRGBA.w));
        return r;
    }
}
[System.Serializable]
public class ColorRandom
{
    
}