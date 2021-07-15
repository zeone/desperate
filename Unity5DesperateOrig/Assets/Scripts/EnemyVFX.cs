using UnityEngine;
using System.Collections;

public class EnemyVFX : MonoBehaviour
{
    public bool isDeath;
    public float timeStartCutoff;
    public float speedCutoff;
    public SkinnedMeshRenderer mesh;
    float t;
    // Use this for initialization
    void Start()
    {
        mesh = GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeath)
        {
            if (Weapon.entity.isPowerfull)
            {
                timeStartCutoff -= Time.deltaTime;
                if (timeStartCutoff < 0)
                {
                    t += Time.deltaTime * speedCutoff;
                    mesh.material.SetFloat("_Cutoff", t);
                    mesh.material.SetFloat("_BurnShift", t);
                }
            }
            else
            {
                timeStartCutoff -= Time.deltaTime;
                if (timeStartCutoff < 0)
                {
                    t += Time.deltaTime * speedCutoff;
                    mesh.material.SetFloat("_Cutoff", t);
                }
            }
        }

    }
}
