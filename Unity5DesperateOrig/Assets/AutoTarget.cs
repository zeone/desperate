using System.Collections.Generic;
using System.Linq;
using System.Security;
using JetBrains.Annotations;
using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class AutoTarget : MonoBehaviour
{


    public List<AutoTargetList> TargetList = new List<AutoTargetList>();

    public string test = "test";
    public static AutoTarget entity { get; set; }

    void Update()
    {
        if (!Weapon.entity.Target)
        {
            SwithNewTarget();
        }
        //   Debug.Log("Terhet count " + TargetList.Count);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            if (TargetList.Where(t => t.ID == col.GetInstanceID()).Count() == 0)
                TargetList.Add(new AutoTargetList() { ID = col.GetInstanceID(), Target = col.gameObject });
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy") DeleteFromTargetList(col.gameObject);

    }

    public void DeleteFromTargetList(GameObject go, string text = "null")
    {
        Debug.Log("Удаляем из списка");
        TargetList.Remove(new AutoTargetList() { ID = go.GetInstanceID(), Target = go.gameObject });
        Debug.Log(TargetList.Where(r => r.ID == go.GetInstanceID()).Count());
        Weapon.entity.Target = null;
        SwithNewTarget(go.GetInstanceID());
    }

    void SwithNewTarget(int ID = 0)
    {
        if (Weapon.entity.Target && Weapon.entity.Target.GetInstanceID() == ID && TargetList.Count != 0)
        {
            Weapon.entity.Target = TargetList.FirstOrDefault().Target;
        }
        else if (Weapon.entity.Target && Weapon.entity.Target.GetInstanceID() != ID)
        {
            return;
        }
        else if (!Weapon.entity.Target && TargetList.Count != 0)
        {
            Weapon.entity.Target = TargetList.FirstOrDefault().Target;
        }
        else
        {
            Weapon.entity.Target = null;
        }
    }

    public void Test(GameObject go)
    {
        Debug.Log("get " + go.name);
    }
}

public class AutoTargetList
{
    public int ID { get; set; }
    public GameObject Target { get; set; }
}
