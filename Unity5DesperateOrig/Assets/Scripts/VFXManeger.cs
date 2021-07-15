using UnityEngine;
using System.Collections;

public class VFXManeger : MonoBehaviour
{
    public Transform bloodEmit;
    public struct Blood
    {
        public static ParticleSystem[] bloodEmiters;
        public static Transform transformEmiter;
    };
    // Use this for initialization
    void Start()
    {

        Blood.bloodEmiters = bloodEmit.GetComponentsInChildren<ParticleSystem>();
        Blood.transformEmiter = bloodEmit;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
